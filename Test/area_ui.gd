extends AreaUI

func _input(event: InputEvent) -> void:
	if event is InputEventMouseMotion:
		position = event.position


func _on__ui进入时(UI: Control) -> void:
	print(UI)
