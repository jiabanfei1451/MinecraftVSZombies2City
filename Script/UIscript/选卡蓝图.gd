extends Control
var 空卡槽贴图 : Texture2D = load("res://Image/UI/Card/card(null).png")
func _process(delta: float) -> void:
	设定贴图()

func 设定贴图():
	$"图片".visible = false
	$"贴图/边框".visible = false
	$"贴图/背景板".texture = 空卡槽贴图
