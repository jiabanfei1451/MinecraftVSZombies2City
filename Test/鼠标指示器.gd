extends Label


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	if get_tree().current_scene.使用稿子 == true:
		text = "> < 稿子"
	elif get_tree().current_scene.使用星之碎片 == true: text = "> < 星碎"
	else: text = "> <"
	custom_minimum_size.x = 0
	position = get_viewport().get_mouse_position() + Vector2(-17,-6)
func _input(event: InputEvent) -> void:
	if event.is_action_pressed("中键"):
		if visible == true:
			visible = false
		else:
			visible = true
