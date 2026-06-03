extends Touchpad
@export var 上升向量 : float
var 指数 : float
func _ready() -> void:
	初始化()
	拖拽时.connect(_on_拖拽时)
func _process(delta: float) -> void:
	$"../VBoxContainer".size.y = 648
	$"../VScrollBar".max_value = $"../VBoxContainer".size.y - 648
	$"../VScrollBar".value = $"../VBoxContainer".position.y
	p2(delta)
	if 上升向量 != 0:
		上升向量 *= 0.9
		$"../VBoxContainer".position.y += 上升向量
	if $"../VScrollBar".value <= -$"../VBoxContainer".size.y + 648:
		if $"../VBoxContainer".size.y > 648:
			$"../VBoxContainer".position.y += 指数
			指数 += 0.5
		elif $"../VScrollBar".value <= -30:
			$"../VBoxContainer".position.y += 指数
			指数 += 0.5
	elif $"../VScrollBar".value >= 1:
		$"../VBoxContainer".position.y -= 指数
		指数 += 0.5
	else:
		指数 = 0
func _on_拖拽时(event: InputEventScreenDrag, 控制器: Touchpad) -> void:
	上升向量 = (event.relative.y + event.screen_velocity.y )* 0.02
func _input(event: InputEvent) -> void:
	监听输入(event)
	if has_focus() == true:
		if event is InputEventMouseButton:
			if event.button_mask == 8:
				上升向量 += 2
			elif event.button_mask == 16:
				上升向量 += -2
		
