extends Touchpad
signal _点击(button:Touchpad)
func _ready() -> void:
	抬起时void.connect(_on_pressed)
func _on_pressed() -> void:
	emit_signal("点击时",$".")
