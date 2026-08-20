extends Touchbutton
var addSpeeding : bool = false
var img : Texture2D = preload("res://Image/UI/Button/MEUIV2/MEUI-pre.png")
var img2 : Texture2D = preload("res://Image/UI/Button/MEUIV2/MEUI.png")
func _ready() -> void:
	初始化()
	$"..".modulate
	点击时void.connect(加速)
	按下时void.connect(高亮)
	抬起时void.connect(取消高亮)
func 加速():
	if addSpeeding == false:
		$"../加速".play()
		$"../Panel/TextureRect".modulate = Color(0.09, 1.0, 0.0, 1.0)
		addSpeeding = true
		Engine.time_scale = 2
	else:
		$"../Panel/TextureRect".modulate = Color(1.0, 1.0, 1.0, 1.0)
		addSpeeding =false
		$"../减速".play()
		Engine.time_scale = 1
func 高亮():
	if addSpeeding == false:
		$"..".texture = img
func 取消高亮():
	if addSpeeding == false:
		$"..".texture = img2
	
func _process(delta: float) -> void:
	p2(delta)
	$"../Panel/TextureRect".position.x += delta * 10
	if $"../Panel/TextureRect".position.x >= 0:
		$"../Panel/TextureRect".position.x = -48
	if Input.is_action_just_released("F"):
		加速()
