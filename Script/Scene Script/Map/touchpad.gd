extends TouchPad_V2


func _on_拖拽时(event: InputEventScreenDrag, 控制器: TouchPad_V2) -> void:
	print(event.index)
	$"../../Camera2D".vec -= event.relative / $"../../Camera2D".zoo

func _process(delta: float) -> void:
	if Input.is_action_just_pressed("中键上"):
		$"../../Camera2D".zoo += 0.05 * $"../../Camera2D".zoo
	if Input.is_action_just_pressed("中键下"):
		$"../../Camera2D".zoo -= 0.05 * $"../../Camera2D".zoo
