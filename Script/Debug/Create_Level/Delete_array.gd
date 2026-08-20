extends Button


func _on_pressed() -> void:
	$"..".queue_free()
	$"..".freel = true
	$"..".ArrayNode.arr[$"..".id].remove_at($"..".key)
