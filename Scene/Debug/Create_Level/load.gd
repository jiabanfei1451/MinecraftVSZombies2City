extends Button


func _on_pressed() -> void:
	var js = $"../../../JsonData"
	var d = js.load_data($"../Path".text,$"../TextEdit2".text)
	print(d.data.Level_Day)
