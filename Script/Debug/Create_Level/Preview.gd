extends Label

func _process(delta: float) -> void:
	text = $"../../../Information/Name".text + " " + $"../../../Information/Day".text
