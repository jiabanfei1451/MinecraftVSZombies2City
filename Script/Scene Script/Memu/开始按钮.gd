extends Touchbutton

func _process(delta: float) -> void:
	p2(delta)
	if get_tree().current_scene.get_child_count() > 1:
		启用 = false
	else:
		启用 = true

func _on_按下时void() -> void:
	发白()


func _on_抬起时void() -> void:
	发回去()


func _on_点击时void() -> void:
	发回去()
	var s : PackedScene = preload("uid://cgl575a7m4dah")
	var si = s.instantiate()
	get_tree().change_scene_to_node(si)
func _on_mouse_entered() -> void:
	发白()


func _on_mouse_exited() -> void:
	发回去()
func 发白():
	$"..".modulate = Color(1.825, 1.825, 1.825, 1.0)
func 发回去():
	$"..".modulate = Color(1.0, 1.0, 1.0, 1.0)
