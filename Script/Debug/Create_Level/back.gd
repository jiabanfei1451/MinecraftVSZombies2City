extends Button

func _ready() -> void:
	pressed.connect(a)
func a():
	$"../..".valuearr = -1
	$"../..".展开 = false
