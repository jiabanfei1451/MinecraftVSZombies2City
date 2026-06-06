extends Button
var b : bool = false

func _ready() -> void:
	pressed.connect(s)
func s():
	if b == false:
		b = true
	else:
		b = false
