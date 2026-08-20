extends Panel
@export var type : bool = false

func _process(delta: float) -> void:
	if $Panel3/ColorRect != null:
		if type == true:
			$Panel3/ColorRect.color = Color(0.0, 1.0, 0.0, 1.0)
		else:
			$Panel3/ColorRect.color = Color(1.0, 0.0, 0.0, 1.0)


func _on_touchbutton_点击时void() -> void:
	if type == true:
		type = false
	else:
		type = true
