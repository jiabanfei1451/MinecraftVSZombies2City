extends Touchbutton
var id : int

func _ready() -> void:
	初始化()
	点击时void.connect(pr)
	按下时void.connect(a)
	抬起时void.connect(r)
	var p : String = str(id)
	var b = $JsonData.res_load_data("res://2/data/Muisc Data/","Music list")
	$ColorRect/Label.text = b.data[p].名称
func _process(delta: float) -> void:
	p2(delta)
	custom_minimum_size.x = $"../..".size.x
	
func pr():
	$ColorRect.color = Color(1.0, 1.0, 1.0, 0.196)
	$"../../../../../Muisc/Misc String".id = id
	MuiscEngine.音乐.音乐选项 = id
	var B = create_tween()
	B.tween_property($"../../../..","scale:x",0,0.75).set_trans(Tween.TRANS_QUART)
	B.parallel().tween_property($"../../../../../Muisc","scale:x",1,0.5).set_trans(Tween.TRANS_QUART)
func r():
	$ColorRect.color = Color(1.0, 1.0, 1.0, 0.196)
func a():
	$ColorRect.color = Color(1.0, 1.0, 1.0, 0.392)
