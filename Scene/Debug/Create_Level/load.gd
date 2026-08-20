extends Button


func _on_pressed() -> void:
	var js = $"../../../JsonData"
	var d = js.load_data($"../Path".text,$"../TextEdit2".text)
	if d != null:
		$"../../Information/Day".text = d.data.Level_Day
		$"../../Information/Name".text = d.data.Level_name
		$"../../Information/Version".text = d.data.Level_create_version
		$"../../Information/BGMID".text = str(d.data.Level_BGM)
		$"../../Menu/Monster Array/Array".arr.clear()
		$"../../Menu/Monster Array/Array".MonsterValue.clear()
		var s = d.data.Level_monster
		for i in $"../../Menu/Monster Array/Array/ScrollContainer/VBoxContainer".get_children():
			i.queue_free()
		if d.data.Level_monster != null:
			for i in d.data.Level_monster.size():
				$"../../Menu/Monster Array/Add_array".emit_signal("pressed")
				if s[i].size() != -1:
					$"../../Menu/Monster Array/Array".arr.back().append_array(s[i])
		await get_tree().create_timer(get_process_delta_time() * 3).timeout
		var p = d.data.Level_monster_Value
		var o : int = 0
		if d.data.Level_monster_Value != null:
			for i in $"../../Menu/Monster Value/ScrollContainer/VBoxContainer".get_children():
				i.get_child(1).text = str(p[o])
				o += 1
		await get_tree().create_timer(get_process_delta_time() * 3).timeout
		p = d.data.Level_Wave
		o = 0
		if d.data.Level_Wave != null:
			for i in $"../../Menu/Wave/ScrollContainer/VBoxContainer".get_children():
				i.get_child(1).b = p[o]
				o += 1
