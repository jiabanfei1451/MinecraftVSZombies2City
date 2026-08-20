extends Button

func _ready() -> void:
	pressed.connect(a)
func a():
	$"../..".索引 = -1
	$"../..".valuearr = -1
	$"../..".展开 = false
	for i in $"../ScrollContainer/VBoxContainer".get_children():
		i.queue_free()
