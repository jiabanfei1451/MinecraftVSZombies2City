extends LineEdit

func _process(delta: float) -> void:
	if $"../../Music Engine".音乐选项 != int(text):
		$"../../Music Engine".音乐选项 = int(text)
		$"../../Music Engine".play()
