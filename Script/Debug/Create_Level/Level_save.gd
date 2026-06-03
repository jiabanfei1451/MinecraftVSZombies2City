extends Button



func _on_pressed() -> void:
	var js = $"../../../JsonData"
	js.add_Data("Level_name",[$"../../Information/Name".text])
	js.add_Data("Level_Day",[$"../../Information/Day".text])
	js.add_Data("Level_monster",[$"../../Menu/Monster Array/Array".arr])
	js.add_Data("Level create version",[初始化.版本])
	js.save_json($"../Path".text,$"../TextEdit2".text)
