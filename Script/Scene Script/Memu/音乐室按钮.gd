extends Touchbutton

func _process(delta: float) -> void:
	p2(delta)
	if get_tree().current_scene.get_child_count() > 2:
		启用 = false
	else:
		启用 = true
	
func _on_按下时void() -> void:
	发白()


func _on_抬起时void() -> void:
	发回去()


func _on_点击时void() -> void:
	发回去()
	print(0)
	var s : PackedScene = preload("uid://ba3fg7v706epj")
	get_tree().change_scene_to_packed(s)
func _on_mouse_entered() -> void:
	发白()


func _on_mouse_exited() -> void:
	发回去()
func 发白():
	$"..".modulate = Color(1.825, 1.825, 1.825, 1.0)
func 发回去():
	$"..".modulate = Color(1.0, 1.0, 1.0, 1.0)
