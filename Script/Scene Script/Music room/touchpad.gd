extends Touchpad
var s : float
func _ready() -> void:
	初始化()
	拖拽时.connect(_on_拖拽时)
	MuiscEngine.音乐.音乐选项 = -1
func _process(delta: float) -> void:
	p2(delta)
	create_tween().tween_property($"../ScrollContainer","scroll_vertical",s,0.3)
	if s <= 0:
		s = 0
func _on_拖拽时(event: InputEventScreenDrag, 控制器: Touchpad) -> void:
	s -= event.screen_relative.y
