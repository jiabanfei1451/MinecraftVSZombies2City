@tool
extends TouchPad_V2
## 适用于渲染纹理的触摸按钮
class_name For_Touch_Button
@export_group("Object_theme")
## 纹理物体
@export var panel : Panel = null
## 纹理的主题
@export var panel_Theme : Theme = preload("uid://crj7nbt82u637")
@export_group("Theme_image")
## 纹理当前使用的主题（无法被外部脚本更改）
@export var ObjTheme : Theme
## 主题外貌（无法被外部脚本更改）
@export var Mouse_Foucs : Variant
## 主题外貌（无法被外部脚本更改）
@export var is_Pressed : Variant
## 主题外貌（无法被外部脚本更改）
@export var not_Pressed : Variant
## 纹理当前使用的主题外貌（无法被外部脚本更改）
@export var S : Variant
func _ready() -> void:
	for i in get_children():
		if i.name == "MePanel":
			i.queue_free()
	var P = Panel.new()
	add_child(P)
	var Th = Theme.new()
	Th.add_type("Panel")
	P.theme = Th
	ObjTheme = Th
	P.name = "MePanel"
	panel = P
func _process(delta: float) -> void:
	p3(delta)
## 进行第3次归递循环而已
func p3(delta:float):
	p2(delta)
	if !Auto_Set:
		panel.position = Rect_Offset
		panel.size = Rect_size
	else:
		panel.size = size
		panel.position = Vector2(0,0)
	if panel_Theme != null:
		if Mouse_Foucs == null:
			Mouse_Foucs = panel_Theme.get_stylebox("Mouse_Foucs","TouchPad_V2")
			is_Pressed = panel_Theme.get_stylebox("is_Pressed","TouchPad_V2")
			not_Pressed = panel_Theme.get_stylebox("not_Pressed","TouchPad_V2")
	if Pre == true:
		ObjTheme.set_stylebox("panel","Panel",is_Pressed)
	else:
		ObjTheme.set_stylebox("panel","Panel",not_Pressed)
	S = panel.theme.get_stylebox("panel","Panel")
