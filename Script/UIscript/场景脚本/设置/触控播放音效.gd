extends Touchbutton

func _ready() -> void:
	初始化()
	点击时void.connect(playsouds)
func playsouds():
	$"../../../../AudioStreamPlayer".play()
