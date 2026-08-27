extends Area2D

func _process(delta: float) -> void:
	rotation = get_global_mouse_position().angle_to_point(position)
	position += Vector2(1,1).from_angle(3.14 / 180 * 0) 
	print(rotation)
	var s : Array[String]
	
