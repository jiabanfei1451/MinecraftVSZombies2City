extends Node2D
signal 选卡完成
#region 不知道
@export_group("mouse")
@export var mouseposition : Vector2
@export_group("音乐")
@export var 音乐ID : int = 0
@export var DEBUG : bool = false
@export var 地图亮度 : float = 0
@export_group("使用属性")
@export var 使用星之碎片 : bool = false
@export var 使用稿子 : bool = false
@export var 使用器械能 : bool = false
var 正在使用其他属性 : bool = false
#endregion

#region 草坪
@export_group("草坪")
@export var 当前器械 : PackedScene
@export var 来自 : Control
@export var 草坪二维向量 : Array[Array]
@export_subgroup("选卡信息")
@export var 已选卡 : Array[int] = [-1,-1,-1,-1,-1,-1,-1,-1,-1,-1]
@export var 来源 : Array[Control] = [null,null,null,null,null,null,null,null,null,null]
@export var 当前器械ID : int
#endregion

#region 信息
@export_group("信息")
@export var 关卡信息 : String = "万圣夜"
@export_range(0,9990,5) var 器械能: int = 50
@export var 当前状态 : String
#endregion

#region 属性
@export_group("属性")
var 即将重置 : bool = false
@export_enum("白日","夜晚") var 天色 : String = "白日"
@export_enum("开启","关闭") var 夜色滤镜 : String = "开启"
var 滤镜节点
@export var 光源颜色 : Color
@export var HDR光源 : bool = true
@export_range(0,1,0.01) var 光源强度 : float = 0.5
@export_range(0,1,0.01) var 滤镜强度 : float = 0.5
#endregion

var arrow = load("res://Image/misc/arrow.png")
var beam = load("res://arrow(City).png")
var ui场景 : PackedScene = preload("res://UI/card/关卡UI.tscn")
var ui场景2 : PackedScene = preload("res://UI/LevelUI/信息显示.tscn")
var ui场景3 : PackedScene = preload("res://UI/菜单.tscn")
var ui场景4 : PackedScene = preload("res://Scene/必要物体/文字.tscn")

func _ready() -> void:
	level_ready()
# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	level_process(delta)
#region 常用函数
func 禁用():
	当前器械 = null
	使用星之碎片 = false
	使用稿子 = false
	使用器械能 = false

func 稿子():
	if 使用稿子 == false:
		禁用()
		$"音效/铲子".play()
		使用稿子 = true
	else:
		使用稿子 = false
		$"音效/取消".play()

func 星之碎片():
	if 使用星之碎片 == false:
		禁用()
		使用星之碎片 = true
	else:使用星之碎片 = false

func _使用器械能():
	if 使用器械能 == false:
		禁用()
		使用器械能 = true
	else:
		使用器械能 = false
#endregion
func 生成节点(节点:PackedScene,生成位置:Node2D):
	var 节点实列 = 节点.instantiate()
	if 生成位置 == null:
		get_tree().current_scene.add_child(节点实列)
	else:
		生成位置.add_child(节点实列)
	print(String("["+Time.get_datetime_string_from_system())+"]",节点实列,",",生成位置)
	return 节点实列

func 场景生成(名称:String,层级:int):
	if get_node(名称) == null:
		var node : Node2D
		node = Node2D.new()
		add_child(node)
		node.name = 名称
		node.y_sort_enabled = true
		node.z_as_relative = false
		node.z_index = 层级
		node.position = Vector2(0,0)
		print("[",Time.get_time_string_from_system(),"]",name,"生成场景","“",名称,"”")
		return node
	else:
		print("[",Time.get_time_string_from_system(),"]",name,"无法生成场景","“",名称,"”","场景已存在")
	
func 选卡():
	$"选卡动画".选卡()
	$"音效/音乐".音乐选项 = 1
	当前状态 = "选卡"
func 完成选卡():
	生成节点(ui场景4,$".")
	var s = preload("res://Object/一次性音效.tscn")
	var x = create_tween()
	get_tree().current_scene.get_node("选卡动画").完成选卡()
	await get_tree().create_timer(1.5).timeout
	var soudo = 生成节点(s,$".")
	soudo.stream = preload("res://Sound/好.准备.安放器械！.ogg")
	soudo.play()
	$"文字/文字".scale = Vector2(1.5,1.5)
	$"文字/文字".text = "好"
	await get_tree().create_timer(0.5).timeout
	$"文字/文字".scale = Vector2(2,2)
	$"文字/文字".text = "准备"
	var xc = create_tween()
	xc.tween_property($"文字/文字","scale",Vector2(1.75,1.75),0.5).set_trans(Tween.TRANS_SINE)
	await get_tree().create_timer(0.5).timeout
	$"文字/文字".text = "安放器械！"
	get_tree().current_scene.当前状态 = "战斗中"
	$"文字/文字".scale = Vector2(2.25,2.25)
	await get_tree().create_timer(2).timeout
	emit_signal("选卡完成")
	xc = create_tween()
	xc.tween_property($"文字/文字","text",String(""),0.5).set_trans(Tween.TRANS_SINE)
	get_node("音效/音乐").音乐选项 = 音乐ID
	生成节点(ui场景,$".")
	生成节点(ui场景2,$".")
	生成节点(ui场景3,$".")
	
func 弃卡(当前卡槽:int):
	for i in 已选卡.size():
		if 当前卡槽+i+1 < 已选卡.size():
			来源[当前卡槽 + i] = 来源[当前卡槽 + i +1]
			已选卡[当前卡槽 + i] = 已选卡[当前卡槽 + i +1]
		elif 当前卡槽+i < 已选卡.size():
			来源[当前卡槽 + i] = null
			已选卡[当前卡槽 + i] = -1

func level_process(delta: float):
	if get_node("光源") != null:
		$"光源".modulate = Color(1.0, 1.0, 1.0, 光源强度)
	if 夜色滤镜 == "开启":
		滤镜节点.visible = true
	else:
		滤镜节点.visible = false
	滤镜节点.modulate = Color(光源颜色.r, 光源颜色.g, 光源颜色.b, 滤镜强度)
	if not 使用稿子 and not 使用星之碎片 and not 使用器械能:
		正在使用其他属性 = false
	else: 正在使用其他属性 = true
	if Input.is_action_just_pressed("S") == true:
		print("按下")
	if Input.is_action_just_pressed("DEBUG"):
		if DEBUG == false:
			print("DEBUG模式已开启")
			DEBUG = true
		else:
			print("DEBUG模式已关闭")
			DEBUG = false
	if Input.is_action_just_pressed("F5"):
		重置()
	if Input.is_action_pressed("←"):
		rotation += 1 * delta
	if Input.is_action_pressed("→"):
		rotation -= 1 * delta
	if Input.is_action_just_released("·"):
		_使用器械能()
	if Input.is_action_just_released("W"):
		星之碎片()
	if Input.is_action_just_released("Q"):
		稿子()
func level_ready():
	print(ProjectSettings.globalize_path("res://"))
	print("OS:",OS.get_distribution_name(),OS.get_version_alias())
	OS.request_permissions()
	#region 初始化
	生成节点(preload("res://Scene/必要物体/选卡动画.tscn"),null)
	节点提供变量.摄像头 = get_node("选卡动画")
	生成节点(preload("res://Scene/必要物体/音效.tscn"),null)
	var ps : Area2D
	ps = 生成节点(preload("res://Scene/必要物体/暗黑滤镜.tscn"),null)
	滤镜节点 = ps
	ps.position = Vector2(0,0)
	var 阴影 = 场景生成("阴影",1)
	节点提供变量.阴影 = 阴影
	var 粒子 = 场景生成("粒子",1)
	节点提供变量.粒子 = 粒子
	var 器械 = 场景生成("器械",2)
	节点提供变量.器械 = 器械
	var 粒子2 = 场景生成("粒子2",2)
	节点提供变量.粒子2 = 粒子2
	var 怪物 = 场景生成("怪物",2)
	节点提供变量.怪物 = 怪物
	var 射弹 = 场景生成("射弹",3)
	节点提供变量.射弹 = 射弹
	var 光源 = 场景生成("光源",5)
	节点提供变量.光源 = 光源
	var 特效 = 场景生成("特效",5)
	节点提供变量.特效 = 特效
	Input.set_custom_mouse_cursor(arrow)
	Input.set_custom_mouse_cursor(beam, Input.CURSOR_IBEAM)
	选卡()
	#endregion
func 重置():
	即将重置 = true
	await get_tree().create_timer(0.2)
	get_tree().reload_current_scene()
