extends TouchColorRectButton

func _ready() -> void:
	R2()
	点击时void.connect(dina)
func dina():
	$"../AudioStreamPlayer".play()
	var tween = create_tween()
	tween.tween_property($"..","modulate",Color(0.0, 0.0, 0.0, 0.0),0.5).set_trans(Tween.TRANS_SINE)
	await tween.finished
	$"../..".queue_free()
