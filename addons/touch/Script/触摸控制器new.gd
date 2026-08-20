@tool
@icon("uid://c8ukwanv0jfm0")
## 触摸控制器的2.0版本，更好的支持罢了,都叫触摸板了，你难不成还要其他形状？
class_name TouchPad_V2
extends Control
signal 点击时(event:InputEventScreenTouch,TouchPad:TouchPad_V2)
signal 长按时(event:InputEventScreenTouch,TouchPad:TouchPad_V2)
signal 按下时(event:InputEventScreenTouch,TouchPad:TouchPad_V2)
signal 抬起时(event:InputEventScreenTouch,TouchPad:TouchPad_V2)
signal 拖拽开始时(event:InputEventScreenDrag,TouchPad:TouchPad_V2)
signal 拖拽时(event:InputEventScreenDrag,TouchPad:TouchPad_V2)
signal 拖拽结束时(event:InputEventScreenDrag,TouchPad:TouchPad_V2)
signal 点击时void()
signal 长按时void()
signal 按下时void()
signal 抬起时void()
signal 拖拽开始时void()
signal 拖拽时void()
signal 拖拽结束时void()
## 用于储存触摸点ID
var touchID : Array[int]
## 范围
@export_group("Rect")
## 检测范围大小
@export var Rect_size : Vector2 = Vector2(20,20)
## 范围矩形偏移
@export var Rect_Offset : Vector2
## 自动设置范围取值由 'Size'
@export var Auto_Set : bool = true
@export_group("Pad")
@export var 长按阈值 :float = 0.5
@export var 启用长按 :bool = false
## 是否处于拖拽状态
var Draging : bool = false
## 按下时间
var PreTime : float = 0
## 按下
var Pre : bool = 0
func _ready() -> void:
	初始化()
func _process(delta: float) -> void:
	p2(delta)
## 进行异步_Ready
func 初始化():
	pass
func _input(event: InputEvent) -> void:
	i2(event)
## 进行异步_Process
func p2(delta:float):
	if Pre == true:
		PreTime += delta
	else:
		PreTime = 0
## 进行异步_Input
func i2(event:InputEvent):
	if event is InputEventScreenTouch:
		if event.pressed:
			var bs = 计算(event)
			if bs == true:
				if touchID.has(event.index) == true:
					touchID.erase(event.index)
				touchID.append(event.index)
				emit_signal("按下时",event,$".")
				emit_signal("按下时void")
				Pre = true
		else:
			if Pre == true:
				Pre = false
				if Draging == true:
					emit_signal("拖拽结束时",event,$".")
					emit_signal("拖拽结束时void")
					Draging = false
				if Draging == false:
					if PreTime >= 长按阈值 and 启用长按 == true:
						emit_signal("长按时",event,$".")
						emit_signal("长按时void")
					else:
						var vs = 计算(event)
						if vs == true:
							emit_signal("点击时",event,$".")
							emit_signal("点击时void")
					emit_signal("抬起时",event,$".")
					emit_signal("抬起时void")
				touchID.erase(event.index)
	if event is InputEventScreenDrag:
		if touchID.has(event.index) == true:
			if Draging == false:
				Draging = true
				emit_signal("拖拽开始时",event,$".")
				emit_signal("拖拽开始时void")
			else:
				emit_signal("拖拽时",event,$".")
				emit_signal("拖拽时void")
## 计算Touch是否在范围内
func 计算(event:InputEvent):
	if event is InputEventScreenTouch or event is InputEventScreenDrag:
		var mypos : Vector2 = get_global_transform_with_canvas()[2]
		var touchpos : Vector2 = event.position
		var myscale : Vector2 = Vector2(get_global_transform_with_canvas().x.x,get_global_transform_with_canvas().y.y)
		
		var value : Vector2 = (mypos - touchpos + Rect_Offset) * -1
		var mesize : Vector2 
		if Auto_Set == true:
			mesize = size
		else:
			mesize = Rect_size
		mesize *= myscale
		if value.x <= mesize.x and value.y <= mesize.y and  value.y >= 0 and value.x >= 0:
			return true
		else:
			return false
