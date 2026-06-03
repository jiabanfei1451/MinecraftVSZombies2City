extends Touchbutton
var addSpeeding : bool = false
var img : Texture2D = preload("res://UI/Button/Meui/UpspeedButton-pre.png")
var img2 : Texture2D = preload("res://UI/Button/Meui/UpspeedButton.png")
var img3 : Texture2D = preload("res://UI/Button/Meui/UpspeedButton-true-notpre.png") 
var img4 : Texture2D = preload("res://UI/Button/Meui/UpspeedButton-true.png")
func _ready() -> void:
	初始化()
	$"..".modulate
	点击时void.connect(加速)
	按下时void.connect(高亮)
	抬起时void.connect(取消高亮)
func 加速():
	if addSpeeding == false:
		$"../加速".play()
		addSpeeding = true
		Engine.time_scale = 2
	else:
		addSpeeding =false
		$"../减速".play()
		Engine.time_scale = 1
func 高亮():
	if addSpeeding == false:
		$"..".texture = img
	else:
		$"..".texture = img4
func 取消高亮():
	if addSpeeding == false:
		$"..".texture = img2
	else:
		$"..".texture = img3
	
