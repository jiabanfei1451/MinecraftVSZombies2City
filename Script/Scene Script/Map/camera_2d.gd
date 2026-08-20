extends Camera2D
var vec : Vector2
var zoo : float = 1
func _process(delta: float) -> void:
	create_tween().tween_property($".","offset",vec,0.2)
	create_tween().tween_property($".","zoom",Vector2(zoo,zoo),0.2)
	if zoo < 0.1:
		zoo = 0.1
