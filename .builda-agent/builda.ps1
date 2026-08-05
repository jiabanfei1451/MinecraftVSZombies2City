$ErrorActionPreference = "Stop"

$SelfDir = Split-Path -Parent ([System.IO.Path]::GetFullPath($PSCommandPath))
# 旧命名 .builda-godot-agent（多引擎化前）由 Invoke-LegacyProjectDirMigrate / Invoke-LegacyAuthMigrate 自动迁移
$AuthDir = if ($env:BUILDA_AUTH_DIR) { [System.IO.Path]::GetFullPath($env:BUILDA_AUTH_DIR) } else { Join-Path $HOME ".builda-agent" }
$LegacyAuthDir = Join-Path $HOME ".builda-godot-agent"
$ProjectStateDir = if ($env:BUILDA_PROJECT_STATE_DIR) { $env:BUILDA_PROJECT_STATE_DIR } else { ".builda-agent" }
$LegacyProjectStateDir = ".builda-godot-agent"
$Root = $ProjectStateDir
$EnvPath = Join-Path $Root "publish.env"
$TokenPath = Join-Path $AuthDir "token"
$RefreshPath = Join-Path $AuthDir "refresh-token"
$Manifest = if ($env:BUILDA_MANIFEST) { $env:BUILDA_MANIFEST } else { "builda.publish.json" }
$ProjectFile = if ($env:BUILDA_PROJECT_FILE) { $env:BUILDA_PROJECT_FILE } else { Join-Path $ProjectStateDir "game.json" }
$LegacyProjectFile = if ($env:BUILDA_LEGACY_PROJECT_FILE) { $env:BUILDA_LEGACY_PROJECT_FILE } else { "builda.game.json" }
$Version = "0.3.1"
$RuntimeVersionFile = Join-Path $Root "VERSION"
$SdkVersionFile = Join-Path $ProjectStateDir "sdk-version"
$DefaultBase = "https://builda-godot-api.poni.fun"
if ($DefaultBase -like "{{*") {
  $DefaultBase = ""
}
$Base = $DefaultBase
$DefaultAgentBase = "https://ai.builda.game"
if ($DefaultAgentBase -like "{{*") {
  $DefaultAgentBase = $Base
}
$AgentBase = $DefaultAgentBase
$DefaultSiteBase = "https://builda.game"
if ($DefaultSiteBase -like "{{*") {
  $DefaultSiteBase = "https://builda.game"
}
$SiteOrigin = ""
$EarlyAccessUrl = ""
$PairCode = ""

if (Test-Path $EnvPath) {
  Get-Content $EnvPath | ForEach-Object {
    if ($_ -match "^BUILDA_BASE=(.*)$") { $script:Base = $Matches[1].Trim() }
    if ($_ -match "^BUILDA_AGENT_BASE=(.*)$") { $script:AgentBase = $Matches[1].Trim() }
    if ($_ -match "^BUILDA_SITE_ORIGIN=(.*)$") { $script:SiteOrigin = $Matches[1].Trim() }
    if ($_ -match "^BUILDA_PAIR_CODE=(.*)$") { $script:PairCode = $Matches[1].Trim() }
  }
}
$EffectiveSiteBase = if ($SiteOrigin) { $SiteOrigin } else { $DefaultSiteBase }
$EarlyAccessUrl = if ($env:BUILDA_EARLY_ACCESS_URL) { $env:BUILDA_EARLY_ACCESS_URL } else { "$EffectiveSiteBase/early-access" }

function Show-Usage {
  Write-Host "builda auth [code|--force]      Ensure agent authorization; --force switches account."
  Write-Host "builda check                    Check local BuildaGame agent environment."
  Write-Host "builda update                   Update this local CLI and installed agent skill."
  Write-Host "builda uninstall [--purge]      Remove project toolchain/registrations; --purge also removes project data and per-user credentials."
  Write-Host "builda engine detect [dir]      Detect project engine; Godot, HTML5 and Unity draft upload are available."
  Write-Host "builda new-manifest             Write builda.publish.json template if missing."
  Write-Host "builda dev --web <dir|zip>      Run a local H5 build (Godot Web / pixi / phaser / Unity WebGL ...) with the Builda mock SDK."
  Write-Host "builda bundle-check <zip>       Check that an H5 Bundle zip is uploadable (per-engine rules)."
  Write-Host "builda sdk init                 Create a minimal Godot project skeleton for SDK testing."
  Write-Host "builda sdk install              Install the Builda SDK (Godot: addons/builda; HTML5: .builda-agent/sdk/web; Unity: Assets/Builda)."
  Write-Host "builda sdk check                Check installed Builda SDK files in this project."
  Write-Host "builda sdk smoke                Run local mock SDK smoke test when Node.js exists."
  Write-Host "builda upload-build <zip>       Upload H5 Bundle zip; update manifest."
  Write-Host "builda assets check <zip>       Check assets.zip: only audio/**/*.mp3|ogg|wav."
  Write-Host "builda assets upload <zip>      Upload audio assets; update manifest."
  Write-Host "builda create-draft [manifest]  Sync a new cysj pending draft and update the Builda current version."
  Write-Host "builda update-draft [game_id] [manifest]"
  Write-Host "                               Sync a cysj pending draft and update the Builda current version."
  Write-Host "builda status                   Show saved receipts."
  Write-Host "builda sync-project             Re-write project-level agent registrations (.gitignore/stubs/AGENTS.md)."
  Write-Host "builda version                  Show local CLI version."
  Write-Host ""
  Write-Host "Project-scoped toolchain: this CLI lives in <project>/.builda-agent/ and must run"
  Write-Host 'from that project''s root. Credentials stay per-user in $HOME/.builda-agent/.'
}

function Need-Token {
  if (Grant-NeedsRefresh) {
    try { Refresh-Grant | Out-Null } catch {}
  }
  if (!(Test-Path $TokenPath)) {
    throw "No agent grant. Run: builda auth"
  }
}

function Get-Token {
  (Get-Content $TokenPath -Raw).Trim()
}

function Get-CmdUrl {
  param([string]$Path)
  # Game_* commands are mandarin cmd-over-HTTP routes mounted at /game/*.
  # Agent REST helpers such as upload/auth live under /api/v1/*.
  "$Base/$Path"
}

# Get-EngineValue 静默判定引擎（小写，与后端/协议口径一致）：godot 优先；
# unity = ProjectSettings/ProjectVersion.txt 或 Assets/+Packages/manifest.json；
# HTML5 = 根目录有 index.html 或 package.json。
function Get-EngineValue {
  param([string]$Dir = ".")
  if (Test-Path (Join-Path $Dir "project.godot")) { return "godot" }
  if ((Test-Path (Join-Path $Dir "ProjectSettings/ProjectVersion.txt")) -or ((Test-Path (Join-Path $Dir "Assets")) -and (Test-Path (Join-Path $Dir "Packages/manifest.json")))) { return "unity" }
  if ((Test-Path (Join-Path $Dir "index.html")) -or (Test-Path (Join-Path $Dir "package.json"))) { return "h5" }
  return "unknown"
}

# Get-ManifestEngine 引擎单一来源：manifest engine 字段优先，缺失按目录判定，非法兜底 godot
function Get-ManifestEngine {
  param([string]$File = $Manifest)
  $V = ""
  if (Test-Path $File) {
    try {
      $Data = Get-Content $File -Raw | ConvertFrom-Json
      if ($Data.engine) { $V = [string]$Data.engine }
    } catch {}
  }
  if (!$V) { $V = Get-EngineValue "." }
  if ($V -notin @("godot", "h5", "unity")) { $V = "godot" }
  return $V
}

function Invoke-EngineDetect {
  param([string]$Dir = ".")
  if (!(Test-Path $Dir -PathType Container)) {
    Write-Host "engine-detect=failed"
    throw "directory not found: $Dir"
  }
  $Engine = Get-EngineValue $Dir
  Write-Host "engine=$Engine"
  switch ($Engine) {
    { $_ -in @("godot", "h5", "unity") } {
      Write-Host "engine-upload-support=available"
      return
    }
    default {
      Write-Host "engine-upload-support=unknown"
      Write-Error "Project engine could not be detected. Do not upload until the user points you at a Godot, HTML5 or Unity project root."
      exit 1
    }
  }
}

function Show-AuthFailureHint {
  Write-Error "auth-required=1"
  if (Test-Path $RefreshPath) {
    Write-Error "Agent grant is missing or expired. Retry the failed command; saved agent authorization refreshes it automatically when still valid."
  } else {
    Write-Error "Agent grant is missing or expired. Run: builda auth"
  }
}

function Get-StubBody {
  @(
    "本项目已安装项目级 BuildaGame 工具链（发布 Godot Web / HTML5 / Unity WebGL 游戏到 Builda）。工具、说明书、SDK 全部随项目走，同一版本号："
    ""
    '1. 完整说明书（先读）：项目根的 `.builda-agent/SKILL.md`'
    '2. 每次任务开始先跑：`./.builda-agent/builda check`（自动对齐工具链版本，并提示 SDK 是否需要同步）'
    ""
    "若上述文件缺失（例如刚 clone 本仓库），在项目根目录重装："
    ""
    '```bash'
    "curl -fsSL $AgentBase/agent/install.sh | bash"
    '```'
  ) -join "`n"
}

function Write-StateGitignore {
  New-Item -ItemType Directory -Force -Path $Root | Out-Null
  @'
# BuildaGame 自动管理：以下为可再生/敏感文件，不入库。
# game.json 与 sdk-version 是项目数据，保持入库。
builda
builda.ps1
builda.cmd
SKILL.md
VERSION
AGENT_INSTALL.md
publish.env
md_*.md
last-*
key-audit-snapshot
sdk/
token
refresh-token
'@ | Set-Content -Encoding UTF8 (Join-Path $Root ".gitignore")
}

$AgentsBlockBegin = "<!-- BEGIN builda (auto-managed) -->"
$AgentsBlockEnd = "<!-- END builda -->"
# 多引擎化前的旧标记块（builda-godot）：写新块前先清掉，避免双块并存
$LegacyAgentsBlockBegin = "<!-- BEGIN builda-godot (auto-managed) -->"
$LegacyAgentsBlockEnd = "<!-- END builda-godot -->"

function Remove-AgentsMdBlockByMarkers {
  param([string]$Begin, [string]$End)
  $Path = "AGENTS.md"
  if (!(Test-Path $Path)) { return $false }
  $Content = Get-Content $Path -Raw
  if ($Content -notlike "*$Begin*") { return $false }
  $Pattern = "(?ms)^$([regex]::Escape($Begin))\r?\n.*?^$([regex]::Escape($End))\r?\n?"
  $Content = [regex]::Replace($Content, $Pattern, "")
  Set-Content -Encoding UTF8 -Path $Path -Value $Content -NoNewline
  return $true
}

function Write-AgentsMdBlock {
  $Path = "AGENTS.md"
  Remove-AgentsMdBlockByMarkers $LegacyAgentsBlockBegin $LegacyAgentsBlockEnd | Out-Null
  if ((Test-Path $Path) -and ((Get-Content $Path -Raw) -like "*$AgentsBlockBegin*")) { return }
  $Prefix = if ((Test-Path $Path) -and (Get-Item $Path).Length -gt 0) { "`n" } else { "" }
  $Block = "$AgentsBlockBegin`n## BuildaGame 发布工具（项目级）`n`n$(Get-StubBody)`n$AgentsBlockEnd`n"
  Add-Content -Encoding UTF8 -Path $Path -Value "$Prefix$Block" -NoNewline
}

function Remove-AgentsMdBlock {
  if (Remove-AgentsMdBlockByMarkers $LegacyAgentsBlockBegin $LegacyAgentsBlockEnd) { Write-Host "removed=AGENTS.md builda-godot block" }
  if (Remove-AgentsMdBlockByMarkers $AgentsBlockBegin $AgentsBlockEnd) { Write-Host "removed=AGENTS.md builda block" }
}

function Write-AgentInstallMd {
  @"
# BuildaGame Agent Installed (project-scoped)

Runtime (all inside this project):

- CLI: ./.builda-agent/builda
- Skill: ./.builda-agent/SKILL.md
- Env: ./.builda-agent/publish.env
- Installed version: $Version (CLI/skill/SDK share this single version)

Credentials stay per-user in `$HOME/.builda-agent/ (token / refresh-token only).

Next for SDK-only tasks:

    ./.builda-agent/builda check
    ./.builda-agent/builda auth
    ./.builda-agent/builda sdk install
    ./.builda-agent/builda sdk check
    ./.builda-agent/builda sdk smoke

Next for upload-draft tasks:

    ./.builda-agent/builda check
    ./.builda-agent/builda auth

Game identity lives in ./.builda-agent/game.json (committed with the project).
"@ | Set-Content -Encoding UTF8 (Join-Path $Root "AGENT_INSTALL.md")
}

function Remove-LegacyProjectStubs {
  # 多引擎化前的旧 stub 名（builda-godot）：每次同步注册时顺手清理，避免双注册
  foreach ($Path in @(".claude/skills/builda-godot", ".cursor/rules/builda-godot.mdc", ".clinerules/builda-godot.md", ".roo/rules/builda-godot.md", ".windsurf/rules/builda-godot.md")) {
    if (Test-Path $Path) { Remove-Item -Recurse -Force $Path -ErrorAction SilentlyContinue }
  }
}

function Sync-ProjectRegistrations {
  $Registrations = New-Object System.Collections.Generic.List[string]
  Write-StateGitignore
  Write-AgentInstallMd
  Remove-LegacyProjectStubs
  Write-AgentsMdBlock
  $Registrations.Add("AGENTS.md: builda block")

  $ClaudeHome = Join-Path $HOME ".claude"
  if ((Test-Path $ClaudeHome) -or (Test-Path ".claude")) {
    $ClaudeSkillRoot = ".claude/skills/builda"
    New-Item -ItemType Directory -Force -Path $ClaudeSkillRoot | Out-Null
    "---`nname: builda`ndescription: BuildaGame（Builda）游戏发布工具链：接入 Builda SDK、本地 mock 调试、导出 Godot Web / Unity WebGL 或打包 HTML5 构建、上传/更新游戏草稿。`n---`n`n$(Get-StubBody)`n" | Set-Content -Encoding UTF8 (Join-Path $ClaudeSkillRoot "SKILL.md") -NoNewline
    $Registrations.Add("Claude Code: .claude/skills/builda/SKILL.md")
  }

  $CursorHome = Join-Path $HOME ".cursor"
  if ((Test-Path $CursorHome) -or (Test-Path ".cursor")) {
    $CursorRuleRoot = ".cursor/rules"
    New-Item -ItemType Directory -Force -Path $CursorRuleRoot | Out-Null
    $CursorRulePath = Join-Path $CursorRuleRoot "builda.mdc"
    "---`ndescription: BuildaGame skill`nalwaysApply: false`n---`n`n$(Get-StubBody)`n" | Set-Content -Encoding UTF8 $CursorRulePath -NoNewline
    $Registrations.Add("Cursor: .cursor/rules/builda.mdc")
  }

  $ClineGlobalRoot = Join-Path $HOME "Documents\Cline\Rules"
  if ((Test-Path ".clinerules") -or (Test-Path $ClineGlobalRoot)) {
    New-Item -ItemType Directory -Force -Path ".clinerules" | Out-Null
    Get-StubBody | Set-Content -Encoding UTF8 ".clinerules/builda.md"
    $Registrations.Add("Cline: .clinerules/builda.md")
  }

  if ((Test-Path ".roo") -or (Test-Path ".roo/rules")) {
    $RooRulesRoot = ".roo/rules"
    New-Item -ItemType Directory -Force -Path $RooRulesRoot | Out-Null
    $RooRulePath = Join-Path $RooRulesRoot "builda.md"
    Get-StubBody | Set-Content -Encoding UTF8 $RooRulePath
    $Registrations.Add("Roo Code: .roo/rules/builda.md")
  }

  if ((Test-Path ".windsurf") -or (Test-Path ".windsurf/rules")) {
    $WindsurfRulesRoot = ".windsurf/rules"
    New-Item -ItemType Directory -Force -Path $WindsurfRulesRoot | Out-Null
    $WindsurfRulePath = Join-Path $WindsurfRulesRoot "builda.md"
    "---`ndescription: BuildaGame skill`nalwaysApply: false`n---`n`n$(Get-StubBody)`n" | Set-Content -Encoding UTF8 $WindsurfRulePath -NoNewline
    $Registrations.Add("Windsurf: .windsurf/rules/builda.md")
  }

  foreach ($Registration in $Registrations) { Write-Host "- $Registration" }
}

function Remove-IfExists {
  param([string]$Path)
  if (Test-Path $Path) {
    Remove-Item -Recurse -Force $Path
    Write-Host "removed=$Path"
  }
}

function Remove-LegacyGlobalRegistrations {
  $CodexHome = if ($env:CODEX_HOME) { $env:CODEX_HOME } else { Join-Path $HOME ".codex" }
  Remove-IfExists (Join-Path $CodexHome "skills/builda-godot")
  Remove-IfExists (Join-Path $HOME ".claude/skills/builda-godot")
  Remove-IfExists (Join-Path $HOME ".cursor/rules/builda-godot.mdc")
  Remove-IfExists (Join-Path $HOME "Documents/Cline/Rules/builda-godot.md")
}

function Invoke-LegacyHomeMigrate {
  param([string[]]$OriginalArgs)
  $TargetRoot = Join-Path (Get-Location).Path $ProjectStateDir
  [Console]::Error.WriteLine('layout-migration=required (toolchain moved from $HOME to per-project)')
  if (((Get-Location).Path -eq [System.IO.Path]::GetFullPath($HOME)) -or ([System.IO.Path]::GetFullPath($TargetRoot) -eq $SelfDir)) {
    [Console]::Error.WriteLine("BuildaGame 工具链已改为项目级：请先 cd 到游戏项目根目录，再重跑原命令，")
    [Console]::Error.WriteLine('迁移会自动完成（凭证仍留在 $HOME 的凭证目录）。')
    exit 1
  }
  New-Item -ItemType Directory -Force -Path $TargetRoot | Out-Null
  foreach ($Name in @("builda.ps1", "SKILL.md", "VERSION", "publish.env")) {
    $Source = Join-Path $SelfDir $Name
    if (Test-Path $Source) { Copy-Item -Force $Source (Join-Path $TargetRoot $Name) }
  }
  Sync-ProjectRegistrations *> $null
  foreach ($Name in @("builda", "builda.ps1", "builda.cmd", "SKILL.md", "VERSION", "AGENT_INSTALL.md", "publish.env", "md_cysj_app.md", "md_cjk_font.md", "md_mobile_perf.md", "last-build.json", "last-assets.json", "last-draft.json", "last-game-id", "last-draft-id", "last-version-id", "key-audit-snapshot")) {
    $Path = Join-Path $SelfDir $Name
    if (Test-Path $Path) { Remove-Item -Force -Recurse $Path }
  }
  Remove-LegacyGlobalRegistrations
  [Console]::Error.WriteLine("layout-migration=done target=$TargetRoot (credentials kept in $AuthDir)")
  & (Join-Path $TargetRoot "builda.ps1") @OriginalArgs
  exit $LASTEXITCODE
}

function Invoke-LegacyProjectDirMigrate {
  # 本体躺在旧命名的项目目录 .builda-godot-agent/：搬进 .builda-agent/、重写 stub、删旧目录、接力原命令
  param([string[]]$OriginalArgs)
  $TargetRoot = Join-Path (Get-Location).Path $ProjectStateDir
  [Console]::Error.WriteLine("layout-migration=required (toolchain dir renamed $LegacyProjectStateDir -> $ProjectStateDir)")
  $Expected = Join-Path (Get-Location).Path $LegacyProjectStateDir
  $Resolved = if (Test-Path $Expected) { (Resolve-Path $Expected).Path } else { "" }
  if ($Resolved -ne $SelfDir) {
    [Console]::Error.WriteLine("run-from=project-root-required")
    [Console]::Error.WriteLine("本 CLI 是项目级工具，必须在它所属的项目根目录运行迁移：")
    [Console]::Error.WriteLine("  cd $(Split-Path -Parent $SelfDir) && ./$LegacyProjectStateDir/builda.ps1 <command>")
    exit 1
  }
  New-Item -ItemType Directory -Force -Path $TargetRoot | Out-Null
  foreach ($Name in @("builda", "builda.ps1", "builda.cmd", "SKILL.md", "VERSION", "publish.env", "game.json", "sdk-version",
                      "last-build.json", "last-assets.json", "last-draft.json", "last-game-id", "last-draft-id", "last-version-id", "key-audit-snapshot",
                      "md_cysj_app.md", "md_cjk_font.md", "md_mobile_perf.md", "md_rankboards.md")) {
    $Source = Join-Path $SelfDir $Name
    if (Test-Path $Source) { Copy-Item -Force $Source (Join-Path $TargetRoot $Name) }
  }
  $SdkDir = Join-Path $SelfDir "sdk"
  if (Test-Path $SdkDir) { Copy-Item -Recurse -Force $SdkDir (Join-Path $TargetRoot "sdk") }
  Sync-ProjectRegistrations *> $null
  Remove-Item -Recurse -Force $SelfDir
  [Console]::Error.WriteLine("layout-migration=done target=$TargetRoot")
  & powershell -NoProfile -ExecutionPolicy Bypass -File (Join-Path $TargetRoot "builda.ps1") @OriginalArgs
  exit $LASTEXITCODE
}

function Invoke-LegacyAuthMigrate {
  # 凭证目录改名迁移：复制不删除——旧 CLI 仍读旧目录；uninstall --purge 两代一起清
  if ($env:BUILDA_AUTH_DIR) { return }
  if (Test-Path $TokenPath) { return }
  $LegacyToken = Join-Path $LegacyAuthDir "token"
  if (!(Test-Path $LegacyToken)) { return }
  New-Item -ItemType Directory -Force -Path $AuthDir | Out-Null
  Copy-Item -Force $LegacyToken $TokenPath
  $LegacyRefresh = Join-Path $LegacyAuthDir "refresh-token"
  if ((Test-Path $LegacyRefresh) -and !(Test-Path $RefreshPath)) { Copy-Item -Force $LegacyRefresh $RefreshPath }
  [Console]::Error.WriteLine("auth-migration=copied credentials $LegacyAuthDir -> $AuthDir (legacy kept for old CLIs)")
}

function Require-ProjectContext {
  param([string[]]$OriginalArgs)
  if (($SelfDir -eq [System.IO.Path]::GetFullPath($AuthDir)) -or ($SelfDir -eq [System.IO.Path]::GetFullPath($LegacyAuthDir))) { Invoke-LegacyHomeMigrate $OriginalArgs }
  if (((Split-Path -Leaf $SelfDir) -eq $LegacyProjectStateDir) -and ($ProjectStateDir -ne $LegacyProjectStateDir)) {
    Invoke-LegacyProjectDirMigrate $OriginalArgs
  }
  $Expected = Join-Path (Get-Location).Path $ProjectStateDir
  $Resolved = if (Test-Path $Expected) { (Resolve-Path $Expected).Path } else { "" }
  if ($Resolved -ne $SelfDir) {
    [Console]::Error.WriteLine("run-from=project-root-required")
    [Console]::Error.WriteLine("本 CLI 是项目级工具，必须在它所属的项目根目录运行：")
    [Console]::Error.WriteLine("  cd $(Split-Path -Parent $SelfDir) && ./$ProjectStateDir/builda.ps1 <command>")
    exit 1
  }
}

function Invoke-Uninstall {
  param([string[]]$Options)
  $Purge = $false
  if ($Options.Count -gt 0) {
    if ($Options[0] -eq "--purge") {
      $Purge = $true
    } elseif ($Options[0] -eq "-h" -or $Options[0] -eq "--help" -or $Options[0] -eq "help") {
      Write-Host "Usage: builda uninstall [--purge]"
      Write-Host "Default removes the project toolchain and agent registrations, keeps game.json/sdk-version"
      Write-Host 'and the per-user credentials in $HOME/.builda-agent/.'
      Write-Host "--purge also removes the whole $Root directory AND per-user credentials"
      Write-Host "(credentials are shared by every project on this machine)."
      return
    } else {
      throw "unknown uninstall option: $($Options[0])"
    }
  }

  Remove-IfExists ".claude/skills/builda"
  Remove-IfExists ".cursor/rules/builda.mdc"
  Remove-IfExists ".clinerules/builda.md"
  Remove-IfExists ".roo/rules/builda.md"
  Remove-IfExists ".windsurf/rules/builda.md"
  Remove-IfExists ".claude/skills/builda-godot"
  Remove-IfExists ".cursor/rules/builda-godot.mdc"
  Remove-IfExists ".clinerules/builda-godot.md"
  Remove-IfExists ".roo/rules/builda-godot.md"
  Remove-IfExists ".windsurf/rules/builda-godot.md"
  Remove-AgentsMdBlock
  Remove-LegacyGlobalRegistrations

  if ($Purge) {
    Remove-IfExists $Root
    Remove-IfExists $LegacyProjectStateDir
    Remove-IfExists $TokenPath
    Remove-IfExists $RefreshPath
    Remove-IfExists (Join-Path $LegacyAuthDir "token")
    Remove-IfExists (Join-Path $LegacyAuthDir "refresh-token")
    Write-Host "uninstall=ok"
    Write-Host "uninstall-mode=purge (credentials removed for ALL projects on this machine)"
    return
  }

  Remove-IfExists (Join-Path $Root "SKILL.md")
  Remove-IfExists (Join-Path $Root "md_cysj_app.md")
  Remove-IfExists (Join-Path $Root "md_cjk_font.md")
  Remove-IfExists (Join-Path $Root "md_mobile_perf.md")
  Remove-IfExists (Join-Path $Root "builda")
  Remove-IfExists (Join-Path $Root "builda.ps1")
  Remove-IfExists (Join-Path $Root "builda.cmd")
  Remove-IfExists (Join-Path $Root "VERSION")
  Remove-IfExists (Join-Path $Root "AGENT_INSTALL.md")
  Remove-IfExists (Join-Path $Root "publish.env")
  foreach ($Name in @("last-build.json", "last-assets.json", "last-draft.json", "last-game-id", "last-draft-id", "last-version-id", "key-audit-snapshot")) { Remove-IfExists (Join-Path $Root $Name) }
  Write-Host "uninstall=ok"
  Write-Host "uninstall-mode=keep-project-data (game.json/sdk-version kept; credentials kept in $AuthDir)"
  if (Test-Path $TokenPath) { Write-Host "grant=kept" }
  if (Test-Path $RefreshPath) { Write-Host "agent-authorization=kept" }
}

function Update-BuildaTool {
  param([string]$NewVersion = $Version)
  New-Item -ItemType Directory -Force -Path $Root | Out-Null
  Invoke-WebRequest -UseBasicParsing "$AgentBase/agent/builda-skill.md" -OutFile (Join-Path $Root "SKILL.md")
  Invoke-WebRequest -UseBasicParsing "$AgentBase/agent/builda.ps1" -OutFile (Join-Path $Root "builda.ps1")
  $NewVersion | Set-Content -Encoding ASCII $RuntimeVersionFile
  Write-Host "self-update=updated"
  Write-Host "Agent registrations:"
  Sync-ProjectRegistrations
}

function Check-SelfUpdate {
  Write-Host "builda-version=$Version"
  Write-Host "toolchain-version=$Version"
  $EffectiveVersion = $Version
  try {
    $Remote = Invoke-RestMethod -Method Get -Uri "$AgentBase/agent/version"
    $RemoteVersion = $Remote.version
    if (!$RemoteVersion) {
      Write-Host "remote-version=unknown"
      Write-Host "self-update=skipped"
    } elseif ($RemoteVersion -ne $Version) {
      Write-Host "remote-version=$RemoteVersion"
      Update-BuildaTool -NewVersion $RemoteVersion
      $EffectiveVersion = $RemoteVersion
    } else {
      Write-Host "remote-version=$RemoteVersion"
      Write-Host "self-update=current"
    }
  } catch {
    Write-Host "remote-version=unknown"
    Write-Host "self-update=skipped"
  }
  if (Test-Path (Join-Path $Root "SKILL.md")) { Write-Host "skill=installed" } else { Write-Host "skill=missing (run: builda update)" }
  if (Test-Path $SdkVersionFile) {
    $SdkVersion = (Get-Content $SdkVersionFile -TotalCount 1).Trim()
    Write-Host "sdk-version=$SdkVersion"
    if ($SdkVersion -eq $EffectiveVersion) {
      Write-Host "sdk-sync=current"
    } else {
      Write-Host "sdk-sync=behind (SDK 与工具链同一版本号，对齐: builda sdk install)"
    }
  } elseif ((Test-Path "addons/builda") -or (Test-Path (Join-Path $ProjectStateDir "sdk/web"))) {
    Write-Host "sdk-version=unknown (missing $SdkVersionFile; run: builda sdk install)"
  } else {
    Write-Host "sdk=not-installed (仅上传任务可忽略；接 SDK 用: builda sdk install)"
  }
}

function Maybe-SelfUpdate {
  param([string[]]$OriginalArgs)
  $First = if ($OriginalArgs.Count -gt 0) { $OriginalArgs[0] } else { "" }
  if ($First -in @("", "-h", "--help", "help", "check", "update", "uninstall", "version", "--version", "-v")) { return }
  if ($env:BUILDA_DISABLE_AUTO_UPDATE -eq "1") { return }
  try {
    $Remote = Invoke-RestMethod -Method Get -Uri "$AgentBase/agent/version"
    $RemoteVersion = $Remote.version
    if ($RemoteVersion -and $RemoteVersion -ne $Version) {
      Write-Error "self-update-required=1" -ErrorAction Continue
      Update-BuildaTool -NewVersion $RemoteVersion
      Write-Error "builda-agent-skill-updated=1" -ErrorAction Continue
      Write-Error "action=rerun-current-command-with-updated-cli-and-reread-skill" -ErrorAction Continue
      $Updated = Join-Path $Root "builda.ps1"
      & powershell -NoProfile -ExecutionPolicy Bypass -File $Updated @OriginalArgs
      exit $LASTEXITCODE
    }
  } catch {
    Write-Error "self-update-check=skipped" -ErrorAction Continue
  }
}

function Get-ProjectGameId {
  Migrate-LegacyProjectFile
  if (!(Test-Path $ProjectFile)) { return "" }
  try {
    $Data = Get-Content $ProjectFile -Raw | ConvertFrom-Json
    if ($Data.gameId) { return [string]$Data.gameId }
  } catch {
    return ""
  }
  return ""
}

function Migrate-LegacyProjectFile {
  if (!(Test-Path $ProjectFile) -and (Test-Path $LegacyProjectFile)) {
    $Parent = Split-Path -Parent $ProjectFile
    if ($Parent) {
      New-Item -ItemType Directory -Force -Path $Parent | Out-Null
    }
    Copy-Item -Force $LegacyProjectFile $ProjectFile
  }
}

function Get-ManifestTitle {
  param([string]$Path)
  if (!(Test-Path $Path)) { return "" }
  try {
    $Data = Get-Content $Path -Raw | ConvertFrom-Json
    if ($Data.title) { return [string]$Data.title }
  } catch {
    return ""
  }
  return ""
}

function Write-ProjectGame {
  param([string]$GameId, [string]$Title = "", [string]$VersionId = "")
  if (!$GameId) { return }
  $Parent = Split-Path -Parent $ProjectFile
  if ($Parent) {
    New-Item -ItemType Directory -Force -Path $Parent | Out-Null
  }
  @{
    provider = "builda-h5"
    gameId = $GameId
    title = $Title
    lastVersionId = $VersionId
    updatedAt = [int64](([DateTimeOffset]::UtcNow.ToUnixTimeMilliseconds()))
  } | ConvertTo-Json -Depth 8 | Set-Content -Encoding UTF8 $ProjectFile
  Write-Host "project-binding=$ProjectFile"
}

function Write-DefaultManifest {
  if (Test-Path $Manifest) {
    Write-Host "$Manifest already exists"
    return
  }
  # engine 按目录判定写入 manifest；unknown 兜底 godot（与后端"空=godot"口径一致）
  $DetectedEngine = Get-EngineValue "."
  if ($DetectedEngine -notin @("godot", "h5", "unity")) { $DetectedEngine = "godot" }
  @{
    title = "Your game"
    tagline = "One short sentence about the game."
    desc = "What players should know before playing."
    category = "Arcade"
    engine = $DetectedEngine
    tags = @($DetectedEngine)
    orientation = "landscape"
    minChromeMajor = 0
    minIOSMajor = 0
    coverKind = "palette"
    coverPalette = "ember"
    coverUrl = ""
    buildPrefix = ""
    buildEntry = "index.html"
    buildSize = 0
    bundleUrl = ""
    bundleMd5 = ""
    bundleVersion = ""
    bundleEntry = "index.html"
    bundleSize = 0
    assetsVersion = ""
    assetsBaseUrl = ""
    assetsManifestUrl = ""
  } | ConvertTo-Json -Depth 8 | Set-Content -Encoding UTF8 $Manifest
  Write-Host 'For in-app purchases, add payPoints only when needed, for example:'
  Write-Host '  "payPoints": [{"payId":"coin_pack_1","payName":"Coins Pack","price":100}]'
  Write-Host 'Omit payPoints entirely for games without purchases; do not write an empty array.'
  Write-Host 'For leaderboards, omit rankBoards to keep existing boards; use [] only to clear all boards.'
  Write-Host '  "rankBoards": [{"rankId":"high_score","displayName":"High Score","sortType":"desc","cycleType":"forever","minScore":0,"maxScore":9999999999}]'
  Write-Host 'minChromeMajor/minIOSMajor gate which devices may play: fill the LOWEST browser majors the build actually runs on (lower = more players; the 0 placeholders are rejected until you assess and fill them).'
  Write-Host 'Judge by what the build really requires: WASM engine exports (e.g. Godot 4 Web, Unity WebGL) inherit their floor from the compiler toolchain (post-MVP WASM features land around Chrome 75-85 / iOS 15) - check the engine or Emscripten minimum-browser docs, and fall back to the platform baseline 80/15 when you cannot verify lower; plain canvas/DOM H5 with down-leveled JS can go much lower. Do not inflate the values, and do not claim majors the build cannot actually run on.'
  Write-Host "Wrote $Manifest"
}

function Assert-ManifestPayPoints {
  param([string]$File)
  if (!(Test-Path $File)) { return }
  try {
    $Data = Get-Content $File -Raw | ConvertFrom-Json
  } catch {
    throw "manifest json invalid: $File"
  }
  $Names = @($Data.PSObject.Properties.Name)
  if ($Names -notcontains "payPoints") { return }
  $Points = @($Data.payPoints)
  if ($null -eq $Data.payPoints -or $Points.Count -eq 0) {
    throw "payPoints must be omitted for games without in-app purchases; do not send an empty array."
  }
  $Seen = @{}
  for ($I = 0; $I -lt $Points.Count; $I++) {
    $Point = $Points[$I]
    if ($null -eq $Point) { throw "payPoints[$I] must be an object with payId/payName/price." }
    if ($Point.payId -isnot [string]) { throw "payPoints[$I].payId must be a string matching [A-Za-z0-9_-] with 1-64 bytes." }
    $PayId = $Point.payId
    $PayName = [string]$Point.payName
    if ([string]::IsNullOrWhiteSpace($PayId)) { throw "payPoints[$I].payId is required." }
    if ($PayId -notmatch '^[A-Za-z0-9_-]{1,64}$') { throw "payPoints[$I].payId must match [A-Za-z0-9_-] and be 1-64 bytes." }
    if ($Seen.ContainsKey($PayId)) { throw "payPoints payId duplicated: $PayId" }
    $Seen[$PayId] = $true
    if ([string]::IsNullOrWhiteSpace($PayName)) { throw "payPoints[$I].payName is required." }
    if ($null -eq $Point.price -or $Point.price -isnot [int]) { throw "payPoints[$I].price must be an integer G-coin amount." }
  }
}

function ConvertTo-ManifestSafeInteger {
  param($Value, [string]$Field)
  $IsIntegral = $Value -is [sbyte] -or $Value -is [byte] -or
    $Value -is [int16] -or $Value -is [uint16] -or
    $Value -is [int32] -or $Value -is [uint32] -or
    $Value -is [int64] -or $Value -is [uint64]
  if (!$IsIntegral) { throw "$Field must be an integer." }
  $Number = [decimal]$Value
  if ([decimal]::Truncate($Number) -ne $Number) { throw "$Field must be an integer." }
  if ($Number -lt -9999999999 -or $Number -gt 9999999999) {
    throw "$Field must be within the supported range [-9999999999, 9999999999]."
  }
  return [int64]$Number
}

function Assert-ManifestRankBoards {
  param([string]$File)
  if (!(Test-Path $File)) { return }
  try {
    $Data = Get-Content $File -Raw | ConvertFrom-Json
  } catch {
    throw "manifest json invalid: $File"
  }
  $Names = @($Data.PSObject.Properties.Name)
  if ($Names -notcontains "rankBoards") { return }
  if ($null -eq $Data.rankBoards) {
    throw "rankBoards must be an array; omit it to keep existing boards or use [] to clear all boards."
  }
  if ($Data.rankBoards -isnot [array]) {
    throw "rankBoards must be an array; omit it to keep existing boards or use [] to clear all boards."
  }
  $Boards = @($Data.rankBoards)
  if ($Boards.Count -gt 5) { throw "rankBoards supports at most 5 boards." }
  $Required = @("rankId", "displayName", "sortType", "cycleType", "minScore", "maxScore")
  $Seen = [System.Collections.Generic.HashSet[string]]::new([System.StringComparer]::Ordinal)
  for ($I = 0; $I -lt $Boards.Count; $I++) {
    $Board = $Boards[$I]
    if ($null -eq $Board -or $Board -isnot [pscustomobject]) {
      throw "rankBoards[$I] must be an object with all six fields."
    }
    $BoardNames = @($Board.PSObject.Properties.Name)
    foreach ($Field in $Required) {
      if ($BoardNames -notcontains $Field) { throw "rankBoards[$I] missing required field: $Field" }
    }
    if ($Board.rankId -isnot [string]) { throw "rankBoards[$I].rankId must be a string." }
    if ($Board.displayName -isnot [string]) { throw "rankBoards[$I].displayName must be a string." }
    if ($Board.sortType -isnot [string]) { throw "rankBoards[$I].sortType must be a string." }
    if ($Board.cycleType -isnot [string]) { throw "rankBoards[$I].cycleType must be a string." }
    $RankId = $Board.rankId
    $DisplayName = $Board.displayName
    $SortType = $Board.sortType
    $CycleType = $Board.cycleType
    if ($RankId -ne $RankId.Trim() -or $RankId -notmatch '^[A-Za-z0-9_-]{1,64}$') {
      throw "rankBoards[$I].rankId must match [A-Za-z0-9_-] and be 1-64 bytes without surrounding whitespace."
    }
    if (!$Seen.Add($RankId)) { throw "rankBoards rankId duplicated: $RankId" }
    if ([string]::IsNullOrWhiteSpace($DisplayName) -or $DisplayName -ne $DisplayName.Trim() -or [Text.Encoding]::UTF8.GetByteCount($DisplayName) -gt 64) {
      throw "rankBoards[$I].displayName must be non-empty UTF-8 up to 64 bytes without surrounding whitespace."
    }
    if ($SortType -notin @("asc", "desc")) { throw "rankBoards[$I].sortType must be asc or desc." }
    if ($CycleType -notin @("day", "week", "month", "forever")) { throw "rankBoards[$I].cycleType must be day, week, month, or forever." }
    $MinScore = ConvertTo-ManifestSafeInteger $Board.minScore "rankBoards[$I].minScore"
    $MaxScore = ConvertTo-ManifestSafeInteger $Board.maxScore "rankBoards[$I].maxScore"
    if ($MinScore -gt $MaxScore) { throw "rankBoards[$I].minScore must not exceed maxScore." }
  }
}

function Assert-ManifestBrowserMinimums {
  param([string]$File)
  if (!(Test-Path $File)) { throw "manifest not found: $File" }
  try {
    $Data = Get-Content $File -Raw | ConvertFrom-Json
  } catch {
    throw "manifest json invalid: $File"
  }
  $Names = @($Data.PSObject.Properties.Name)
  foreach ($Key in @("minChromeMajor", "minIOSMajor")) {
    $Bad = $true
    if ($Names -contains $Key) {
      try {
        $Value = ConvertTo-ManifestSafeInteger $Data.$Key $Key
        if ($Value -gt 0) { $Bad = $false }
      } catch { }
    }
    if ($Bad) {
      throw "Fill minChromeMajor and minIOSMajor in $File as positive integers: the LOWEST Chrome major and Safari iOS major this build actually runs on. Lower values let more devices play, so do not inflate them; but ground the floor in evidence: WASM engine exports (e.g. Godot 4 Web) inherit it from the compiler toolchain (post-MVP WASM features land around Chrome 75-85 / iOS 15; check engine or Emscripten minimum-browser docs, fall back to the platform baseline 80/15 when unverifiable), while plain canvas/DOM H5 with down-leveled JS can go much lower. Then rerun this command."
    }
  }
}

function Get-PersistentIdentifierAuditFiles {
  $Extensions = @(".gd", ".js", ".mjs", ".cjs", ".ts", ".mts", ".cts", ".tsx", ".jsx", ".vue", ".svelte", ".html", ".cs", ".json", ".sh", ".ps1", ".bat", ".cmd", ".py", ".pyw", ".yaml", ".yml", ".toml", ".cfg", ".ini", ".conf", ".xml", ".gradle", ".groovy", ".java", ".kt", ".kts", ".rb", ".php", ".lua", ".tres", ".tscn", ".unity", ".prefab", ".asset")
  $Names = @("Makefile", "makefile", "GNUmakefile", "CMakeLists.txt", "project.godot", "builda.publish.json")
  $Skip = @(".git", ".godot", ".builda-agent", ".builda-godot-agent", "node_modules", "Library", "Temp", "Obj", "Logs", "Packages", "build", "dist", "target", "Build", "Builds", "server-pack", "server_pack")
  $RootPath = (Get-Location).Path
  $Files = [System.Collections.Generic.List[System.IO.FileInfo]]::new()
  function Visit-PersistentIdentifierDirectory([string]$Directory) {
    foreach ($Item in @(Get-ChildItem -LiteralPath $Directory -Force -ErrorAction SilentlyContinue)) {
      if ($Item.PSIsContainer) {
        if (($Item.Attributes -band [IO.FileAttributes]::ReparsePoint) -ne 0) { continue }
        $RelDir = [System.IO.Path]::GetRelativePath($RootPath, $Item.FullName).Replace('\', '/')
        if ($Skip -contains $Item.Name -or $RelDir -eq "addons/builda" -or $RelDir -eq "Assets/Builda") { continue }
        Visit-PersistentIdentifierDirectory $Item.FullName
        continue
      }
      if (($Extensions -contains $Item.Extension.ToLowerInvariant()) -or ($Names -ccontains $Item.Name) -or $Item.Name -eq ".env" -or $Item.Name.StartsWith(".env.")) {
        [void]$Files.Add($Item)
      }
    }
  }
  Visit-PersistentIdentifierDirectory $RootPath
  @($Files | Sort-Object { [System.IO.Path]::GetRelativePath($RootPath, $_.FullName).Replace('\', '/') })
}

function Get-PersistentIdentifierAuditFingerprint {
  $Sha = [System.Security.Cryptography.SHA256]::Create()
  try {
    foreach ($File in (Get-PersistentIdentifierAuditFiles)) {
      $Rel = [System.IO.Path]::GetRelativePath((Get-Location).Path, $File.FullName).Replace('\', '/')
      $PathBytes = [Text.Encoding]::UTF8.GetBytes($Rel + [char]0)
      [void]$Sha.TransformBlock($PathBytes, 0, $PathBytes.Length, $PathBytes, 0)
      $Content = [System.IO.File]::ReadAllBytes($File.FullName)
      [void]$Sha.TransformBlock($Content, 0, $Content.Length, $Content, 0)
      $Zero = [byte[]]@(0)
      [void]$Sha.TransformBlock($Zero, 0, 1, $Zero, 0)
    }
    [void]$Sha.TransformFinalBlock([byte[]]@(), 0, 0)
    return ([BitConverter]::ToString($Sha.Hash) -replace '-', '').ToLowerInvariant()
  } finally {
    $Sha.Dispose()
  }
}

function Show-PersistentIdentifierAudit {
  $Pattern = 'privateKV|private_kv_|Kv(Get|Set|Remove)|showPayPanel|PayShowPanel|pay_show_panel|rank\s*\.\s*(submitScore|getRankList)|Rank(Submit|Get)|rank_(submit|get)|rankBoards|payPoints|localStorage|indexedDB|document\s*\.\s*cookie|user://'
  Write-Host "sdk-key-audit=inventory"
  $Files = Get-PersistentIdentifierAuditFiles
  $Matches = [System.Collections.Generic.List[string]]::new()
  $Files | ForEach-Object {
    $Rel = [System.IO.Path]::GetRelativePath((Get-Location).Path, $_.FullName).Replace('\', '/')
    if ($_.Name -eq ".env" -or $_.Name.StartsWith(".env.")) {
      if (Select-String -Path $_.FullName -Pattern $Pattern -Quiet -ErrorAction SilentlyContinue) {
        $Matches.Add("${Rel}: persistence-related environment config found; inspect locally (value hidden)")
      }
      return
    }
    Select-String -Path $_.FullName -Pattern $Pattern -AllMatches -ErrorAction SilentlyContinue | ForEach-Object {
      $Text = $_.Line
      if ($Text.Length -gt 300) { $Text = $Text.Substring(0, 300) + "..." }
      $Matches.Add("${Rel}:$($_.LineNumber):$Text")
    }
  }
  $Matches | Select-Object -First 200 | ForEach-Object { Write-Host $_ }
  if ($Matches.Count -gt 200) { Write-Host "sdk-key-audit=truncated shown=200 total=$($Matches.Count)" }
  $Snapshot = Join-Path $Root "key-audit-snapshot"
  Get-PersistentIdentifierAuditFingerprint | Set-Content -Encoding ASCII $Snapshot
  Write-Host "sdk-key-audit=review-required"
  Write-Host "Review every listed call plus aliases, wrappers, multiline arguments and dynamic containers; see $AgentBase/agent/releases/$Version/md_persistent_ids.md"
  Write-Host "sdk-key-audit=snapshot-written files=$($Files.Count) matches=$($Matches.Count)"
}

function Assert-PersistentIdentifierAuditFresh {
  $Snapshot = Join-Path $Root "key-audit-snapshot"
  if (!(Test-Path $Snapshot)) {
    throw "draft-preflight=failed: missing $Snapshot; run '.\.builda-agent\builda.ps1 sdk key-audit', review the inventory, then retry."
  }
  $Expected = (Get-Content $Snapshot -Raw).Trim()
  $Current = Get-PersistentIdentifierAuditFingerprint
  if ($Current -ne $Expected) {
    throw "draft-preflight=failed: persistent-id audit snapshot is stale because source, manifest, or build scripts changed; rerun '.\.builda-agent\builda.ps1 sdk key-audit'."
  }
}

function Set-ManifestSdkVersion {
  param([string]$File)
  # sdkVersion 跟随项目 SDK 安装收据，草稿上报后进 release manifest sdk 契约；不要手工维护
  if (!(Test-Path $SdkVersionFile)) { return }
  $V = (Get-Content $SdkVersionFile -First 1).Trim()
  if (!$V) { return }
  try {
    $Data = Get-Content $File -Raw | ConvertFrom-Json
    $Data | Add-Member -NotePropertyName sdkVersion -NotePropertyValue $V -Force
    $Data | ConvertTo-Json -Depth 16 | Set-Content -Encoding UTF8 $File
  } catch {
    Write-Host "sdk-version-inject=skipped ($($_.Exception.Message))"
  }
}

function Invoke-DraftPreflight {
  param([string]$File)
  Assert-ManifestPayPoints $File
  Assert-ManifestRankBoards $File
  Assert-ManifestBrowserMinimums $File
  Assert-PersistentIdentifierAuditFresh
}

function Test-SdkCompat {
  $Allowed = @(".gd", ".js", ".ts", ".tsx", ".jsx", ".html", ".cfg", ".tscn")
  # Unity 工程目录整体跳过：旧 Builda SDK API 只存在于 GDScript/JS 时代，Unity 项目无迁移
  # 负担；Library/Temp 文件量大，扫描徒增耗时与误报面。
  $Skip = @(".git", ".godot", ".builda-agent", ".builda-godot-agent", "addons", "node_modules", "__pycache__", "build", "dist", "target", "server-pack", "server_pack", "Library", "Temp", "Obj", "Logs", "Assets", "ProjectSettings", "Packages")
  $Errors = New-Object System.Collections.Generic.List[string]
  Get-ChildItem -Path "." -Recurse -File -ErrorAction SilentlyContinue | ForEach-Object {
    if ($Allowed -notcontains $_.Extension.ToLowerInvariant()) { return }
    $Rel = [System.IO.Path]::GetRelativePath((Get-Location).Path, $_.FullName)
    foreach ($Part in ($Rel -split '[\\/]')) {
      if ($Skip -contains $Part) { return }
    }
    $Lines = Get-Content $_.FullName -ErrorAction SilentlyContinue
    for ($Index = 0; $Index -lt $Lines.Count; $Index++) {
      $Line = [string]$Lines[$Index]
      $LineNo = $Index + 1
      if ($Line -match '\.cloud_get\s*\(') { $Errors.Add("${Rel}:${LineNo}: old Builda SDK API found; replace old cloud_get(...) with private_kv_get(...)") }
      if ($Line -match '\.cloud_set\s*\(') { $Errors.Add("${Rel}:${LineNo}: old Builda SDK API found; replace old cloud_set(...) with private_kv_set(...)") }
      if ($Line -match '\.cloud_delete\s*\(') { $Errors.Add("${Rel}:${LineNo}: old Builda SDK API found; replace old cloud_delete(...) with private_kv_remove(...)") }
      if ($Line -match '\.kv_get\s*\(') { $Errors.Add("${Rel}:${LineNo}: old Builda SDK API found; kv_get(...) no longer exists; use private_kv_get(...) (value is now bytes: var_to_bytes/bytes_to_var)") }
      if ($Line -match '\.kv_set\s*\(') { $Errors.Add("${Rel}:${LineNo}: old Builda SDK API found; kv_set(...) no longer exists; use private_kv_set(key, var_to_bytes(...))") }
      if ($Line -match '\.kv_remove\s*\(') { $Errors.Add("${Rel}:${LineNo}: old Builda SDK API found; kv_remove(...) no longer exists; use private_kv_remove(...)") }
      if ($Line -match '\bBuilda\s*\.\s*kv\s*\.') { $Errors.Add("${Rel}:${LineNo}: old Builda SDK API found; Builda.kv.* no longer exists; use Builda.privateKV.* (value is now Uint8Array)") }
      if ($Line -match '\.ready\s*\(') { $Errors.Add("${Rel}:${LineNo}: old Builda SDK API found; replace old ready(...) with runtime_ready(...)") }
      if ($Line -match 'Engine\.get_singleton\s*\(\s*["'']Builda["'']\s*\)') { $Errors.Add("${Rel}:${LineNo}: old Builda SDK API found; replace old Engine.get_singleton(""Builda"") usage with the generated BuildaClient autoload") }
      if ($Line -match 'get_node(?:_or_null)?\s*\(\s*["'']/root/Builda["'']\s*\)') { $Errors.Add("${Rel}:${LineNo}: old Builda SDK API found; replace old /root/Builda node usage with /root/BuildaClient") }
      if ($Line -match '\.init\s*\(') { $Errors.Add("${Rel}:${LineNo}: old Builda SDK API found; old init(...) is no longer part of the public SDK; use BuildaClient.builda.runtime_ready() when the game is ready") }
      if ($Line -match '\.event\s*\(') { $Errors.Add("${Rel}:${LineNo}: old Builda SDK API found; old event(...) analytics API is not part of the MVP SDK; remove it or keep analytics outside Builda") }
      if ($Line -match '\.track\s*\(') { $Errors.Add("${Rel}:${LineNo}: old Builda SDK API found; old track(...) analytics API is not part of the MVP SDK; remove it or keep analytics outside Builda") }
      if ($Line -match '\bbackend_create\s*\(') { $Errors.Add("${Rel}:${LineNo}: Builda 联机能力开发中、尚未开放；backend_create(...) 已从 SDK 移除，删掉该调用") }
      if ($Line -match '\bBuilda\s*\.\s*backend\s*\.\s*create\s*\(') { $Errors.Add("${Rel}:${LineNo}: Builda 联机能力开发中、尚未开放；Builda.backend.create() 已从 SDK 移除，删掉该调用") }
    }
  }
  if ($Errors.Count -gt 0) {
    Write-Host "sdk-compat=failed"
    foreach ($ErrorItem in $Errors) { Write-Error $ErrorItem }
    Write-Error "Current SDK calls are async request/signal based. If the old code expected a direct return value, store the request id and handle the result in BuildaClient.builda.sdk_result; see addons/builda/examples/example.gd and scripts/BuildaClient.gd."
    throw "SDK compatibility check failed"
  }
  Write-Host "sdk-compat=ok"
}

function Assert-CmdResponseOk {
  param([object]$Resp, [string]$Label)
  if ($null -ne $Resp.code -and [int]$Resp.code -ne 0) {
    if ([int]$Resp.code -eq 200455) {
      throw "发布失败：暂未开放外部游戏发布权限，请前往 $EarlyAccessUrl 申请内测资格"
    }
    $Message = if ($Resp.error) { $Resp.error } elseif ($Resp.msg) { $Resp.msg } else { "" }
    throw "$Label failed: code=$($Resp.code) $Message"
  }
}

function Test-BundleZip {
  param([string]$Zip, [bool]$WebViewCompat = $false, [string]$Engine = "")
  if (!$Zip -or !(Test-Path $Zip)) { throw "zip file required" }
  if (!$Engine) { $Engine = Get-ManifestEngine }
  if ($Engine -notin @("godot", "h5", "unity")) { throw "bundle-check engine must be godot, h5 or unity: $Engine" }
  # 校验口径与后端 buildzip 一致（单一真相源：h5_bundle_spec.md 附表 B）
  $GodotExt = @(".html", ".js", ".wasm", ".pck", ".png", ".jpg", ".jpeg", ".webp", ".svg", ".ico", ".json", ".css", ".txt", ".md")
  $Html5Ext = $GodotExt + @(".mjs", ".woff", ".woff2", ".ttf", ".otf", ".mp3", ".ogg", ".wav", ".m4a", ".mp4", ".webm", ".avif", ".gif", ".bin", ".dat", ".glsl", ".frag", ".vert", ".atlas", ".fnt", ".xml", ".gltf", ".glb", ".ktx2", ".basis")
  $UnityExt = $Html5Ext + @(".unityweb", ".data")
  $AllowedExt = switch ($Engine) { "godot" { $GodotExt } "unity" { $UnityExt } default { $Html5Ext } }
  Add-Type -AssemblyName System.IO.Compression.FileSystem
  $Archive = [System.IO.Compression.ZipFile]::OpenRead((Resolve-Path $Zip))
  try {
    $HasIndex = $false
    $HasJs = $false
    $HasWasm = $false
    $HasPck = $false
    $HasBuildaSdk = $false
    $IndexHtml = ""
    $IndexJs = ""
    $Forbidden = @()
    $BadExt = @()
    # unity 的 Build/ 四件套（文件名带构建名变量，只做前缀+后缀匹配，附表 B.7）
    $UnityFound = @{ "loader.js" = $false; "framework.js" = $false; "wasm" = $false; "data" = $false }
    foreach ($Entry in $Archive.Entries) {
      $Name = $Entry.FullName.TrimStart("./").TrimEnd("/")
      if (!$Name) { continue }
      if ($Name -eq "index.html") {
        $HasIndex = $true
        $Reader = New-Object System.IO.StreamReader($Entry.Open())
        try { $IndexHtml = $Reader.ReadToEnd() } finally { $Reader.Dispose() }
      }
      if ($Name -eq "index.js") {
        $HasJs = $true
        if ($Engine -eq "godot") {
          $Reader = New-Object System.IO.StreamReader($Entry.Open())
          try { $IndexJs = $Reader.ReadToEnd() } finally { $Reader.Dispose() }
        }
      }
      if ($Name -eq "index.wasm") { $HasWasm = $true }
      if ($Name -eq "index.pck") { $HasPck = $true }
      if ($Name -eq "addons/builda/web/builda-sdk.js" -or $Name -eq "builda-sdk.js") { $HasBuildaSdk = $true }
      if ($Name -match '(^|/)(project\.godot|export_presets\.cfg|\.env|builda-dev-shell\.html)$' -or
          $Name -match '\.import$' -or $Name -match '\.d\.ts$' -or
          $Name -match '(^|/)(\.godot|\.git|\.svn|\.hg|__MACOSX)(/|$)') {
        $Forbidden += $Name
      }
      if ($Engine -ne "godot") {
        if ($Name -match '\.map$') { $Forbidden += "$Name (sourcemap leaks sources; disable sourcemaps and rebuild)" }
        if ($Name -match '(^|/)node_modules(/|$)') { $Forbidden += "$Name (node_modules must not be zipped)" }
      }
      if ($Engine -eq "unity") {
        if ($Name -match '\.(meta|csproj|sln|unity)$') { $Forbidden += "$Name (Unity project/editor files must not be zipped)" }
        if ($Name -match '(^|/)(Library|Temp|Obj|ProjectSettings)(/|$)') { $Forbidden += "$Name (Unity project directories must not be zipped)" }
        if ($Name -like "Build/*") {
          if ($Name -like "*.loader.js") { $UnityFound["loader.js"] = $true }
          elseif ($Name -like "*.framework.js" -or $Name -like "*.framework.js.unityweb") { $UnityFound["framework.js"] = $true }
          elseif ($Name -like "*.wasm" -or $Name -like "*.wasm.unityweb") { $UnityFound["wasm"] = $true }
          elseif ($Name -like "*.data" -or $Name -like "*.data.unityweb") { $UnityFound["data"] = $true }
        }
      }
      # unity 的 StreamingAssets/ 跳过扩展名白名单（AssetBundle 惯例常无扩展名，附表 B.3）；
      # 禁用清单照常执行
      $StreamingExempt = ($Engine -eq "unity") -and ($Name -like "StreamingAssets/*")
      $Ext = [System.IO.Path]::GetExtension($Name).ToLowerInvariant()
      if (!$StreamingExempt -and $AllowedExt -notcontains $Ext) { $BadExt += $Name }
    }
    if (!$HasIndex) { throw "Missing root index.html" }
    if ($Engine -eq "godot") {
      if (!$HasJs) { throw "Missing root index.js" }
      if (!$HasWasm) { throw "Missing root index.wasm" }
      if (!$HasPck) { throw "Missing root index.pck" }
    }
    if ($Engine -eq "unity") {
      $MissingUnity = @($UnityFound.Keys | Where-Object { -not $UnityFound[$_] } | Sort-Object)
      if ($MissingUnity.Count -gt 0) {
        throw "Missing Build/ Unity WebGL artifacts (*.$($MissingUnity -join ', *.'); .unityweb suffix allowed). Re-export with the default Build/ output layout."
      }
    }
    if ($Forbidden.Count -gt 0) { throw "Forbidden project/editor files: $($Forbidden -join ', ')" }
    if ($BadExt.Count -gt 0) { throw "File types not allowed for engine=${Engine}: $($BadExt -join ', ')" }
    if ($HasBuildaSdk) { Write-Host "bundle-sdk=embedded (legacy form; new exports ship without SDK, host injects by manifest)" }
    if ($IndexHtml -notlike "*builda-sdk.js*") {
      if ($Engine -eq "godot") { throw "index.html does not load builda-sdk.js. Ensure Godot Web export head include injects the SDK script." }
      if ($Engine -eq "unity") { throw 'index.html does not load builda-sdk.js. Select the Builda WebGL Template (Player Settings -> Resolution and Presentation; installed by builda sdk install), or add <script src="builda-sdk.js"></script> to your custom template head, then rebuild.' }
      throw 'index.html does not load builda-sdk.js. Add <script src="builda-sdk.js"></script> to the HTML head and rebuild.'
    }
    if ($Engine -eq "godot" -and $WebViewCompat) {
      # Godot 专属：threads/COI 导出模板变量正则（h5 跳过，误报不可控）
      if ($IndexHtml -match '\bGODOT_THREADS_ENABLED\s*=\s*true\b' -or $IndexJs -match '\bGODOT_THREADS_ENABLED\s*=\s*true\b') {
        throw "Godot Web export has threads enabled. For WebView compatibility mode, set variant/thread_support=false and re-export."
      }
      if ($IndexHtml -match '\bensureCrossOriginIsolationHeaders\s*:\s*true\b' -or $IndexJs -match '\bensureCrossOriginIsolationHeaders\s*:\s*true\b') {
        throw "Godot Web export requests cross-origin isolation headers. For WebView compatibility mode, set progressive_web_app/ensure_cross_origin_isolation_headers=false and re-export."
      }
    }
    Write-Host "bundle=ok"
    Write-Host "bundle-engine=$Engine"
    if ($Engine -eq "godot") {
      if ($WebViewCompat) { Write-Host "threads-check=webview-compatible" } else { Write-Host "threads-check=default-allowed" }
    }
  } finally {
    $Archive.Dispose()
  }
}

function Invoke-DevServer {
  param([string[]]$Args)
  $Web = ""
  $Port = 18088
  $GameId = "local-game"
  $SafeArea = ""
  for ($i = 0; $i -lt $Args.Count; $i++) {
    $Arg = $Args[$i]
    switch -Regex ($Arg) {
      "^--web$" {
        $i++
        if ($i -ge $Args.Count) { throw "Usage: builda dev --web <dir|zip> [--port 18088] [--game-id local-game]" }
        $Web = $Args[$i]
      }
      "^--web=(.*)$" { $Web = $Matches[1] }
      "^--port$" {
        $i++
        if ($i -ge $Args.Count) { throw "Usage: builda dev --web <dir|zip> [--port 18088] [--game-id local-game]" }
        $Port = [int]$Args[$i]
      }
      "^--port=(.*)$" { $Port = [int]$Matches[1] }
      "^--game-id$" {
        $i++
        if ($i -ge $Args.Count) { throw "Usage: builda dev --web <dir|zip> [--port 18088] [--game-id local-game]" }
        $GameId = $Args[$i]
      }
      "^--game-id=(.*)$" { $GameId = $Matches[1] }
      "^--safearea$" {
        $i++
        if ($i -ge $Args.Count) { throw "Usage: builda dev --web <dir|zip> [--port 18088] [--game-id local-game] [--safearea 44,0,34,0]" }
        $SafeArea = $Args[$i]
      }
      "^--safearea=(.*)$" { $SafeArea = $Matches[1] }
      "^-h$|^--help$|^help$|^$" {
        Write-Host "Usage: builda dev --web <dir|zip> [--port 18088] [--game-id local-game] [--safearea 44,0,34,0]"
        return
      }
      default {
        throw "Unknown dev arg: $Arg"
      }
    }
  }
  if (!$Web) { throw "Usage: builda dev --web <dir|zip> [--port 18088]" }
  if (!(Test-Path $Web)) { throw "dev web path not found: $Web" }
  if ($Port -lt 0 -or $Port -gt 65535) { throw "dev port must be between 0 and 65535" }
  if ($SafeArea -and $SafeArea -notmatch "^\d+(\.\d+)?,\d+(\.\d+)?,\d+(\.\d+)?,\d+(\.\d+)?$") {
    throw "dev --safearea must be top,right,bottom,left in CSS px, e.g. 44,0,34,0"
  }

  $Resolved = Resolve-Path $Web
  $TempDir = $null
  if ((Get-Item $Resolved).PSIsContainer) {
    $WebRoot = $Resolved.Path
  } else {
    if ([System.IO.Path]::GetExtension($Resolved.Path).ToLowerInvariant() -ne ".zip") { throw "dev --web file must be a .zip H5 bundle" }
    $TempDir = Join-Path ([System.IO.Path]::GetTempPath()) ("builda-dev-web-" + [System.Guid]::NewGuid().ToString("N"))
    New-Item -ItemType Directory -Force -Path $TempDir | Out-Null
    Add-Type -AssemblyName System.IO.Compression.FileSystem
    $Zip = [System.IO.Compression.ZipFile]::OpenRead($Resolved.Path)
    try {
      foreach ($Entry in $Zip.Entries) {
        $Name = $Entry.FullName
        if ($Name.StartsWith("/") -or $Name.Contains("\") -or $Name.Contains(":") -or ($Name.Split("/") -contains "..")) {
          throw "Unsafe zip path: $Name"
        }
        if (!$Entry.Name) { continue }
        $Target = Join-Path $TempDir ($Name -replace "/", [System.IO.Path]::DirectorySeparatorChar)
        $Parent = Split-Path -Parent $Target
        if ($Parent) { New-Item -ItemType Directory -Force -Path $Parent | Out-Null }
        [System.IO.Compression.ZipFileExtensions]::ExtractToFile($Entry, $Target, $true)
      }
    } finally {
      $Zip.Dispose()
    }
    $WebRoot = $TempDir
  }
  if (!(Test-Path (Join-Path $WebRoot "index.html"))) { throw "dev web root missing index.html: $WebRoot" }
  # mock SDK/外壳兜底查找顺序：web 导出目录 → Godot addon → HTML5 工具链落点（$ProjectStateDir/sdk/web/）
  $ProjectSdkJs = @(
    (Join-Path (Get-Location) "addons\builda\web\builda-sdk.js"),
    (Join-Path (Get-Location) (Join-Path $ProjectStateDir "sdk\web\builda-sdk.js"))
  ) | Where-Object { Test-Path $_ } | Select-Object -First 1
  $SdkPresent = (Test-Path (Join-Path $WebRoot "addons\builda\web\builda-sdk.js")) -or (Test-Path (Join-Path $WebRoot "builda-sdk.js")) -or [bool]$ProjectSdkJs
  # 测试外壳随 mock SDK 装进项目，web 导出里不会有；从项目目录兜底 serve
  $ProjectShell = @(
    (Join-Path (Get-Location) "addons\builda\web\builda-dev-shell.html"),
    (Join-Path (Get-Location) (Join-Path $ProjectStateDir "sdk\web\builda-dev-shell.html"))
  ) | Where-Object { Test-Path $_ } | Select-Object -First 1
  $ShellPresent = (Test-Path (Join-Path $WebRoot "builda-dev-shell.html")) -or [bool]$ProjectShell

  $Listener = New-Object System.Net.HttpListener
  $ActualPort = $Port
  $Bound = $false
  $Candidates = if ($Port -eq 0) { @(0) } else { $Port..([Math]::Min($Port + 20, 65535)) }
  foreach ($Candidate in $Candidates) {
    $TryPort = if ($Candidate -eq 0) {
      $Tcp = [System.Net.Sockets.TcpListener]::new([System.Net.IPAddress]::Parse("127.0.0.1"), 0)
      $Tcp.Start()
      $FreePort = $Tcp.LocalEndpoint.Port
      $Tcp.Stop()
      $FreePort
    } else {
      $Candidate
    }
    $Listener.Prefixes.Clear()
    $Listener.Prefixes.Add("http://127.0.0.1:$TryPort/")
    try {
      $Listener.Start()
      $ActualPort = $TryPort
      $Bound = $true
      break
    } catch {
      if ($Candidate -eq 0) { throw }
    }
  }
  if (!$Bound) { throw "could not bind localhost port $Port" }

  $EntryQuery = "gameId=$([uri]::EscapeDataString($GameId))"
  if ($SafeArea) { $EntryQuery += "&builda_mock_safearea=$([uri]::EscapeDataString($SafeArea))" }
  $EntryPath = "/index.html?$EntryQuery"
  $ShellPath = "/builda-dev-shell.html?$EntryQuery"
  function Send-DevBytes($Context, [byte[]]$Bytes, [string]$ContentType, [int]$Status = 200) {
    $Context.Response.StatusCode = $Status
    $Context.Response.ContentType = $ContentType
    $Context.Response.Headers["Cross-Origin-Opener-Policy"] = "same-origin"
    $Context.Response.Headers["Cross-Origin-Embedder-Policy"] = "require-corp"
    $Context.Response.Headers["Cross-Origin-Resource-Policy"] = "cross-origin"
    $Context.Response.ContentLength64 = $Bytes.Length
    $Context.Response.OutputStream.Write($Bytes, 0, $Bytes.Length)
    $Context.Response.Close()
  }
  function Get-DevContentType([string]$Path) {
    switch ([System.IO.Path]::GetExtension($Path).ToLowerInvariant()) {
      ".html" { "text/html; charset=utf-8" }
      ".js" { "text/javascript" }
      ".wasm" { "application/wasm" }
      ".pck" { "application/octet-stream" }
      ".json" { "application/json" }
      ".png" { "image/png" }
      ".jpg" { "image/jpeg" }
      ".jpeg" { "image/jpeg" }
      ".webp" { "image/webp" }
      default { "application/octet-stream" }
    }
  }

  $BaseUrl = "http://127.0.0.1:$ActualPort"
  Write-Host "dev=ok"
  if ($ShellPresent) { Write-Host "dev-url=$BaseUrl$ShellPath" } else { Write-Host "dev-url=$BaseUrl$EntryPath" }
  Write-Host "game-url=$BaseUrl$EntryPath"
  Write-Host "sdk-mode=local-mock"
  if ($ShellPresent) { Write-Host "dev-shell=present" } else { Write-Host "dev-shell=missing" }
  if ($SdkPresent) { Write-Host "dev-sdk=present" } else { Write-Host "dev-sdk=missing" }
  Write-Host "dev-web=$WebRoot"
  Write-Host "dev-mock-player=local-player (append &builda_mock_player=<id>&builda_mock_name=<name> to the URL to switch identity)"
  if (!$ShellPresent) {
    Write-Host "dev-shell-warning=builda-dev-shell.html not found; run sdk install to get the test shell (orientation/notch/capsule/pay/ad mock)."
  }
  if (!$SdkPresent) {
    Write-Host "dev-warning=builda-sdk.js not found in web root or project; run sdk install (dev server serves the project copy automatically)."
  }

  try {
    while ($Listener.IsListening) {
      $Context = $Listener.GetContext()
      $Path = [System.Uri]::UnescapeDataString($Context.Request.Url.AbsolutePath)
      if ($Path -eq "/") {
        $Context.Response.StatusCode = 302
        if ($ShellPresent) { $Context.Response.Headers["Location"] = $ShellPath } else { $Context.Response.Headers["Location"] = $EntryPath }
        $Context.Response.Close()
        continue
      }
      if ($Path -eq "/builda-dev-shell.html" -and !(Test-Path (Join-Path $WebRoot "builda-dev-shell.html")) -and $ProjectShell) {
        Send-DevBytes $Context ([System.IO.File]::ReadAllBytes($ProjectShell)) "text/html; charset=utf-8"
        continue
      }
      if (($Path -eq "/builda-sdk.js" -or $Path -eq "/addons/builda/web/builda-sdk.js") -and $ProjectSdkJs) {
        $InWeb = Join-Path $WebRoot ($Path.TrimStart("/") -replace "/", [System.IO.Path]::DirectorySeparatorChar)
        if (!(Test-Path $InWeb)) {
          Send-DevBytes $Context ([System.IO.File]::ReadAllBytes($ProjectSdkJs)) "text/javascript"
          continue
        }
      }
      $Rel = $Path.TrimStart("/")
      if (!$Rel) { $Rel = "index.html" }
      $Target = [System.IO.Path]::GetFullPath((Join-Path $WebRoot ($Rel -replace "/", [System.IO.Path]::DirectorySeparatorChar)))
      $RootFull = [System.IO.Path]::GetFullPath($WebRoot)
      if (!$Target.StartsWith($RootFull, [System.StringComparison]::OrdinalIgnoreCase) -or !(Test-Path $Target) -or (Get-Item $Target).PSIsContainer) {
        Send-DevBytes $Context ([System.Text.Encoding]::UTF8.GetBytes("not found")) "text/plain; charset=utf-8" 404
        continue
      }
      Send-DevBytes $Context ([System.IO.File]::ReadAllBytes($Target)) (Get-DevContentType $Target)
    }
  } finally {
    if ($Listener.IsListening) { $Listener.Stop() }
    $Listener.Close()
    if ($TempDir -and (Test-Path $TempDir)) { Remove-Item -Recurse -Force $TempDir }
  }
}

function Test-AssetsZip {
  param([string]$Zip)
  if (!$Zip -or !(Test-Path $Zip)) { throw "assets.zip file required" }
  Add-Type -AssemblyName System.IO.Compression.FileSystem
  $Archive = [System.IO.Compression.ZipFile]::OpenRead((Resolve-Path $Zip))
  try {
    $Count = 0
    [int64]$Total = 0
    foreach ($Entry in $Archive.Entries) {
      $Name = $Entry.FullName.TrimStart("./").TrimEnd("/")
      if (!$Name) { continue }
      if ($Name.EndsWith("/")) { continue }
      if ($Name.StartsWith("/") -or $Name.Contains("\") -or $Name.Contains(":") -or $Name -match '(^|/)\.\.(/|$)') {
        throw "Unsafe asset path: $Name"
      }
      if ($Name -notmatch '^audio/') { throw "Asset must be under audio/: $Name" }
      if ($Name -notmatch '\.(mp3|ogg|wav)$') { throw "Only .mp3/.ogg/.wav assets are allowed: $Name" }
      if ($Name -match '(^|/)(\.godot|\.git|\.svn|\.hg|__MACOSX)(/|$)' -or $Name -match '(^|/)\.[^/]+$') {
        throw "Forbidden asset path: $Name"
      }
      if ($Entry.Length -gt 20MB) { throw "Asset file exceeds 20MB: $Name" }
      $Count += 1
      $Total += $Entry.Length
      if ($Count -gt 200) { throw "Too many asset files: max 200" }
      if ($Total -gt 100MB) { throw "Assets zip exceeds 100MB uncompressed" }
    }
    if ($Count -eq 0) { throw "assets.zip is empty" }
    Write-Host "assets=ok"
    Write-Host "assets-files=$Count"
    Write-Host "assets-bytes=$Total"
  } finally {
    $Archive.Dispose()
  }
}

function Upload-AssetsZip {
  param([string]$Zip)
  Need-Token
  if (!$Zip -or !(Test-Path $Zip)) { throw "assets.zip file required" }
  Test-AssetsZip $Zip | Out-Null
  if (!(Get-Command curl.exe -ErrorAction SilentlyContinue)) { throw "curl.exe is required for assets upload on Windows." }
  $ZipPath = Resolve-Path $Zip
  $GameId = Get-ProjectGameId
  if ($GameId) {
    $Raw = & curl.exe -fsS -X POST "$Base/api/v1/uploads/assets" -H "Authorization: Bearer $(Get-Token)" -F "file=@$ZipPath" -F "gameId=$GameId"
  } else {
    $Raw = & curl.exe -fsS -X POST "$Base/api/v1/uploads/assets" -H "Authorization: Bearer $(Get-Token)" -F "file=@$ZipPath"
  }
  if ($LASTEXITCODE -ne 0) { throw "assets upload failed" }
  $Resp = $Raw | ConvertFrom-Json
  $Resp | ConvertTo-Json -Depth 8 | Set-Content -Encoding UTF8 (Join-Path $Root "last-assets.json")
  $Resp | ConvertTo-Json -Depth 8
  if ($Resp.assetsVersion) {
    if (!(Test-Path $Manifest)) { Write-DefaultManifest }
    $Data = Get-Content $Manifest -Raw | ConvertFrom-Json
    $Data | Add-Member -NotePropertyName assetsVersion -NotePropertyValue $Resp.assetsVersion -Force
    $Data | Add-Member -NotePropertyName assetsBaseUrl -NotePropertyValue $Resp.assetsBaseUrl -Force
    $Data | Add-Member -NotePropertyName assetsManifestUrl -NotePropertyValue $Resp.assetsManifestUrl -Force
    $Data | ConvertTo-Json -Depth 8 | Set-Content -Encoding UTF8 $Manifest
    Write-Host "Updated $Manifest with audio assets fields."
  }
}

function Install-WebSdk {
  # HTML5 项目：mock SDK/d.ts/dev-shell/mic worker 装到 $ProjectStateDir/sdk/web/
  $Meta = Invoke-RestMethod -Method Get -Uri "$AgentBase/agent/sdk/web/latest.json"
  if (!$Meta.version -or !$Meta.url) { throw "SDK metadata invalid." }
  $Tmp = Join-Path ([System.IO.Path]::GetTempPath()) ("builda-sdk-web-" + [System.Guid]::NewGuid().ToString() + ".zip")
  Invoke-WebRequest -UseBasicParsing $Meta.url -OutFile $Tmp
  $Dst = Join-Path $ProjectStateDir "sdk/web"
  Remove-Item -Recurse -Force $Dst -ErrorAction SilentlyContinue
  New-Item -ItemType Directory -Force -Path $Dst | Out-Null
  Expand-Archive -Force -Path $Tmp -DestinationPath $Dst
  Remove-Item -Force $Tmp
  New-Item -ItemType Directory -Force -Path $ProjectStateDir | Out-Null
  $Meta.version | Set-Content -Encoding ASCII $SdkVersionFile
  Write-Host "sdk-version=$($Meta.version)"
  Write-Host "sdk-installed=$Dst"
  Write-Host "sdk-engine=h5"
  Write-Host 'sdk-next-1=在游戏 HTML 的 <head> 里引用根路径 SDK：<script src="builda-sdk.js"></script>（构建产物 index.html 必须带上该引用；zip 不内嵌 SDK，正式运行时由宿主按 manifest sdk 契约注入）'
  Write-Host "sdk-next-2=本地调试：builda dev --web <构建目录或 zip>（dev server 自动从 $Dst 兜底 serve mock SDK 与测试外壳）"
  Write-Host "sdk-next-3=TypeScript 类型提示：把 $Dst/builda-sdk.d.ts 加进 tsconfig include（可选）"
  try {
    Test-Sdk
  } catch {
    Write-Host "sdk-install=ok"
    Write-Error "sdk-next=fix sdk check errors, then rerun: builda sdk check"
  }
}

function Install-UnitySdk {
  # Unity 项目：C# 包装层/jslib 装 Assets/Builda/，Builda WebGL 模板装 Assets/WebGLTemplates/
  # （Unity 硬性要求的模板位置），web 内核 mock 落 $ProjectStateDir/sdk/web/（与 h5 同构）。
  # 三个落点都归 SDK 所有，升级整体覆盖。
  $Meta = Invoke-RestMethod -Method Get -Uri "$AgentBase/agent/sdk/unity/latest.json"
  if (!$Meta.version -or !$Meta.url) { throw "SDK metadata invalid." }
  $Tmp = Join-Path ([System.IO.Path]::GetTempPath()) ("builda-sdk-unity-" + [System.Guid]::NewGuid().ToString() + ".zip")
  Invoke-WebRequest -UseBasicParsing $Meta.url -OutFile $Tmp
  $TmpDir = Join-Path ([System.IO.Path]::GetTempPath()) ("builda-sdk-unity-" + [System.Guid]::NewGuid().ToString())
  Expand-Archive -Force -Path $Tmp -DestinationPath $TmpDir
  Remove-Item -Force $Tmp
  if (!(Test-Path (Join-Path $TmpDir "Assets/Builda")) -or !(Test-Path (Join-Path $TmpDir ".builda-agent/sdk/web"))) {
    Remove-Item -Recurse -Force $TmpDir
    throw "Unity SDK zip layout invalid."
  }
  Remove-Item -Recurse -Force "Assets\Builda" -ErrorAction SilentlyContinue
  Remove-Item -Recurse -Force "Assets\WebGLTemplates\Builda" -ErrorAction SilentlyContinue
  Remove-Item -Recurse -Force (Join-Path $ProjectStateDir "sdk/web") -ErrorAction SilentlyContinue
  New-Item -ItemType Directory -Force -Path "Assets\WebGLTemplates" | Out-Null
  New-Item -ItemType Directory -Force -Path (Join-Path $ProjectStateDir "sdk") | Out-Null
  Copy-Item -Recurse -Force (Join-Path $TmpDir "Assets/Builda") "Assets\Builda"
  Copy-Item -Recurse -Force (Join-Path $TmpDir "Assets/WebGLTemplates/Builda") "Assets\WebGLTemplates\Builda"
  Copy-Item -Recurse -Force (Join-Path $TmpDir ".builda-agent/sdk/web") (Join-Path $ProjectStateDir "sdk/web")
  # BuildaMic 分析 Worker 归项目所有（L2 模板，CP 可魔改算法）：升级不覆盖已存在目录；
  # 恢复默认 = 删除 Assets\StreamingAssets\builda-mic 后重跑 sdk install
  if (Test-Path (Join-Path $TmpDir "Assets/StreamingAssets/builda-mic")) {
    if (Test-Path "Assets\StreamingAssets\builda-mic") {
      Write-Host "sdk-mic=preserved (Assets/StreamingAssets/builda-mic is project-owned; delete it and rerun sdk install to reset to defaults)"
    } else {
      New-Item -ItemType Directory -Force -Path "Assets\StreamingAssets" | Out-Null
      Copy-Item -Recurse -Force (Join-Path $TmpDir "Assets/StreamingAssets/builda-mic") "Assets\StreamingAssets\builda-mic"
      Write-Host "sdk-mic=Assets/StreamingAssets/builda-mic"
    }
  }
  Remove-Item -Recurse -Force $TmpDir
  $Meta.version | Set-Content -Encoding ASCII $SdkVersionFile
  Write-Host "sdk-version=$($Meta.version)"
  Write-Host "sdk-installed=Assets/Builda"
  Write-Host "sdk-engine=unity"
  Write-Host "sdk-next-1=Player Settings > Resolution and Presentation > WebGL Template 选 Builda（模板已装到 Assets/WebGLTemplates/Builda/，head 内已引用 builda-sdk.js）"
  Write-Host "sdk-next-2=Player Settings > Publishing Settings：Compression Format 选 Brotli 并勾选 Decompression Fallback（推荐），或选 Disabled；开压缩但不勾 Fallback 的产物会被上传校验拒绝"
  Write-Host "sdk-next-3=本地调试：builda dev --web <WebGL 构建目录或 zip>（dev server 自动从 $ProjectStateDir/sdk/web 兜底 serve mock SDK 与测试外壳）"
  try {
    Test-Sdk
  } catch {
    Write-Host "sdk-install=ok"
    Write-Error "sdk-next=fix sdk check errors, then rerun: builda sdk check"
  }
}

function Install-Sdk {
  # 引擎分流：godot 装 addon（现有全流程）；h5 装 web 内核；unity 装 C# 包装层 + 模板 + web 内核
  if ((Get-EngineValue ".") -eq "h5") {
    Install-WebSdk
    return
  }
  if ((Get-EngineValue ".") -eq "unity") {
    Install-UnitySdk
    return
  }
  if (!(Test-Path "project.godot")) { throw "project.godot missing. Run inside a Godot project root, run: builda sdk init, or use an HTML5 (index.html/package.json at root) or Unity (ProjectSettings/ProjectVersion.txt) project." }
  $Meta = Invoke-RestMethod -Method Get -Uri "$AgentBase/agent/sdk/latest.json"
  if (!$Meta.version -or !$Meta.url) { throw "SDK metadata invalid." }
  $Tmp = Join-Path ([System.IO.Path]::GetTempPath()) ("builda-sdk-" + [System.Guid]::NewGuid().ToString() + ".zip")
  Invoke-WebRequest -UseBasicParsing $Meta.url -OutFile $Tmp
  Remove-Item -Recurse -Force "addons\builda" -ErrorAction SilentlyContinue
  Expand-Archive -Force -Path $Tmp -DestinationPath .
  Remove-Item -Force $Tmp
  New-Item -ItemType Directory -Force -Path $ProjectStateDir | Out-Null
  $Meta.version | Set-Content -Encoding ASCII $SdkVersionFile
  Write-Host "sdk-version=$($Meta.version)"
  Write-Host "sdk-installed=addons/builda"
  Install-BuildaClient
  Set-WebHeadInclude
  try {
    Test-Sdk
  } catch {
    Write-Host "sdk-install=ok"
    Write-Error "sdk-next=fix sdk check errors, then rerun: builda sdk check"
  }
}

function Install-BuildaClient {
  $Example = Join-Path "addons\builda\examples" "BuildaClient.gd"
  if (!(Test-Path $Example)) { return }
  New-Item -ItemType Directory -Force -Path "scripts" | Out-Null
  $Target = Join-Path "scripts" "BuildaClient.gd"
  if (!(Test-Path $Target)) {
    Copy-Item -Force $Example $Target
    Write-Host "sdk-client=scripts/BuildaClient.gd"
  } else {
    Write-Host "sdk-client=scripts/BuildaClient.gd exists"
  }
  Set-BuildaAutoload
}

function Set-BuildaAutoload {
  if (!(Test-Path "project.godot")) { return }
  $Lines = New-Object System.Collections.Generic.List[string]
  $Lines.AddRange([string[]](Get-Content "project.godot"))
  $Result = New-Object System.Collections.Generic.List[string]
  $InAutoload = $false
  $SawAutoload = $false
  $Wrote = $false
  foreach ($Line in $Lines) {
    if ($Line -eq "[autoload]") {
      $InAutoload = $true
      $SawAutoload = $true
      $Result.Add($Line)
      continue
    }
    if ($Line -match '^\[') {
      if ($InAutoload -and !$Wrote) {
        $Result.Add('BuildaClient="*res://scripts/BuildaClient.gd"')
        $Wrote = $true
      }
      $InAutoload = $false
    }
    if ($InAutoload -and $Line -match '^BuildaClient=') {
      if (!$Wrote) {
        $Result.Add('BuildaClient="*res://scripts/BuildaClient.gd"')
        $Wrote = $true
      }
      continue
    }
    $Result.Add($Line)
  }
  if (!$Wrote) {
    if (!$SawAutoload) {
      $Result.Add("")
      $Result.Add("[autoload]")
    }
    $Result.Add('BuildaClient="*res://scripts/BuildaClient.gd"')
  }
  $Result | Set-Content -Encoding UTF8 "project.godot"
  Write-Host "sdk-autoload=BuildaClient"
}

function Set-WebHeadInclude {
  if (!(Test-Path "export_presets.cfg")) {
    Write-Host "sdk-web-head=export_presets.cfg missing"
    return
  }
  $Path = "export_presets.cfg"
  $Lines = [System.Collections.Generic.List[string]]::new()
  $Lines.AddRange([string[]](Get-Content $Path))
  $WebIds = @{}
  $Current = $null
  foreach ($Line in $Lines) {
    if ($Line -match '^\[preset\.(\d+)\]$') {
      $Current = $Matches[1]
      continue
    }
    if ($Line -match '^\[') {
      $Current = $null
      continue
    }
    if ($null -ne $Current -and $Line -eq 'platform="Web"') {
      $WebIds[$Current] = $true
    }
  }
  if ($WebIds.Count -eq 0) {
    Write-Host "sdk-web-head=no web preset"
    return
  }
  $Out = [System.Collections.Generic.List[string]]::new()
  $CurrentSection = $null
  $InWebOptions = $false
  $SeenHead = $false
  $Changed = $false
  $AlreadyOk = $false
  $Injected = 'html/head_include="<script src=\"builda-sdk.js\"></script>"'
  foreach ($Line in $Lines) {
    if ($Line -match '^\[preset\.(\d+)(\.options)?\]$') {
      if ($InWebOptions -and !$SeenHead) {
        $Out.Add($Injected)
        $Changed = $true
      }
      $CurrentSection = $Matches[1]
      $InWebOptions = $WebIds.ContainsKey($CurrentSection) -and ($Matches[2] -eq ".options")
      $SeenHead = $false
      $Out.Add($Line)
      continue
    }
    if ($Line -match '^\[') {
      if ($InWebOptions -and !$SeenHead) {
        $Out.Add($Injected)
        $Changed = $true
      }
      $CurrentSection = $null
      $InWebOptions = $false
      $SeenHead = $false
      $Out.Add($Line)
      continue
    }
    if ($null -ne $CurrentSection -and $WebIds.ContainsKey($CurrentSection) -and !$InWebOptions -and $Line.StartsWith("html/head_include=") -and $Line.Contains("builda-sdk.js")) {
      $Changed = $true
      continue
    }
    if ($InWebOptions -and $Line.StartsWith("html/head_include=")) {
      $SeenHead = $true
      if ($Line.Contains("builda-sdk.js")) {
        $Out.Add($Line.Replace("addons/builda/web/builda-sdk.js", "builda-sdk.js"))
        $AlreadyOk = $true
      } elseif ($Line -eq 'html/head_include=""') {
        $Out.Add($Injected)
        $Changed = $true
      } elseif ($Line.EndsWith('"')) {
        $Out.Add($Line.Substring(0, $Line.Length - 1) + '\n<script src=\"builda-sdk.js\"></script>"')
        $Changed = $true
      } else {
        $Out.Add($Line)
      }
      continue
    }
    $Out.Add($Line)
  }
  if ($InWebOptions -and !$SeenHead) {
    $Out.Add($Injected)
    $Changed = $true
  }
  if ($Changed) {
    $Out | Set-Content -Encoding UTF8 $Path
    Write-Host "sdk-web-head=builda-sdk.js injected"
  } elseif ($AlreadyOk) {
    Write-Host "sdk-web-head=builda-sdk.js exists"
  } else {
    Write-Host "sdk-web-head=builda-sdk.js exists"
  }
}

function Initialize-SdkProject {
  if (Test-Path "project.godot") {
    Write-Host "project.godot already exists"
    return
  }
  New-Item -ItemType Directory -Force -Path "scenes" | Out-Null
  New-Item -ItemType Directory -Force -Path "scripts" | Out-Null
  @'
; Engine configuration file.
; Minimal project skeleton generated by Builda SDK tooling for agent validation.

config_version=5

[application]
config/name="Builda SDK Minimal"
run/main_scene="res://scenes/main.tscn"
'@ | Set-Content -Encoding UTF8 "project.godot"
  @'
[gd_scene load_steps=2 format=3]

[ext_resource type="Script" path="res://scripts/main.gd" id="1_main"]

[node name="Main" type="Node"]
script = ExtResource("1_main")
'@ | Set-Content -Encoding UTF8 (Join-Path "scenes" "main.tscn")
  @'
extends Node

func _ready() -> void:
	if Engine.has_singleton("JavaScriptBridge"):
		print("Builda SDK minimal project loaded")
	else:
		print("Builda SDK minimal project loaded outside Web runtime")
'@ | Set-Content -Encoding UTF8 (Join-Path "scripts" "main.gd")
  Write-Host "sdk-init=ok"
  Write-Host "sdk-init-project=project.godot"
  Write-Host "sdk-init-next=builda sdk install && builda sdk smoke"
}

function Test-WebSdk {
  $Dst = Join-Path $ProjectStateDir "sdk/web"
  if (!(Test-Path (Join-Path $Dst "builda-sdk.js"))) { throw "Missing $Dst/builda-sdk.js. Run: builda sdk install" }
  if (!(Test-Path (Join-Path $Dst "builda-sdk.d.ts"))) { throw "Missing $Dst/builda-sdk.d.ts. Run: builda sdk install" }
  if (Test-Path $SdkVersionFile) {
    Write-Host "sdk-version=$((Get-Content $SdkVersionFile -TotalCount 1).Trim())"
  } else {
    Write-Host "sdk-version=unknown"
    Write-Host "sdk-upgrade-hint=run builda sdk install to record the installed SDK version"
  }
  Test-SdkCompat
  Write-Host "sdk=ok"
  Write-Host "sdk-engine=h5"
  Write-Host "sdk-web=$Dst"
}

function Test-UnitySdk {
  $Dst = Join-Path $ProjectStateDir "sdk/web"
  if (!(Test-Path "Assets/Builda/Runtime/Builda.cs")) { throw "Missing Assets/Builda/Runtime/Builda.cs. Run: builda sdk install" }
  if (!(Test-Path "Assets/Builda/Plugins/WebGL/Builda.jslib")) { throw "Missing Assets/Builda/Plugins/WebGL/Builda.jslib. Run: builda sdk install" }
  if (!(Test-Path "Assets/Builda/Runtime/BuildaMic.cs")) { throw "Missing Assets/Builda/Runtime/BuildaMic.cs. Run: builda sdk install" }
  if (!(Test-Path "Assets/Builda/Plugins/WebGL/BuildaMic.jslib")) { throw "Missing Assets/Builda/Plugins/WebGL/BuildaMic.jslib. Run: builda sdk install" }
  if (!(Test-Path "Assets/WebGLTemplates/Builda/index.html")) { throw "Missing Assets/WebGLTemplates/Builda/index.html. Run: builda sdk install" }
  if (!(Select-String -Path "Assets/WebGLTemplates/Builda/index.html" -Pattern "builda-sdk.js" -SimpleMatch -Quiet)) {
    throw "Builda WebGL template lost its builda-sdk.js reference. Run: builda sdk install"
  }
  if (!(Test-Path (Join-Path $Dst "builda-sdk.js"))) { throw "Missing $Dst/builda-sdk.js (mock SDK for builda dev). Run: builda sdk install" }
  if (Test-Path $SdkVersionFile) {
    $InstalledSdkVersion = (Get-Content $SdkVersionFile -TotalCount 1).Trim()
    Write-Host "sdk-version=$InstalledSdkVersion"
    $CsLine = Get-Content "Assets/Builda/Runtime/Builda.cs" | Where-Object { $_ -match 'SdkVersion = "([^"]*)"' } | Select-Object -First 1
    if ($CsLine -match 'SdkVersion = "([^"]*)"') {
      $CsVersion = $Matches[1]
      if ($InstalledSdkVersion -and $CsVersion -ne $InstalledSdkVersion) {
        throw "SDK version mismatch: $SdkVersionFile=$InstalledSdkVersion but Assets/Builda/Runtime/Builda.cs=$CsVersion. Run: builda sdk install"
      }
    }
  } else {
    Write-Host "sdk-version=unknown"
    Write-Host "sdk-upgrade-hint=run builda sdk install to record the installed SDK version"
  }
  Test-SdkCompat
  # 分析 Worker 项目所有，缺失只提示不判失败（BuildaMic 可选能力）
  if (Test-Path "Assets/StreamingAssets/builda-mic/builda-mic-worker.js") {
    Write-Host "sdk-mic=Assets/StreamingAssets/builda-mic user-owned"
  } else {
    Write-Host "sdk-mic=missing (rerun builda sdk install to restore the BuildaMic analysis worker)"
  }
  Write-Host "sdk=ok"
  Write-Host "sdk-engine=unity"
  Write-Host "sdk-unity=Assets/Builda"
}

# Get-SdkMockPath 当前项目 mock SDK 的落点（引擎分流；smoke 用；
# h5/unity 都落 $ProjectStateDir/sdk/web/，无 addons 时自动命中）
function Get-SdkMockPath {
  $WebMock = Join-Path $ProjectStateDir "sdk/web/builda-sdk.js"
  if ((Test-Path $WebMock) -and !(Test-Path "addons/builda/web/builda-sdk.js")) { return $WebMock }
  return "addons/builda/web/builda-sdk.js"
}

function Test-Sdk {
  if ((Get-EngineValue ".") -eq "h5") {
    Test-WebSdk
    return
  }
  if ((Get-EngineValue ".") -eq "unity") {
    Test-UnitySdk
    return
  }
  if (!(Test-Path "addons/builda/builda.gd")) { throw "Missing addons/builda/builda.gd. Run: builda sdk install" }
  if (!(Test-Path "addons/builda/web/builda-sdk.js")) { throw "Missing addons/builda/web/builda-sdk.js. Run: builda sdk install" }
  if (!(Test-Path "addons/builda/web/builda-sdk.d.ts")) { throw "Missing addons/builda/web/builda-sdk.d.ts. Run: builda sdk install" }
  $InstalledSdkVersion = ""
  if (Test-Path $SdkVersionFile) {
    $InstalledSdkVersion = (Get-Content $SdkVersionFile -TotalCount 1).Trim()
    Write-Host "sdk-version=$InstalledSdkVersion"
  } else {
    Write-Host "sdk-version=unknown"
    Write-Host "sdk-upgrade-hint=run builda sdk install to record the installed SDK version"
  }
  $PluginVersion = ""
  if (Test-Path "addons/builda/plugin.cfg") {
    $PluginLine = Get-Content "addons/builda/plugin.cfg" | Where-Object { $_ -match '^version="([^"]*)"' } | Select-Object -First 1
    if ($PluginLine -match '^version="([^"]*)"') { $PluginVersion = $Matches[1] }
  }
  if ($PluginVersion) {
    Write-Host "sdk-addon-version=$PluginVersion"
    if ($InstalledSdkVersion -and $InstalledSdkVersion -ne $PluginVersion) {
      throw "SDK version mismatch: $SdkVersionFile=$InstalledSdkVersion but addons/builda/plugin.cfg=$PluginVersion. Run: builda sdk install"
    }
  } else {
    Write-Host "sdk-addon-version=unknown"
  }
  if ((Test-Path "project.godot") -and (Select-String -Path "project.godot" -Pattern '^BuildaClient="\*res://scripts/BuildaClient\.gd"' -Quiet) -and !(Test-Path "scripts/BuildaClient.gd")) {
    throw "Autoload BuildaClient points to missing scripts/BuildaClient.gd. Run: builda sdk install"
  }
  if (Test-Path "scripts/BuildaClient.gd") { Write-Host "sdk-client=scripts/BuildaClient.gd user-owned" } else { Write-Host "sdk-client=missing" }
  Test-SdkCompat
  Write-Host "sdk=ok"
  Write-Host "sdk-addon=addons/builda"
}

function Invoke-SdkSmoke {
  Test-Sdk | Out-Null
  if (!(Get-Command node -ErrorAction SilentlyContinue)) {
    Write-Host "sdk-smoke=manual"
    Write-Host "Node.js not found. Export/run the Godot Web build with:"
    Write-Host "?gameId=local-game&builda_mock_player=alice&builda_mock_name=Alice"
    Write-Host "Then call Builda.whoami() and Builda.privateKV.set/get/remove."
    return
  }
  $Script = @'
const fs = require("fs");
const vm = require("vm");
const store = new Map();
global.window = {
  location: { search: "?gameId=local-game&builda_mock_player=alice&builda_mock_name=Alice" },
  localStorage: {
    getItem: (key) => store.has(key) ? store.get(key) : null,
    setItem: (key, value) => store.set(key, String(value)),
    removeItem: (key) => store.delete(key),
    key: (i) => Array.from(store.keys())[i] ?? null,
    get length() { return store.size; }
  }
};
global.URLSearchParams = URLSearchParams;
global.fetch = async () => { throw new Error("local mock smoke must not call fetch"); };
vm.runInThisContext(fs.readFileSync(process.env.BUILDA_SMOKE_SDK || "addons/builda/web/builda-sdk.js", "utf8"));
(async () => {
  const who = await window.Builda.whoami();
  if (!who.ok || who.data.id !== "alice") throw new Error("whoami mock failed");
  const save = new TextEncoder().encode(JSON.stringify({ count: 3 }));
  const set = await window.Builda.privateKV.set("wins", save);
  if (!set.ok) throw new Error("privateKV.set mock failed");
  const got = await window.Builda.privateKV.get("wins");
  if (!got.ok || !(got.data instanceof Uint8Array) || JSON.parse(new TextDecoder().decode(got.data)).count !== 3) throw new Error("privateKV.get mock failed");
  const setMany = await window.Builda.privateKV.setMany({ a: new Uint8Array([1]), b: new Uint8Array([2, 2]) });
  if (!setMany.ok) throw new Error("privateKV.setMany mock failed");
  const gotMany = await window.Builda.privateKV.getMany(["a", "b", "absent"]);
  if (!gotMany.ok || gotMany.data.entries.a.length !== 1 || gotMany.data.entries.b.length !== 2 || gotMany.data.entries.absent !== null) throw new Error("privateKV.getMany mock failed");
  const tooBig = await window.Builda.privateKV.set("big", new Uint8Array(32 * 1024 + 1));
  if (tooBig.ok || tooBig.error.code !== "VALUE_TOO_LARGE") throw new Error("privateKV value size limit not enforced");
  const removedMany = await window.Builda.privateKV.removeMany(["a", "b"]);
  if (!removedMany.ok) throw new Error("privateKV.removeMany mock failed");
  const removed = await window.Builda.privateKV.remove("wins");
  if (!removed.ok) throw new Error("privateKV.remove mock failed");
  const missing = await window.Builda.privateKV.get("wins");
  if (!missing.ok || missing.data !== null) throw new Error("privateKV.remove verification failed");
  console.log("sdk-smoke=ok");
  console.log("sdk-mode=local-mock");
  console.log("sdk-smoke-verified=whoami,privateKV.set,privateKV.get,privateKV.setMany,privateKV.getMany,privateKV.removeMany,privateKV.remove");
  console.log("sdk-next=export Godot Web build and run it with local mock URL parameters");
  console.log("sdk-next-url=?gameId=local-game&builda_mock_player=alice&builda_mock_name=Alice");
})().catch((err) => {
  console.error(err && err.stack || err);
  process.exit(1);
});
'@
  $env:BUILDA_SMOKE_SDK = Get-SdkMockPath
  try {
    $Script | node
  } finally {
    Remove-Item Env:BUILDA_SMOKE_SDK -ErrorAction SilentlyContinue
  }
  if ($LASTEXITCODE -ne 0) { throw "sdk smoke failed" }
}

function Save-Grant {
  param([string]$Grant)
  if (!$Grant) { throw "empty grant" }
  New-Item -ItemType Directory -Force -Path $AuthDir | Out-Null
  $Grant | Set-Content -Encoding ASCII $TokenPath
  Write-Host "Agent grant saved for 30 minutes."
}

function Save-Refresh {
  param([string]$Refresh)
  if (!$Refresh) { return }
  New-Item -ItemType Directory -Force -Path $AuthDir | Out-Null
  $Refresh | Set-Content -Encoding ASCII $RefreshPath
  Write-Host "Agent authorization saved for 30 days."
}

function Grant-NeedsRefresh {
  if (!(Test-Path $RefreshPath)) { return $false }
  if (!(Test-Path $TokenPath)) { return $true }
  $Age = (Get-Date) - (Get-Item $TokenPath).LastWriteTime
  return $Age.TotalSeconds -ge 1500
}

function Test-GrantFresh {
  if (!(Test-Path $TokenPath)) { return $false }
  $Age = (Get-Date) - (Get-Item $TokenPath).LastWriteTime
  return $Age.TotalSeconds -lt 1500
}

function Refresh-Grant {
  if (!(Test-Path $RefreshPath)) { throw "No saved agent authorization." }
  $Refresh = (Get-Content $RefreshPath -Raw).Trim()
  if (!$Refresh) { throw "Empty saved agent authorization." }
  $Resp = Invoke-RestMethod -Method Post -Uri "$Base/api/v1/agent/auth/refresh" -ContentType "application/json" -Body (@{ refresh_token = $Refresh } | ConvertTo-Json)
  if (!$Resp.token) { throw "Refresh did not return an agent grant." }
  Save-Grant $Resp.token | Out-Null
  Write-Host "Agent grant refreshed automatically."
}

function Auth-WithPairCode {
  param([string]$Code)
  try {
    $Resp = Invoke-RestMethod -Method Post -Uri "$Base/api/v1/agent/exchange" -ContentType "application/json" -Body (@{ code = $Code } | ConvertTo-Json)
    if (!$Resp.token) { throw "Exchange failed." }
    Save-Grant $Resp.token
    Save-Refresh $Resp.refresh_token
  } catch {
    Write-Host "Pair code exchange failed or expired. Starting browser authorization instead."
    Auth-WithBrowser
  }
}

function Auth-WithBrowser {
  $AuthBody = if ($SiteOrigin) { @{ site_origin = $SiteOrigin } | ConvertTo-Json -Compress } else { "{}" }
  $Resp = Invoke-RestMethod -Method Post -Uri "$Base/api/v1/agent/auth/start" -ContentType "application/json" -Body $AuthBody
  if (!$Resp.auth_id -or !$Resp.authorize_url) { throw "Auth start failed." }
  $Interval = if ($Resp.interval) { [int]$Resp.interval } else { 2 }
  $Expires = if ($Resp.expires_in) { [int]$Resp.expires_in } else { 600 }
  $ActivateUrl = if ($Resp.activate_url) { [string]$Resp.activate_url } else { "$Base/activate" }

  Write-Host "I need your BuildaGame permission to upload a draft."
  Write-Host "Opening the browser authorization page..."
  try {
    Start-Process $Resp.authorize_url | Out-Null
  } catch {
    Write-Host "Open this URL in your browser:"
    Write-Host $Resp.authorize_url
  }
  Write-Host "If the browser did not open, visit:"
  Write-Host $Resp.authorize_url
  if ($Resp.user_code) {
    Write-Host ""
    Write-Host "On another device, open:"
    Write-Host $ActivateUrl
    Write-Host "Enter code: $($Resp.user_code)"
  }
  Write-Host "Log in or sign up there, click Allow agent, then return here."

  $Elapsed = 0
  while ($Elapsed -lt $Expires) {
    Start-Sleep -Seconds $Interval
    $Elapsed += $Interval
    $Poll = Invoke-RestMethod -Method Get -Uri "$Base/api/v1/agent/auth/status?auth_id=$($Resp.auth_id)"
    if ($Poll.status -eq "approved") {
      Save-Grant $Poll.token
      Save-Refresh $Poll.refresh_token
      return
    }
    if ($Poll.status -eq "expired") {
      throw "Authorization expired. Run: builda auth"
    }
    Write-Host -NoNewline "."
  }
  Write-Host ""
  throw "Authorization timed out. Run: builda auth"
}

function Ensure-Authorization {
  param([string]$Code)
  if (Test-GrantFresh) {
    Write-Host "authorization=ready"
    Write-Host "grant=present"
    if (Test-Path $RefreshPath) { Write-Host "agent-authorization=present" } else { Write-Host "agent-authorization=missing" }
    return
  }
  if (Test-Path $RefreshPath) {
    try {
      Refresh-Grant
      Write-Host "authorization=ready"
      Write-Host "grant=present"
      Write-Host "agent-authorization=present"
      return
    } catch {
      Write-Host "Saved agent authorization could not refresh; starting browser authorization."
    }
  }
  if ($Code) {
    Auth-WithPairCode $Code
  } else {
    Auth-WithBrowser
  }
  Write-Host "authorization=ready"
  Write-Host "grant=present"
  if (Test-Path $RefreshPath) { Write-Host "agent-authorization=present" } else { Write-Host "agent-authorization=missing" }
}

$Cmd = if ($args.Count -gt 0) { $args[0] } else { "" }
$Rest = if ($args.Count -gt 1) { $args[1..($args.Count - 1)] } else { @() }
if ($Cmd -notin @("", "-h", "--help", "help", "version", "--version", "-v")) {
  Require-ProjectContext -OriginalArgs $args
}
Maybe-SelfUpdate -OriginalArgs $args

switch ($Cmd) {
  "auth" {
    $AuthArg = if ($Rest.Count -gt 0) { $Rest[0] } else { "" }
    if ($AuthArg -eq "--force") {
      Auth-WithBrowser
      Write-Host "authorization=ready"
      Write-Host "grant=present"
      Write-Host "agent-authorization=present"
    } else {
      $Code = if ($AuthArg) { $AuthArg } else { $PairCode }
      Ensure-Authorization $Code
    }
  }
  "check" {
    Check-SelfUpdate
    Sync-ProjectRegistrations *> $null
    Write-Host "project-registrations=ensured"
    Write-Host "BUILDA_BASE=$Base"
    Write-Host "BUILDA_AGENT_BASE=$AgentBase"
    if (Test-Path $TokenPath) { Write-Host "grant=present" } else { Write-Host "grant=missing" }
    if (Test-Path $RefreshPath) { Write-Host "agent-authorization=present" } else { Write-Host "agent-authorization=missing" }
    if (Get-Command curl.exe -ErrorAction SilentlyContinue) { Write-Host "curl=ok" } else { Write-Host "curl=missing" }
    Write-Host "engine=$(Get-EngineValue '.')"
    if (Test-Path $ProjectFile) {
      $ProjectGameId = Get-ProjectGameId
      if ($ProjectGameId) { Write-Host "project-game-id=$ProjectGameId" } else { Write-Host "project-game-id=invalid" }
      Write-Host "project-state-dir=$(Split-Path -Parent $ProjectFile)"
    } elseif (Test-Path $LegacyProjectFile) {
      $ProjectGameId = Get-ProjectGameId
      if ($ProjectGameId) { Write-Host "project-game-id=$ProjectGameId" } else { Write-Host "project-game-id=invalid" }
      Write-Host "project-binding=$ProjectFile"
      Write-Host "project-state-dir=$(Split-Path -Parent $ProjectFile)"
    } else {
      Write-Host "project-game-id=missing"
    }
  }
  "update" {
    try {
      $Remote = Invoke-RestMethod -Method Get -Uri "$AgentBase/agent/version"
      Update-BuildaTool -NewVersion $Remote.version
    } catch {
      Update-BuildaTool
    }
  }
  "sync-project" {
    Write-Host "Agent registrations:"
    Sync-ProjectRegistrations
  }
  "engine" {
    $Sub = if ($Rest.Count -gt 0) { $Rest[0] } else { "" }
    switch ($Sub) {
      "detect" {
        $Dir = if ($Rest.Count -gt 1) { $Rest[1] } else { "." }
        Invoke-EngineDetect $Dir
      }
      { $_ -in @("", "-h", "--help", "help") } {
        Write-Host "Usage: builda engine detect [dir]"
      }
      default {
        Write-Error "Unknown engine command: $Sub"
        Write-Host "Usage: builda engine detect [dir]"
        exit 1
      }
    }
  }
  "version" {
    Write-Host $Version
  }
  "--version" {
    Write-Host $Version
  }
  "-v" {
    Write-Host $Version
  }
  "new-manifest" {
    Write-DefaultManifest
  }
  "dev" {
    Invoke-DevServer -Args $Rest
  }
  "bundle-check" {
    if ($Rest.Count -lt 1) { throw "zip file required" }
    $WebViewCompat = $false
    $Engine = ""
    $ZipArg = ""
    for ($I = 0; $I -lt $Rest.Count; $I++) {
      switch -Regex ($Rest[$I]) {
        "^--webview-compatible$" { $WebViewCompat = $true }
        "^--engine$" {
          $I++
          if ($I -ge $Rest.Count) { throw "Usage: builda bundle-check [--engine godot|h5|unity] [--webview-compatible] <zip>" }
          $Engine = $Rest[$I]
        }
        "^--engine=(.*)$" { $Engine = $Matches[1] }
        default { $ZipArg = $Rest[$I] }
      }
    }
    if (!$ZipArg) { throw "zip file required" }
    if ($env:BUILDA_WEBVIEW_COMPAT -eq "1") { $WebViewCompat = $true }
    Test-BundleZip $ZipArg $WebViewCompat $Engine
  }
  "assets" {
    $Sub = if ($Rest.Count -gt 0) { $Rest[0] } else { "" }
    switch ($Sub) {
      "check" {
        if ($Rest.Count -lt 2) { throw "Usage: builda assets check <zip>" }
        Test-AssetsZip $Rest[1]
      }
      "upload" {
        if ($Rest.Count -lt 2) { throw "Usage: builda assets upload <zip>" }
        Upload-AssetsZip $Rest[1]
      }
      { $_ -in @("", "-h", "--help", "help") } {
        Write-Host "Usage: builda assets check <zip> | upload <zip>"
      }
      default {
        Write-Error "Unknown assets command: $Sub"
        Write-Host "Usage: builda assets check <zip> | upload <zip>"
        exit 1
      }
    }
  }
  "webrtc" {
    Write-Host "multiplayer=unsupported"
    Write-Error "Builda 联机能力开发中、尚未开放，webrtc 命令已下线。"
    exit 1
  }
  "backend" {
    Write-Host "multiplayer=unsupported"
    Write-Error "Builda 联机能力开发中、尚未开放，backend 命令已下线。"
    exit 1
  }
  "sdk" {
    $Sub = if ($Rest.Count -gt 0) { $Rest[0] } else { "" }
    switch ($Sub) {
      "init" { Initialize-SdkProject }
      "install" { Install-Sdk }
      "check" { Test-Sdk }
      "smoke" { Invoke-SdkSmoke }
      "key-audit" { Show-PersistentIdentifierAudit }
      { $_ -in @("", "-h", "--help", "help") } {
        Write-Host "Usage: builda sdk init | install | check | smoke | key-audit"
      }
      default {
        Write-Error "Unknown sdk command: $Sub"
        Write-Host "Usage: builda sdk init | install | check | smoke | key-audit"
        exit 1
      }
    }
  }
  "upload-build" {
    Need-Token
    if ($Rest.Count -lt 1 -or !(Test-Path $Rest[0])) { throw "zip file required" }
    $Engine = Get-ManifestEngine
    Test-BundleZip $Rest[0] $false $Engine | Out-Null
    $Zip = Resolve-Path $Rest[0]
    if (!(Get-Command curl.exe -ErrorAction SilentlyContinue)) { throw "curl.exe is required for build upload on Windows." }
    $Raw = & curl.exe -fsS -X POST "$Base/api/v1/uploads/build" -H "Authorization: Bearer $(Get-Token)" -F "file=@$Zip" -F "engine=$Engine"
    if ($LASTEXITCODE -ne 0) { throw "build upload failed" }
    $Resp = $Raw | ConvertFrom-Json
    $Resp | ConvertTo-Json -Depth 8 | Set-Content -Encoding UTF8 (Join-Path $Root "last-build.json")
    $Resp | ConvertTo-Json -Depth 8
    if ($Resp.prefix) {
      if (!(Test-Path $Manifest)) { Write-DefaultManifest }
      $Data = Get-Content $Manifest -Raw | ConvertFrom-Json
      $Data.buildPrefix = $Resp.prefix
      $Data.buildEntry = if ($Resp.entry) { $Resp.entry } else { "index.html" }
      $Data.buildSize = if ($Resp.size) { $Resp.size } else { 0 }
      $Data.bundleUrl = if ($Resp.bundleUrl) { $Resp.bundleUrl } else { "" }
      $Data.bundleMd5 = if ($Resp.bundleMd5) { $Resp.bundleMd5 } else { "" }
      $Data.bundleVersion = if ($Resp.bundleVersion) { $Resp.bundleVersion } else { $Resp.prefix }
      $Data.bundleEntry = if ($Resp.bundleEntry) { $Resp.bundleEntry } else { $Data.buildEntry }
      $Data.bundleSize = if ($Resp.bundleSize) { $Resp.bundleSize } else { 0 }
      $Data | ConvertTo-Json -Depth 8 | Set-Content -Encoding UTF8 $Manifest
      Write-Host "Updated $Manifest with build and H5 Bundle fields."
    }
  }
  "create-draft" {
    $File = if ($Rest.Count -gt 0) { $Rest[0] } else { $Manifest }
    if ($File -eq "-h" -or $File -eq "--help" -or $File -eq "help") {
      Write-Host "Usage: builda create-draft [manifest]"
      return
    }
    if (!(Test-Path $File)) { throw "manifest not found: $File" }
    Invoke-DraftPreflight $File
    Set-ManifestSdkVersion $File
    Need-Token
    Migrate-LegacyProjectFile
    if (Test-Path $ProjectFile) {
      $ExistingId = Get-ProjectGameId
      if ($ExistingId) { throw "$ProjectFile already binds this project to $ExistingId. Use: builda update-draft" }
    }
    $Headers = @{ Authorization = "Bearer $(Get-Token)" }
    $Resp = Invoke-RestMethod -Method Post -Uri (Get-CmdUrl "game/create") -Headers $Headers -ContentType "application/json" -InFile $File
    Assert-CmdResponseOk $Resp "create draft"
    $Resp | ConvertTo-Json -Depth 8 | Set-Content -Encoding UTF8 (Join-Path $Root "last-draft.json")
    $GameId = $Resp.body.game.id
    $VersionId = if ($Resp.body.game.versionId) { $Resp.body.game.versionId } else { $Resp.body.game.draftVersionId }
    if ($GameId) {
      $GameId | Set-Content -Encoding ASCII (Join-Path $Root "last-game-id")
      $GameId | Set-Content -Encoding ASCII (Join-Path $Root "last-draft-id")
    }
    if ($VersionId) { $VersionId | Set-Content -Encoding ASCII (Join-Path $Root "last-version-id") }
    Write-ProjectGame $GameId (Get-ManifestTitle $File) $VersionId
    Write-Host "sync=ok builda-current-version=$VersionId cysj-stage=pending-app-publish"
    $Resp | ConvertTo-Json -Depth 8
  }
  "update-draft" {
    $Id = if ($Rest.Count -gt 0) { $Rest[0] } else { "" }
    if ($Id -eq "-h" -or $Id -eq "--help" -or $Id -eq "help") {
      Write-Host "Usage: builda update-draft [game_id] [manifest]"
      return
    }
    $File = if ($Rest.Count -gt 1) { $Rest[1] } else { $Manifest }
    if ($Id -and (Test-Path $Id)) {
      $File = $Id
      $Id = ""
    }
    if (!$Id) { $Id = Get-ProjectGameId }
    $LastGameId = Join-Path $Root "last-game-id"
    if (!$Id -and (Test-Path $LastGameId)) {
      $WeakId = (Get-Content $LastGameId -Raw).Trim()
      throw "No $ProjectFile in this project. Last global game id is $WeakId, but it may belong to another project. Pass the intended id explicitly once: builda update-draft $WeakId"
    }
    if (!$Id) { throw "game id required. Put $ProjectStateDir/game.json in this project or pass game id explicitly." }
    if (!(Test-Path $File)) { throw "manifest not found: $File" }
    Invoke-DraftPreflight $File
    Set-ManifestSdkVersion $File
    Need-Token
    $Data = Get-Content $File -Raw | ConvertFrom-Json
    $Data | Add-Member -NotePropertyName id -NotePropertyValue $Id -Force
    $Body = $Data | ConvertTo-Json -Depth 8
    $Headers = @{ Authorization = "Bearer $(Get-Token)" }
    $Resp = Invoke-RestMethod -Method Post -Uri (Get-CmdUrl "game/updatedraft") -Headers $Headers -ContentType "application/json" -Body $Body
    Assert-CmdResponseOk $Resp "update draft"
    $Resp | ConvertTo-Json -Depth 8 | Set-Content -Encoding UTF8 (Join-Path $Root "last-draft.json")
    $GameId = $Resp.body.game.id
    $VersionId = if ($Resp.body.game.versionId) { $Resp.body.game.versionId } else { $Resp.body.game.draftVersionId }
    if (!$GameId) { $GameId = $Id }
    if ($GameId) {
      $GameId | Set-Content -Encoding ASCII (Join-Path $Root "last-game-id")
      $GameId | Set-Content -Encoding ASCII (Join-Path $Root "last-draft-id")
    }
    if ($VersionId) { $VersionId | Set-Content -Encoding ASCII (Join-Path $Root "last-version-id") }
    Write-ProjectGame $GameId (Get-ManifestTitle $File) $VersionId
    Write-Host "sync=ok builda-current-version=$VersionId cysj-stage=pending-app-publish"
    $Resp | ConvertTo-Json -Depth 8
  }
  "status" {
    Write-Host "root=$Root"
    if (Test-Path $TokenPath) { Write-Host "grant=present" } else { Write-Host "grant=missing" }
    if (Test-Path $RefreshPath) { Write-Host "agent-authorization=present" } else { Write-Host "agent-authorization=missing" }
    if (Test-Path $Manifest) { Write-Host "manifest=$Manifest" }
    if (Test-Path $ProjectFile) {
      $ProjectGameId = Get-ProjectGameId
      if ($ProjectGameId) { Write-Host "project-game-id=$ProjectGameId" } else { Write-Host "project-game-id=invalid" }
      Write-Host "project-binding=$ProjectFile"
      Write-Host "project-state-dir=$(Split-Path -Parent $ProjectFile)"
    }
    $LastBuild = Join-Path $Root "last-build.json"
    if (Test-Path $LastBuild) { Write-Host "last-build=$LastBuild" }
    $LastAssets = Join-Path $Root "last-assets.json"
    if (Test-Path $LastAssets) { Write-Host "last-assets=$LastAssets" }
    $LastGameId = Join-Path $Root "last-game-id"
    if (Test-Path $LastGameId) { Write-Host "last-game-id=$((Get-Content $LastGameId -Raw).Trim()) (weak receipt, not project identity)" }
    $LastVersionId = Join-Path $Root "last-version-id"
    if (Test-Path $LastVersionId) { Write-Host "last-version-id=$((Get-Content $LastVersionId -Raw).Trim())" }
  }
  "uninstall" {
    Invoke-Uninstall $Rest
  }
  { $_ -in @("", "-h", "--help", "help") } {
    Show-Usage
  }
  default {
    Write-Error "Unknown command: $Cmd"
    Show-Usage
    exit 1
  }
}
