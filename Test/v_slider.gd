extends VSlider
func _process(delta: float) -> void:
	$"../CanvasLayer/touchBar".rotation = value
	$"../CanvasLayer/touchBar".乘数校准 = Vector2($"../VSlider2".value,$"../VSlider3".value)
