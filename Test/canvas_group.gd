extends CanvasGroup
var d : float = 1
func _process(delta: float) -> void:
	position = Vector2(sin(d) * 20,cos(d) * 20)
	d += 0.1
