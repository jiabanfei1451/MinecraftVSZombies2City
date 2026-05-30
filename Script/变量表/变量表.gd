extends Node
static var 窗口模式 : int = 4
static var 窗口拉伸模式 : int = 1
static var 卡槽提示词动画 : bool
static var 窗口缩放 : float = 1
@export_range(0,10,0.4) var 卡槽数量 : int = 6

func is_windows_admin() -> bool:
	var dir = OS.get_environment("WINDIR") + "\\System32"
	var test_path = dir + "\\test.tmp"
	var file = FileAccess.open(test_path, FileAccess.WRITE)
	if file:
		file.close()
		DirAccess.remove_absolute(test_path)
		return true
	return false

func _process(delta: float) -> void:
	get_window().content_scale_factor = 窗口缩放
	get_window().content_scale_aspect = 窗口模式
	get_window().sdf_scale = Viewport.SDF_SCALE_25_PERCENT
	if 窗口拉伸模式 == 0:
		get_window().content_scale_mode = Window.CONTENT_SCALE_MODE_VIEWPORT
	if 窗口拉伸模式 == 1:
		get_window().content_scale_mode = Window.CONTENT_SCALE_MODE_CANVAS_ITEMS
	if 窗口拉伸模式 == 2:
		get_window().content_scale_mode = Window.CONTENT_SCALE_MODE_DISABLED

func _ready() -> void:
	var d
	d = 存档.load_数据("Window-set.cfg","Window","Window_Scale")
	if d != null:
		窗口缩放 = d[0]
	d = 存档.load_数据("Window-set.cfg","Window","Window_size_mode")
	if d != null:
		窗口拉伸模式 = d[0]
	d = 存档.load_数据("Window-set.cfg","Window","Window_mode")
	if d != null:
		窗口模式 = d[0]
