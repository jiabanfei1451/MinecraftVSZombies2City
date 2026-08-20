extends Touchbutton
var image : Texture2D = preload("res://Image/UI/Button/MEUIV2/MEUI-play.png")
var image2 : Texture2D = preload("res://Image/UI/Button/MEUIV2/MEUI-play-prepng.png")
var image3 : Texture2D = preload("res://Image/UI/Button/MEUIV2/MEUI-stop.png")
var image4 : Texture2D = preload("res://Image/UI/Button/MEUIV2/MEUI-stop-pre.png")
var a : bool = false
var time : float
var id : int
func _ready() -> void:
	初始化()
	按下时void.connect(p)
	抬起时void.connect(r)
	点击时void.connect(d)
func _process(delta: float) -> void:
	p2(delta)
	if a == false:
		if $"../../音频引擎".playing == false:
			$TextureRect.texture = image
		else:
			$TextureRect.texture = image3
	else:
		if $"../../音频引擎".playing == false:
			$TextureRect.texture = image2
		else:
			$TextureRect.texture = image4
	if Input.is_action_pressed("空格"):
		a = true
	if Input.is_action_just_released("空格"):
		d()
		a = false
func d():
	a = false
	if $"../../音频引擎".自动播放 == true:
		time = $"../../音频引擎".get_playback_position()
		$"../../音频引擎".自动播放 = false
		$"../../音频引擎".playing = false
	else:
		$"../../音频引擎".自动播放 = true
		if id == $"../../音频引擎".音乐选项:
			$"../../音频引擎".play(time)
		else:
			id = $"../../音频引擎".音乐选项
			$"../../音频引擎".play(0)
func r():
	a = false
func p():
	a = true
