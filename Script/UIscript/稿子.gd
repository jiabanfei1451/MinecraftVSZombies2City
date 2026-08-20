extends Button


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	var twee = create_tween()
	twee.tween_property($".","position",Vector2(243 + (144.5*(0.5*0.8))*全局变量.卡槽数量,0),0.1)
	if get_tree().current_scene.使用稿子 == true:
		$"我是贴图".position = get_viewport().get_mouse_position() - $".".position + Vector2(-20,-20)
	else:
		$"我是贴图".position = Vector2(4.408,4.408)
	if Input.is_action_just_pressed("右键"):
		if get_tree().current_scene.使用稿子 == true:
			get_tree().current_scene.使用稿子 = false
			$"../取消".play()
func _on_pressed() -> void:
	get_tree().current_scene.稿子()
