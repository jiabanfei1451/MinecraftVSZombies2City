extends Touchbutton

func _ready() -> void:
	初始化()
	点击时void.connect(Back)
func Back():
	var B = create_tween()
	B.tween_property($"../../..","scale:x",0,0.75).set_trans(Tween.TRANS_QUART)
	B.parallel().tween_property($"../../../../Muisc list","scale:x",1,0.5).set_trans(Tween.TRANS_QUART)
