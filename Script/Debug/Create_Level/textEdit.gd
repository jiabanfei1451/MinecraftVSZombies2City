extends LineEdit
var ed : bool

func _on_text_changed(new_text: String) -> void:
	$"..".ArrayNode[$"..".id][$"..".key] = int(new_text)
	ed = false


func _on_text_submitted(new_text: String) -> void:
	ed = true
