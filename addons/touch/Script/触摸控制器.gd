@tool
@icon("uid://c8ukwanv0jfm0")
## 为手机用户添加可触摸的事件
## 	温馨提示请勿将此节点放置在非CanvasLayer的节点中否则报错自负，当前并未做地图适配
class_name Touchpad
extends ColorRect
## 短按时会触发
signal 点击时(event:InputEvent,控制器:Touchpad)
signal 点击时void
## 鼠标触碰到时进行触发 （暂时废弃）
signal 鼠标触碰时(event:InputEvent,控制器:Touchpad)
## 鼠标触碰到时进行触发 （暂时废弃）
signal 鼠标触碰时void
## 按下触发
signal 按下时(event:InputEvent,控制器:Touchpad)
signal 按下时void
## 在按钮范围内抬起后触发
signal 抬起时(event:InputEvent,控制器:Touchpad)
signal 抬起时void
## 在非按钮范围内抬起时触发
signal 外部抬起时(event:InputEvent,控制器:Touchpad)
signal 外部抬起时void
## 当计时超过长按阈值时抬起后触发
signal 长按时(event:InputEvent,控制器:Touchpad)
signal 长按时void
## 按住移动时触发
signal 拖拽时(event:InputEventScreenDrag,控制器:Touchpad)
signal 拖拽时void
## 按住移动时触发
signal 拖拽开始时(event:InputEventScreenDrag,控制器:Touchpad)
signal 拖拽开始时void
## 拖拽抬起后触发
signal 拖拽抬起时(event:InputEventScreenDrag,控制器:Touchpad)
signal 拖拽抬起时void
@export_group("focus")
@export_range(0,2,1) var 触摸优先级 : int = 2
@export_group("触摸板属性")
@export var 范围 : Vector2
@export var 偏移offect : Vector2
## 开启后自动以 "size" 属性作为范围
@export var 自动设置 : bool = true
@export var 按下状态 : bool = false
@export var 拖拽状态 : bool = false
@export var 短按阈值 : float = 0.2
@export var 长按阈值 : float = 1
## 决定了按钮的触发模式
@export var 启用 : bool = true
@export_enum("普通:0","可串流:1") var 触摸模式 : int
@export_group("DEBUG")
@export var DEBUGColor : Color = Color(0.0, 0.302, 1.0, 0.4) 
@export var DEBUG : bool = false
var Rect : ReferenceRect
var po = []
var 计时 : bool = false
var 计时_ : float = 0
var pos : Array
var 真实范围 : Vector2
var 指点id : float
func _ready() -> void:
	初始化()
func _input(event: InputEvent) -> void:
	监听输入(event)
func _DEBUG_Touchpad_按下时(event: InputEvent) -> void:
	计时 = true
func _process(delta: float) -> void:
	p2(delta)
func _DEBUG_Touchpad_抬起时(event: InputEvent) -> void:
	计时 = false
	if 计时_ <= 短按阈值:
		emit_signal("点击时",event,$".")
		emit_signal("点击时void")
	elif 计时_ >= 长按阈值:
		emit_signal("长按时",event)
		emit_signal("长按时void",$".")
func 计算相对坐标():
	var 坐标 : Array[Vector2]
	for i in po:
		if not i is CanvasLayer:
			坐标.append(i.position / i.scale)
	var 最终坐标值:Vector2 = position
	for i in 坐标:
		最终坐标值 += i
	return 最终坐标值
func 计算相对缩放():
	var 坐标 : Array[Vector2]
	for i in po:
		if not i is CanvasLayer:
			坐标.append(i.scale)
	var 最终缩放值:Vector2 = scale
	for i in 坐标:
		最终缩放值 *= i
	return 最终缩放值
func _exit_tree() -> void:
	if get_node("Rect") != null:
		get_node("Rect").queue_free()
func 初始化():
	focus_exited.connect(_focus_exited)
	focus_mode = 触摸优先级
	color = DEBUGColor
	按下时.connect(_DEBUG_Touchpad_按下时)
	抬起时.connect(_DEBUG_Touchpad_抬起时)
	var ds = $"."
	while not ds is CanvasLayer:
		ds = ds.get_node("..")
		po.append(ds)
		await get_tree().create_timer(0.02).timeout
	if get_node("Rect") != null:
		get_node("Rect").queue_free()

	var new = ReferenceRect.new()
	Rect = new
	ds.add_child(new)
	Rect.name = "Rect"
func p2(delta:float):
	if Rect != null:
		if DEBUG == true:
			Rect.border_color = DEBUGColor
		else:
			Rect.border_color = Color(0,0,0,0)
	if 自动设置 == true:
		范围 = size
	if 计时 == true:
		计时_ += delta
	else:
		计时_ = 0
	if Rect != null:
		Rect.position = 计算相对坐标() * 计算相对缩放() + 偏移offect
		Rect.size = 范围 * 计算相对缩放()
func 监听输入(event: InputEvent) -> void:
	if get_node("..").visible == true:
		if 启用 == true:
			if 触摸模式 == 0:
				if event is InputEventScreenDrag or event is InputEventScreenTouch:
					var 判定范围 : Vector2 = 范围 * 计算相对缩放()
					var 可判定 : bool = false
					var posi : Vector2 = position
					var touchpos : Vector2
					if event is InputEventScreenTouch:
						var touch : InputEventScreenTouch = event
						touchpos = touch.position

					if event is InputEventScreenDrag:
						var touch : InputEventScreenDrag = event
						touchpos = touch.position
						if 按下状态 == true:
							if event.index == 指点id:
								emit_signal("拖拽时void")
								emit_signal("拖拽时",event,$".")
							if 拖拽状态 == false:
								emit_signal("拖拽开始时",event,$".")
								emit_signal("拖拽开始时void")
								指点id = event.index
								拖拽状态 = true
					posi = 计算相对坐标() * 计算相对缩放() + 偏移offect - touchpos
					var posx : float = posi.x
					var posy : float = posi.y
					if posx <= 0 and posx >= -判定范围.x:
						if posy <= 0 and posy >= -判定范围.y:
							if event is InputEventScreenTouch:
								if event.is_pressed():
									if get_focus_mode_with_override() != 0:
										grab_focus(true)
									emit_signal("按下时",event,$".")
									emit_signal("按下时void")
									按下状态 = true
								if event.is_released():
									emit_signal("抬起时",event,$".")
									emit_signal("抬起时void")
									按下状态 = false
					if event is InputEventScreenTouch:
						if 按下状态 == true:
							if event.is_released():
								emit_signal("外部抬起时",event,$".")
								emit_signal("外部抬起时void")
								按下状态 = false
					if 拖拽状态 == true:
						if event is InputEventScreenTouch:
							if event.is_released():
								emit_signal("拖拽抬起时",event,$".")
								emit_signal("拖拽抬起时void")
								拖拽状态 = false
					真实范围 = 判定范围
			if 触摸模式 == 1:
				var 判定范围 : Vector2 = 范围 * 计算相对缩放()
				var posi : Vector2
				var posx : float
				var posy : float
				var touchpos : Vector2
				if event is InputEventScreenTouch or event is InputEventMouseButton or (拖拽状态 == true and event is InputEventScreenDrag) or (拖拽状态 == true and event is InputEventMouseMotion):
					if event is InputEventScreenTouch:
						var touch : InputEventScreenTouch = event
						touchpos  = touch.position
						拖拽状态 = touch.is_pressed()
					if event is InputEventMouseButton:
						var touch : InputEventMouseButton = event
						touchpos = touch.position
						拖拽状态 = touch.is_pressed() == true
					if event is InputEventMouseMotion:
						var touch : InputEventMouseMotion = event
						touchpos = touch.position
				posi = (计算相对坐标() + 偏移offect) - touchpos
				posx = posi.x
				posy = posi.y
				if posx <= 0 and posx >= -判定范围.x:
					if posy <= 0 and posx >= -判定范围.y:
						if 拖拽状态 == true:
							if 按下状态 == false:
								按下状态 = true
								grab_focus(true)
								emit_signal("按下时",event,$".")
								emit_signal("按下时void")
						else:
							emit_signal("抬起时",event,$".")
							emit_signal("抬起时void")
							按下状态 = false
				真实范围 = 判定范围
func _focus_exited():
	按下状态 = false
	拖拽状态 = false
