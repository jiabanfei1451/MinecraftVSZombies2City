extends Button

func _ready() -> void:
	if get_tree().current_scene.当前状态 == "选卡":
		mouse_filter = 2
		mouse_default_cursor_shape = 0
	else:
		pass

func _on_pressed() -> void:
	if get_tree().current_scene.使用器械能 == false:
		get_tree().current_scene.使用器械能 = true
	else:
		get_tree().current_scene.使用器械能 = false
