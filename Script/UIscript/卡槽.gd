extends Control
@export_group("贴图")
@export var 提示文字 : Label
var 空卡槽贴图 : Texture2D
var 卡槽默认贴图 : Texture2D
var 卡槽边框默认贴图 : Texture2D
@export var 贴图组 : int = 0
@export_group("卡槽状态")
@export var 启用 : bool = true
var 器械 : PackedScene
@export var 器械ID : int = -1
var 当前ID : int = -1
@export var 消耗 : int = 50
@export var 冷却 : float = 7.5
@export_range(0,100) var 冷却减免 : float = 0
@export var 冷却中 : bool = false
@export var UI:CanvasLayer
@export_subgroup("选卡蓝图")
@export var 调用功能 : int = 0
@export_group("选用状态")
@export var 选用状态 : bool = false
@export_group("绑定快捷键")
@export_enum("1","2","3","4","5","6","7","8","9","0") var 绑定快捷键 : String = "1"
@export_group("其他")
@export_enum("全局坐标:0","局部坐标:1") var 缓动类型 : int = 1
@export var 缓动帧时长 : float = 30
@export var 描述生成偏移 :Vector2
@export var 附加属性 : Array
@export var 附加属性名称 : Array[String]
@export var pos : Vector2
var shushu : bool = false
var 冷却渐变 : Tween
var 描述 : Control
var 选卡_ : bool = false
var 设备ID : int = 1

func _ready() -> void:
	#region 初始化
	if OS.get_name() == "Windows":
		设备ID = 0
	var ttz = 精灵图列表.返回贴图组(贴图组,设备ID)
	空卡槽贴图 = ttz[0]
	卡槽默认贴图 = ttz[1]
	卡槽边框默认贴图 = ttz[2]
	await get_tree().create_timer(get_viewport().get_physics_process_delta_time()).timeout
	添加属性()
	设定卡槽状态()
	#endregion 
	if 调用功能 == 0:
		if get_tree().current_scene.当前状态 != "选卡":
			开始冷却(冷却减免)
	visible=true
func _physics_process(delta: float) -> void:
	if 缓动类型 == 1:
		create_tween().tween_property($".","position",pos,delta * 缓动帧时长)
	else:
		create_tween().tween_property($".","global_position",pos,delta * 缓动帧时长)
func _process(delta: float) -> void:
	#region 通用
	设定鼠标状态()
	设定卡槽状态()
	var jjj = int(绑定快捷键)
	if jjj == 0:
		jjj = 10
	if jjj > 全局变量.卡槽数量:
		visible = false
	else:
		visible = true
	#endregion
	if 调用功能 == 0:
		添加属性()
		if 启用 == true and 器械ID > -1:
			if get_tree().current_scene.当前器械 == 器械 and get_tree().current_scene.来自 == $".": #定义器械选定状态
				modulate = Color(0.735, 0.735, 0.735, 1.0)
			else:
				modulate = Color(1.0, 1.0, 1.0, 1.0)
			if 判定选卡状态() == false:
				if Input.is_action_just_released("右键"): #如果器械选中时右键取消
					取消选定()
					
				if Input.is_action_just_released(绑定快捷键):
					被选中()

				if get_tree().current_scene.器械能 < 消耗:
					modulate = Color(0.5, 0.5, 0.5, 1.0)
				else:
					if get_tree().current_scene.当前器械 != 器械 and get_tree().current_scene.来自 != $".":
						modulate = Color(1.0, 1.0, 1.0, 1.0)
				if get_tree().current_scene.正在使用其他属性 == true:
					选用状态 = false
			$"裁剪节点".visible = 冷却中
	elif 调用功能 == 1:
		if get_tree().current_scene.来源.has($"."): #定义器械选定状态
			modulate = Color(0.735, 0.735, 0.735, 1.0)
		else:
			modulate = Color(1.0, 1.0, 1.0, 1.0)
		$"裁剪节点".visible = false
#region 互动类
func _on_pressed() -> void: ## 点击时
	if 调用功能 == 0:
		if 判定选卡状态() == false:
			被选中()
	elif 调用功能 == 1:
		if get_tree().current_scene.来源.has($".") == false:
			选卡()

func _on_button_down() -> void: ## 按下时
	if 调用功能 == 0:
		if 判定选卡状态() == false:
			if 启用 == true:
				modulate = Color(1.825, 1.825, 1.825, 1.0)

func _on_button_up() -> void: ## 抬起时
	if 调用功能 == 0:
		if 判定选卡状态() == false:
			if 启用 == true:
				modulate = Color(1.0, 1.0, 1.0, 1.0)
	
func _鼠标进入时() -> void:  ## 这里后面可能会改成触摸的判定
	if 器械ID != -1:
		创建描述()
		var d = 创建文本(32,Color(0,0,0,1),1)
		d.text = 精灵图列表.Card_名称[器械ID]
		描述.get_node("排列节点").add_child(d)
		d = 创建文本(24,Color(0,0,0,1),0)
		d.text = 精灵图列表.Card_描述[器械ID]
		描述.get_node("排列节点").add_child(d)
		if 调用功能 == 0:
			if get_tree().current_scene.器械能 < 消耗:
				d = 创建文本(24,Color(1.0, 0.0, 0.0, 1.0),0)
				d.text = "(器械能不足)"
				描述.get_node("排列节点").add_child(d)
			if 冷却中 == true:
				d = 创建文本(24,Color(1.0, 0.0, 0.0, 1.0),0)
				d.text = "(冷却中)"
				描述.get_node("排列节点").add_child(d)
	
func _鼠标离开时() -> void: ## 一样
	if 描述 != null:
		描述.fr()
#endregion
#region 卡槽互动
func 开始冷却(减免:float):
	冷却中 = true
	$"裁剪节点/冷却".scale.y = 1
	var cd = create_tween()
	cd.tween_property($"裁剪节点/冷却","scale",Vector2(1,0),冷却 * (1 - 冷却减免 / 100))
	冷却渐变 = cd
	await 冷却渐变.finished
	冷却中 = false

func 被选中():
	if 启用 == true:
		if get_tree().current_scene.器械能 >= 消耗:
			if get_tree().current_scene.当前器械 != 器械:
				if 冷却中 == false:
					选定()
			else:
				取消选定()
		else:
			if 提示文字 != null:
				for d in 3:
					var sd = create_tween()
					sd.tween_property(提示文字,"theme_override_colors/font_color",Color(1.0, 0.0, 0.0, 1.0),0)
					get_tree().create_timer(0.1).timeout
					sd.tween_property(提示文字,"theme_override_colors/font_color",Color(0.0, 0.0, 0.0, 1.0),0)
					get_tree().create_timer(0.1).timeout

func 选定():
	if 启用 == true:
		get_tree().current_scene.当前器械ID = 器械ID
		get_tree().current_scene.禁用()
		get_tree().current_scene.来自 = $"."
		get_tree().current_scene.当前器械 = 器械
		$"选用".play()
	
func 取消选定():
	if 启用 == true:
		if 器械ID != -1:
			if get_tree().current_scene.当前器械 == 器械:
				$"取消".play()
				get_tree().current_scene.当前器械 = null

func 设定卡槽状态():
	var ttz = 精灵图列表.返回贴图组(贴图组,设备ID)
	空卡槽贴图 = ttz[0]
	卡槽默认贴图 = ttz[1]
	卡槽边框默认贴图 = ttz[2]
	if 器械ID > -1 and 启用 == true:
		$"消耗".text = str(消耗) #消耗显示
		visible = true
		modulate = Color(1.0, 1.0, 1.0, 1.0)
		$"贴图/背景板".texture = 卡槽默认贴图
		$"贴图/边框".texture = 卡槽边框默认贴图
		$"图片".visible = true
		$"消耗".visible = true
		$"裁剪节点".visible = true
		$"裁剪节点/冷却".modulate = Color(0.0, 0.0, 0.0, 0.502)
		器械 = 精灵图列表.Card_Pack[器械ID]
		if 器械ID != 当前ID:
			for i in $"图片".get_children():
				i.queue_free()
			var qxi = 精灵图列表.生成物体贴图(器械ID)
			$"图片".add_child(qxi)
			当前ID = 器械ID
	else:
		$"贴图/背景板".texture = 空卡槽贴图
		$"贴图/边框".texture = null
		modulate = Color(1.0, 1.0, 1.0, 0.627)
		$"裁剪节点/冷却".modulate = Color(1.0,1.0,1.0,0)
		$"图片".visible = false
		$"消耗".visible = false
		$"裁剪节点".visible = false

func 已被放置():
	if 启用 == true:
		get_tree().current_scene.当前器械 = null
		get_tree().current_scene.器械能 -= 消耗
		开始冷却(0)

func 判定选卡状态():
	if get_tree().current_scene.当前状态 == "选卡" or 器械ID <= -1:
		return true
	else:
		return false
#endregion
func 设定鼠标状态():
	if get_tree().current_scene.当前器械 == 器械 or 器械ID <= -1:
		$"贴图/触摸点".mouse_default_cursor_shape = 0
	else:
		$"贴图/触摸点".mouse_default_cursor_shape = 2
#region 描述生成
func 创建描述():
		var 描述d = preload("res://UI/描述框.tscn")
		var 实例 = 描述d.instantiate()
		实例.position = 描述生成偏移
		实例.父物体 = $"."
		add_theme_color_override("fout_color",Color(0,0,0,1))
		$".".add_child(实例)
		描述 = 实例

func 创建文本(缩放:int = 32,颜色:Color = Color(0.0, 0.0, 0.0, 1.0),粗细:int = 1):
	var 文本 = Label.new()
	文本.theme = preload("uid://bsballpnnbgjv")
	文本.horizontal_alignment = 1
	文本.vertical_alignment = 1
	文本.anchors_preset = 0
	文本.size.x = 0
	文本.size.y = 0
	文本.custom_minimum_size = Vector2(0,0)
	文本.add_theme_font_size_override("outline_size",粗细)
	文本.add_theme_font_size_override("font_size",缩放)
	文本.add_theme_color_override("font_color",颜色)
	return 文本
#endregion
#region 选卡类
func 选卡():
	var id:int = 0
	var on:bool = true
	var 取数组值:Array[int] = get_tree().current_scene.已选卡
	while on == true and id < 取数组值.size():
		if 取数组值.get(id) == -1:
			get_tree().current_scene.来源[id] = $"."
			取数组值[id] = 器械ID
			Game_Ready.生成日志(["选卡完成id值为 ",取数组值.get(id)])
			on = false
		id += 1
	Game_Ready.生成日志(取数组值)
#endregion
## 其实人家是读取属性啦
func 添加属性():
	if 器械ID > -1:
		附加属性 = 精灵图列表.Card_附加属性
		附加属性名称
		消耗 = 精灵图列表.Card_消耗[器械ID]
		冷却 = 精灵图列表.Card_冷却[器械ID]
		冷却减免 = 精灵图列表.Card_开局冷却减免百分比[器械ID]
