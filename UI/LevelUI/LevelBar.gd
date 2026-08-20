extends TextureProgressBar


func _process(delta: float) -> void:
	var t = create_tween()
	max_value = get_tree().current_scene.最大波数
	t.tween_property($".","value",get_tree().current_scene.当前波数,0.2)
		
