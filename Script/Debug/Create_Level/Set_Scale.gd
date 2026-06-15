extends HSlider

func _process(delta: float) -> void:
	for i in $"../ScrollContainer/VBoxContainer".get_children():
		i.scale.x = value
		i.scale.y = value
