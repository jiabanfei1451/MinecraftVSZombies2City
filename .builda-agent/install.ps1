$ErrorActionPreference = "Stop"

$Code = ""
$SiteOrigin = ""
for ($i = 0; $i -lt $args.Count; $i++) {
  if ($args[$i] -eq "--code" -and ($i + 1) -lt $args.Count) {
    $Code = $args[$i + 1]
    $i++
  } elseif ($args[$i] -eq "--site-origin" -and ($i + 1) -lt $args.Count) {
    $SiteOrigin = $args[$i + 1]
    $i++
  }
}

if ($SiteOrigin -and $SiteOrigin -notmatch '^https://[A-Za-z0-9](?:[A-Za-z0-9.-]*[A-Za-z0-9])?$') {
  Write-Error "Invalid --site-origin: expected https://<hostname> without path, port or credentials"
  exit 2
}

if ($PWD.Path -eq $HOME) {
  Write-Error "BuildaGame 工具链是项目级的:请先 cd 到游戏项目根目录再运行(Godot:有 project.godot 的目录;HTML5:含 index.html 或 package.json 的工程根;Unity:含 ProjectSettings 的工程根;或用于试装的空目录)"
  exit 1
}

$Root = Join-Path $PWD ".builda-agent"
New-Item -ItemType Directory -Force -Path $Root | Out-Null

$SkillPath = Join-Path $Root "SKILL.md"
Invoke-WebRequest -UseBasicParsing "https://ai.builda.game/agent/releases/0.3.1/builda-skill.md" -OutFile $SkillPath

$CliPath = Join-Path $Root "builda.ps1"
Invoke-WebRequest -UseBasicParsing "https://ai.builda.game/agent/releases/0.3.1/builda.ps1" -OutFile $CliPath

$VersionPath = Join-Path $Root "VERSION"
"0.3.1" | Set-Content -Encoding ASCII $VersionPath

$EnvPath = Join-Path $Root "publish.env"
@"
BUILDA_BASE=https://builda-godot-api.poni.fun
BUILDA_AGENT_BASE=https://ai.builda.game
"@ | Set-Content -Encoding UTF8 $EnvPath
if ($SiteOrigin) {
  "BUILDA_SITE_ORIGIN=$SiteOrigin" | Add-Content -Encoding UTF8 $EnvPath
}
"BUILDA_PAIR_CODE=$Code" | Add-Content -Encoding UTF8 $EnvPath

$CmdPath = Join-Path $Root "builda.cmd"
@"
@echo off
powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0builda.ps1" %*
"@ | Set-Content -Encoding ASCII $CmdPath

$env:BUILDA_DISABLE_AUTO_UPDATE = "1"
& powershell -NoProfile -ExecutionPolicy Bypass -File "$Root\builda.ps1" sync-project
$SyncExit = $LASTEXITCODE
Remove-Item Env:BUILDA_DISABLE_AUTO_UPDATE -ErrorAction SilentlyContinue
if ($SyncExit -ne 0) {
  exit $SyncExit
}

Write-Host "BuildaGame skill installed at $SkillPath (project-level)"
Write-Host "Builda CLI installed at $CliPath (project-level)"
if ($Code) {
  Write-Host "Pair code saved for the installation authorization step."
} else {
  Write-Host "Authorization is checked immediately after installation; existing authorization is reused."
}
Write-Host "Next: .builda-agent\builda.cmd check && .builda-agent\builda.cmd auth"
