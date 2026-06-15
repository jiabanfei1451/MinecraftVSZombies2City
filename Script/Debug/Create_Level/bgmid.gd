extends LineEdit

func _process(delta: float) -> void:
	if MuiscEngine.音乐.音乐选项 != int(text):
		MuiscEngine.音乐.音乐选项 = int(text)
		MuiscEngine.音乐.play()
