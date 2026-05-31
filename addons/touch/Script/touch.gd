@tool
@icon("uid://dittpp2ukt1lg")
extends Control
## 适用于可多指触控设备点击操作按钮，目前可以在地图中放置使用（大概吧？）
class_name Touchbutton
## 用于快捷创建Ready信号的替代
signal createReady()
## 用于快速创建Process信号替代
signal CreateProcess(delta:float)
signal 按下时(name:String)
signal 抬起时(name:String)
signal 点击时(name:String)
signal 长按时(name:String)
signal 拖拽开始时(event:InputEventScreenDrag,控制器:Touchbutton)
signal 拖拽时(event:InputEventScreenDrag,控制器:Touchbutton)
signal 拖拽抬起时(控制器:Touchbutton)
signal 按下时void
signal 抬起时void
signal 点击时void
signal 长按时void
signal 拖拽开始时void
signal 拖拽时void
signal 拖拽抬起时void
@export_group("focus")
@export_range(0,2,1) var 触摸优先级 : int = 2
@export_group("TouchButton")
@export var Shape : Shape2D
@export var 偏移 : Vector2
@export var 自动设置 : bool = true
@export var 按下时给予焦点给 : Control
@export var Parent_Object : Control
@export_enum("YX:0","X:1","Y:2") var 自动偏移模式 : int
@export var 短按阈值 : float = 0.2
@export var 长按阈值 : float = 1
@export var 启用 : bool = true
@export_subgroup("Debug")
@export var Debug : bool = false
@export_enum("矩形:0","圆形:1","圆形取Y:2","椭圆:3") var 自动设置形状 : int = 0
@export_group("Texture")
@export var notTexture : bool = false
@export var 按下纹理 : StyleBoxTexture = preload("uid://c4ffe4u1spmqk")
@export var 抬起纹理 : StyleBoxTexture = preload("uid://ciadyrsksiodo")
@export_subgroup("Object State")
@export var 纹理物体 : Panel
@export var 物体纹理 : StyleBoxTexture
@export var object_array : Array
var pre : bool = false
var Drag : bool = false
var touchid : float = -1
var pretime : float = 0
var 触摸控制器 : TouchScreenButton
func _ready() -> void:
	初始化()
	
func _process(delta: float) -> void:
	p2(delta)
func 按下():
	if get_node("..").visible == true:
		if 启用 == true:
			pre = true
			if Debug == true:
				print(2)
			if get_focus_mode_with_override() != 0:
				if 按下时给予焦点给 == null:
					grab_focus(true)
				else:
					按下时给予焦点给.grab_focus(true)
			emit_signal("按下时",name)
			emit_signal("按下时void")
			纹理物体.add_theme_stylebox_override("panel",按下纹理)
func 抬起():
	if get_node("..").visible == true:
		if 启用 == true:
			if Debug == true:
				print(1)
			pre = false
			if pretime <= 短按阈值:
				emit_signal("点击时",name)
				emit_signal("点击时void")
			else:
				if pretime >= 长按阈值:
					emit_signal("长按时",name)
					emit_signal("长按时void")
			if Drag == true:
				emit_signal("拖拽抬起时void")
				emit_signal("拖拽抬起时",$".")
			emit_signal("抬起时",name)
			emit_signal("抬起时void")
			纹理物体.add_theme_stylebox_override("panel",抬起纹理)
func 清空数组物体():
	for i in object_array:
		if i != null:
			if i is Node:
				i.queue_free()
	object_array.clear()

func 初始化():
	清空数组物体()
	focus_mode = 触摸优先级
	var 生成触摸 = TouchScreenButton.new()
	add_child(生成触摸)
	生成触摸.position = Vector2(0,0)
	object_array.append(生成触摸)
	生成触摸.pressed.connect(按下)
	生成触摸.released.connect(抬起)
	触摸控制器 = 生成触摸
	
	var 生成纹理物体 = Panel.new()
	object_array.append(生成纹理物体)
	生成纹理物体.add_theme_stylebox_override("panel",抬起纹理)
	add_child(生成纹理物体)
	纹理物体 = 生成纹理物体
func p2(delta:float):
	var 形状
	if pre == true:
		pretime += delta
	else:
		pretime = 0
	if 纹理物体 != null:
		纹理物体.size = size
		if notTexture != true:
			物体纹理 = 纹理物体.get_theme_stylebox("panel")
		if notTexture == true:
			纹理物体.visible = false
		else:
			纹理物体.visible = true
	if 自动设置 == true:
		if 自动设置形状 == 0:
			形状 = RectangleShape2D.new()
			if 自动偏移模式 == 0:
				形状.size = size
			if 自动偏移模式 == 1:
				形状.size.x = size.x
			if 自动偏移模式 == 2:
				形状.size.y = size.y
		elif 自动设置形状 == 1:
			形状 = CircleShape2D.new()
			形状.radius = size.x
		elif 自动设置形状 == 2:
			形状= CircleShape2D.new()
			形状.radius= size.y
		elif 自动设置形状 == 3:
			形状= CapsuleShape2D.new()
			形状.radius= size.x
			形状.height= size.y
		if 触摸控制器 != null:
			触摸控制器.shape = 形状
			if 自动偏移模式 == 0:
				偏移 = size / 2
			if 自动偏移模式 == 1:
				偏移.x = size.x / 2
			if 自动偏移模式 == 2:
				偏移.y = size.y / 2
			触摸控制器.position = 偏移
	else:
		触摸控制器.position = 偏移
		触摸控制器.shape = Shape
func _input(event: InputEvent) -> void:
	输入监听(event)
func 输入监听(event: InputEvent) -> void:
	if pre == true:
		if event is InputEventScreenDrag:
			if Drag == false:
				emit_signal("拖拽开始时void")
				emit_signal("拖拽开始时",event,$".")
			else:
				emit_signal("拖拽时void")
				emit_signal("拖拽时",event,$".")
