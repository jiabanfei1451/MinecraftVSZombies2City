@tool
@icon("uid://dtb733fk35xis")
extends Touchbutton
## 仅有单一颜色触摸按钮
class_name TouchColorRectButton
@export var 按下时颜色 : Color = Color(0,0,0,1)
@export var 抬起时颜色 : Color = Color(1,1,1,1)
@export_group("物体状态")
@export var Rect : ColorRect

func _ready() -> void:
	R2()
func _process(delta: float) -> void:
	P3(delta)

func 按():
	Rect.color = 按下时颜色
func 抬():
	Rect.color = 抬起时颜色
func P3(delta:float):
	p2(delta)
	if Rect != null:
		Rect.position = Vector2(0,0)
		Rect.size = size
func R2():
	初始化()
	
	var colorRectnew = ColorRect.new()
	add_child(colorRectnew)
	Rect = colorRectnew
	Rect.color = 抬起时颜色
	
	按下时void.connect(按)
	抬起时void.connect(抬)
