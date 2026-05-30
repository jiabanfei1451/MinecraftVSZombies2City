extends Button


func _on_pressed() -> void:
	$AudioStreamPlayer.play()
	text = "场景加载中"
	var 加载 = load("res://Scene/序章.tscn")
	await 加载
	$"../AnimationPlayer".play("透明")
	await get_tree().create_timer(1.3).timeout
	get_tree().change_scene_to_packed(加载)
