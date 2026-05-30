extends Area2D
var d
var ins : int
signal _受击时(伤害:float,攻击者:Area2D)
signal _倒计时完成()
signal _开启大招时()
signal _死亡时()

#region 物体绑定
@export_group("物体绑定")
@export var 碰撞节点 : ColorRect
@export var 碰撞自动生成 : bool = false
@export var 碰撞大小 : Vector2 = Vector2(40,40)
@export var 碰撞生成偏移 : Vector2 = Vector2(-20,-20)
@export var 死亡音效播放节点 : AudioStreamPlayer
@export var 挖掘音效播放节点 : AudioStreamPlayer
@export var 大招音效播放节点 : AudioStreamPlayer
@export var 自动生成音频节点 : bool = true
@export var 计时器节点 : Timer
@export var 计时器自动生成 : bool = false
@export var 自动生成物理组件 : bool = true
#endregion

#region 属性
@export_group("属性")
@export var 缩放 : Vector2 = Vector2(1,1)
@export var 坐标校准 : Vector2
@export_range(0,0.5,0.01) var 校准延迟 : float = 0.01
var 绑定 : bool
var 获得焦点 : bool
#endregion

#region 光源
@export_group("光源")
@export var 光源节点 : Node2D
@export var 光源数组 : Array[PackedScene]
@export var 光源生成偏移 : Array[Vector2]
@export var 光源颜色 : Array[Color]
@export var 光源缩放 : Array[Vector2]
#endregion

#region 生命组件
@export_group("生命组件")
@export var 血量 : float = 15
@export var 最大血量 : float
@export var 自动设置最大血量 : bool = true
var 是否死亡 : bool = false
@export var 死亡时销毁 : bool = true
@export_group("生命组件/效果")
@export var 粒子效果颜色 : Array[Color] = [Color(1.0, 1.0, 1.0, 1.0)]
@export var 粒子数量 : int = 15
@export var 死亡音效 : Array[AudioStreamOggVorbis]
@export var 放置音效 : Array[AudioStreamOggVorbis] = [preload("res://Sound/石头挖掘音效/stone1.ogg"),
preload("res://Sound/石头挖掘音效/stone2.ogg"),
preload("res://Sound/石头挖掘音效/stone3.ogg"),
preload("res://Sound/石头挖掘音效/stone4.ogg")]
#endregion

#region 攻击组件
@export_group("攻击组件")
@export var 大招特效 : PackedScene = preload("uid://cryojjhf4dxf")
@export var 大招特效偏移 : Vector2

@export_group("攻击组件/攻击属性")
@export var 向量 : Vector2 = Vector2(-12,-15)
@export var 后坐力_ : float = 1
@export var 攻速 : Vector2 = Vector2(1.2,1.7)
@export var 射弹节点 : Node2D

@export_group("攻击组件")
var 开始攻击 : bool = false
@export var 怪物 : Array[Area2D]
@export var 选定攻击 : Area2D
var FPS : float = 0
var 大招 : bool = false
#endregion

#region 物理
@export_group("物理")
@export var 物理节点 : Node
#endregion

func _倒计时结束() -> void: #倒计时
	_倒计时完成.emit()
	if 大招 == false:
		计时器节点.wait_time = float(randf_range(攻速.x,攻速.y))

func _离开时(area: Area2D) -> void: #碰撞检测
	if 怪物.has(area) == true and not is_in_group("判定"):
		怪物.erase(area)
		
func _碰到时(area: Area2D) -> void: #碰撞检测2
	if area.is_in_group("怪物") and is_in_group("判定") == false:
		if $"物理组件".计算Y(area) == true:
			if 怪物.has(area) == false:
				怪物.append(area)

func 减少血量(减少血量:float,伤害者:Area2D):
	_受击时.emit(减少血量,伤害者)
	if ins >= 10:
		ins = 0
		粒子生成(1)
	else:
		ins += 60 * 减少血量
	血量 -= 减少血量

func 死亡():
	if 是否死亡 == false: #中文变量这一块
		_死亡时.emit()
		是否死亡 = true
		粒子生成(粒子数量)
		var 节点音效 = preload("res://Object/一次性音效.tscn").instantiate()
		节点音效.stream = 死亡音效.pick_random()
		get_tree().current_scene.add_child(节点音效)
		节点音效.play()
		if 死亡时销毁 == true:
			queue_free()

func 触发大招():
	_开启大招时.emit()
	var 循环数 : int = 0
	$"大招音效".play()
	var 特效dd = 大招特效.instantiate()
	特效dd.position = $".".position + Vector2(14,0) + 大招特效偏移
	$"../..".get_node("特效").add_child(特效dd)
	大招 = true

func _on_碰到鼠标时() -> void:
	获得焦点 = true
	if get_tree().current_scene.正在使用其他属性 == true:
		$".".modulate = Color(1.353, 1.353, 1.353, 1.0)

func _on_鼠标离开时() -> void:
	获得焦点 = false
	$".".modulate = Color(1.0, 1.0, 1.0, 1.0)

func 生成日志(日志: String):
	var dataTime : String = Time.get_datetime_string_from_system()
	if get_tree().current_scene.DEBUG == true:
		print("[",dataTime,"]",name,日志)

func 选定物体():
	var line : int = 怪物.size()
	for i in line:
		if 怪物[i] != null:
			选定攻击 = 怪物[i]
			i = 21000000000
		else:
			怪物.remove_at(i)
			line -= 1
			i -= 1
			print("剔除2")

#region 初始化
func _ready():
	初始化()
func _process(delta: float) -> void: #检测
	循环判定(delta)

func 初始化():
	d = 怪物
	最大血量 = 血量
	visible = false
	物理节点.生成节点 = $"."
	var 一次性音效 = preload("res://Object/一次性音效.tscn")
	var 一次性音效i = 一次性音效.instantiate()
	var 音效 = preload("res://Object/可互动/音频.tscn")
	var d = 音效.instantiate()
	if 碰撞自动生成 == true:
		if 碰撞节点 == null:
			var cor = ColorRect.new()
			cor.position = 碰撞生成偏移
			cor.size = 碰撞大小
			if get_tree().current_scene.DEBUG == false:
				cor.modulate = Color(1.0, 1.0, 1.0, 0.0)
			else:
				cor.modulate = Color(0.0, 1.0, 0.033, 0.467)
			add_child(cor)
			碰撞节点 = cor
			cor.mouse_entered.connect(_on_碰到鼠标时)
			cor.mouse_exited.connect(_on_鼠标离开时)
		else:
			碰撞节点.mouse_entered.connect(_on_碰到鼠标时)
			碰撞节点.mouse_exited.connect(_on_鼠标离开时)
	if 计时器自动生成 == true:
		if 计时器节点 == null:
			var tim = Timer.new()
			tim.autostart = true
			add_child(tim)
			计时器节点 = tim
			tim.wait_time = 攻速.x
			tim.timeout.connect(_倒计时结束)
			tim.start(攻速.x)
	if 自动生成音频节点 == true:
		if get_node("大招音效") == null:
			d.name = "大招音效"
			d.选项 = "音效"
			add_child(d)
			大招音效播放节点 = d
			大招音效播放节点.stream = preload("res://Sound/开大.ogg")
		if get_node("挖掘音效") == null:
			d = 音效.instantiate()
			d.name = "挖掘音效"
			d.选项 = "音效"
			add_child(d)
			挖掘音效播放节点 = d
		if get_node("死亡音效") == null:
			d = 音效.instantiate()
			d.name = "死亡音效"
			d.选项 = "音效"
			add_child(d)
			死亡音效播放节点 = d
	if 自动生成物理组件 == true:
		if 物理节点 == null:
			var wl = preload("uid://dq2hxvboug84f")
			var wlo = wl.instantiate()
			wlo.生成节点 = $"."
		else:
			物理节点.生成节点 = $"."
	一次性音效i.stream = 放置音效.pick_random()
	get_tree().current_scene.add_child(一次性音效i)
	一次性音效i.play()
	await get_tree().create_timer(物理节点.开始延迟 + 校准延迟).timeout
	visible = true
	物理节点.初始高度 += 坐标校准.y
	position.x += 坐标校准.x
	光源节点 = get_tree().current_scene.get_node("光源")
	var 循环取值 : int = 0
	for i in 光源数组.size(): 
		var 实列化光源 = 光源数组[循环取值].instantiate()
		实列化光源.绑定偏移 = 光源生成偏移[循环取值]
		实列化光源.绑定颜色 = 光源颜色[循环取值]
		实列化光源.绑定缩放 = 光源缩放[循环取值]
		实列化光源.绑定物体 = $"."
		光源节点.add_child(实列化光源)
		实列化光源.z_index = 4
		循环取值 += 1

func 循环判定(delta: float):
	if get_tree().current_scene.DEBUG == true:
		if d != 怪物:
			d = 怪物
	FPS = delta
	if 血量 > 最大血量:
		血量 = 最大血量
	if 选定攻击 == null:
		选定物体()
	if 获得焦点 == true and Input.is_action_just_released("左键") and get_tree().current_scene.使用星之碎片 == true and 大招 == false:
		触发大招()
	if 获得焦点 == true and Input.is_action_just_released("左键") and get_tree().current_scene.使用稿子:
		死亡()
	if 血量 <= 0: #检测血量状态
		死亡()
	$".".scale = 缩放

func 粒子生成(数量:int):
	var 随机值 : float = 0
	for i in 数量:
		随机值 = randf_range(0,100)
		var 粒子 : PackedScene = preload("res://Object/粒子/粒子.tscn")
		var 粒子实例化 = 粒子.instantiate()
		粒子实例化.modulate = 粒子效果颜色.pick_random()
		粒子实例化.position = position + Vector2(0,10)
		if 随机值 > 50:
			节点提供变量.粒子.add_child(粒子实例化)
		else:
			节点提供变量.粒子2.add_child(粒子实例化)
#endregion
