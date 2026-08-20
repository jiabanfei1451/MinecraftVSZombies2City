extends Touchpad


func _on_拖拽时(event: InputEventScreenDrag, 控制器: Touchpad) -> void:
	控制器.position += event.relative * Vector2(sin(rad_to_deg(15)),cos(rad_to_deg(15)))
