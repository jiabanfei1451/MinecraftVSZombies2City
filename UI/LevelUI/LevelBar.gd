extends TextureProgressBar


func _process(delta: float) -> void:
	$"../../..".position .x = $"../../../../X".position.x - $"../../..".size.x
	var t = create_tween()
	max_value = get_tree().current_scene.最大波数
	t.tween_property($".","value",get_tree().current_scene.当前波数,0.2)
func _ready() -> void:
	while true:
		var ds = create_tween().tween_property($"../../..","size:x",250,2).set_trans(Tween.TRANS_SINE)
		await ds.finished
		ds = create_tween().tween_property($"../../..","size:x",244,2).set_trans(Tween.TRANS_SINE)
		await ds.finished
