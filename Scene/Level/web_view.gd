extends WebView

func _input(event: InputEvent) -> void:
	if (event is InputEventKey):
		if (event.as_text_keycode() == "S"):
			incognito = true;
