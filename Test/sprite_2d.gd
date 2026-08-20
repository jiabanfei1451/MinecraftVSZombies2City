extends Sprite2D

func _input(event: InputEvent) -> void:
	if event is InputEventScreenTouch:
		while get_global_transform_with_canvas()[2].x >= 0:
			position.x -= 1
		while get_global_transform_with_canvas()[2].y >= 0:
			position.y -= 1
		position += Vector2(randf_range(0,100),randf_range(0,100))
		
