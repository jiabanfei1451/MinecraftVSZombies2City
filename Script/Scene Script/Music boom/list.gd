extends Touchbutton
var id : int

func _ready() -> void:
	初始化()
	custom_minimum_size.x = $"../..".size.x
	点击时void.connect(pr)
func _process(delta: float) -> void:
	p2(delta)
	var p : String = str(id)
	var b = $JsonData.load_data("res://2/data/Muisc Data/","Music list")
	$ColorRect/Label.text = b.data[p].名称
	
func pr():
	$"../../../../Misc String".id = id
	$"../../../../音频引擎".音乐选项 = id
