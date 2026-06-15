@tool
extends Node2D
signal 加载关卡时

var text : Texture2D = preload("uid://bfwevmciduc6p")
@export var 关卡路径 : String
@export var ID : int
@export var 显示 : String = "1"
@export var color : Color = Color(0.126, 0.84, 0.0, 1.0)
var Y : float = 0
var a : AtlasTexture = AtlasTexture.new()
var a2 : AtlasTexture = AtlasTexture.new()
var a3 : AtlasTexture = AtlasTexture.new()
var r : Rect2 = Rect2(48,46,48,16)
var r2 : Rect2 = Rect2(48,6,48,16)
var r3 : Rect2 = Rect2(51,23,42,22)

func _ready() -> void:
	$Touchbutton.点击时void.connect(djishi)
	$Touchbutton.按下时void.connect(pr)
	$Touchbutton.抬起时void.connect(re)
func _process(delta: float) -> void:
	$Label.text = 显示
	$Button2/Button.self_modulate = color
	a.atlas = text
	a2.atlas = text
	a3.atlas = text
	a.region = r
	a2.region = r2
	a3.region = r3
	r.position.x = 0 + 48 * ID + 2 * (ID - 1)
	r2.position.x = 0 + 48 * ID + 2 * (ID - 1)
	r3.position.x = 4 + 51 * ID + (-1.75 * ID)
	$"边框".texture = a
	$"背景".texture = a2
	$"底座".texture = a3
	create_tween().tween_property($Label,"position:y",Y + -25,0.3)
	create_tween().tween_property($Button2/Button,"position:y",Y,0.1)

func djishi():
	$AudioStreamPlayer.play()
	emit_signal("加载关卡时")
func pr():
	Y = 7
func re():
	Y = 0
