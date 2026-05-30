extends Area2D

func _process(delta: float) -> void:
	$"光源/HDR光源".energy = get_tree().current_scene.滤镜强度
