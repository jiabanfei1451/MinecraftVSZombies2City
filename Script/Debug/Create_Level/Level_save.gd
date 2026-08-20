extends Button



func _on_pressed() -> void:
	var js = $"../../../JsonData"
	js.add_Data("Level_name",[$"../../Information/Name".text])
	js.add_Data("Level_Day",[$"../../Information/Day".text])
	js.add_Data("Level_BGM",[int($"../../Information/BGMID".text)])
	js.add_Data("Level_monster",[$"../../Menu/Monster Array/Array".arr])
	js.add_Data("Level_monster_Value",[$"../../Menu/Monster Array/Array".MonsterValue])
	js.add_Data("Level_Wave",[$"../../Menu/Monster Array/Array".Wave])
	js.add_Data("Level_image",[$"../../Menu/Image/ScrollContainer/VBoxContainer/LineEdit".text])
	js.add_Data("Level_create_version",[ProjectSettings.get_setting("application/config/version")])
	js.save_json($"../Path".text,$"../TextEdit2".text)
