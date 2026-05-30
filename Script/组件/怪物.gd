extends Area2D
var spell_targets = 0
@export_group("效果")
@export var 粒子生成范围 : RectangleShape2D = RectangleShape2D.new()
@export var 生成偏移 : Vector2
@export var 数量 : int = 45
@export_group("属性")
@export_group("属性/不可更改")
@export var FPS : float
@export var 选定攻击 : Area2D
@export var 已检测 : Array[Area2D]
@export_group("属性")
@export var 攻击力 : float = 0.5
@export_group("移动组件")
@export var 速度 : float = 1
var 状态 :String
@export_group("绑定物理")
@export var 组件节点 : Node
@export_group("生命组件")
@export_group("生命组件/血量")
@export var 血量 : float = 15
@export var 最大血量 : float
@export var 自动设置最大血量 : bool = true
@export_group("生命组件/效果")
@export var 受击: Array[AudioStreamOggVorbis] = [
	preload("res://Sound/怪物受击/splat1.ogg"),
	preload("res://Sound/怪物受击/splat2.ogg"),
	preload("res://Sound/怪物受击/splat3.ogg")
]
@export var 死亡 : AudioStreamOggVorbis = preload("res://Sound/僵尸死亡/zombie_death.ogg")
@export var 特效 : PackedScene
@export var 是否死亡 : bool = false
@export var 颜色持续时间 : float = 0.2
@export_group("额外脚本")
@export var 脚本 : Array[Script]
# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	if 自动设置最大血量 == true:
		最大血量 = 血量
	组件节点.生成节点 = $"."
	初始化移动()
	var s = preload("res://Object/可互动/音频.tscn")
	var ds = s.instantiate()
	ds.选项 = "音效"
	ds.name = "受击音效"
	add_child(ds)
	ds = s.instantiate()
	ds.选项 = "音效"
	ds.name = "死亡音效"
	add_child(ds)
func _process(delta: float) -> void: 
	选定物体()
	FPS = delta
	$".".检测到的 = 已检测
	if 选定攻击 == null:
		$"检测".visible = true
	else:
		$"检测".visible = false

func 选定物体():
	if 选定攻击 == null:
		for i in 已检测.size():
			if 已检测[i] != null:
				选定攻击 = 已检测[i]
				扫描数组()
				
func 减少血量(数量:float):
	血量 -= 数量
	if 血量 >= 1 and 是否死亡 == false:
		$".".受击时()
	else:
		$".".触发死亡()
		是否死亡 = true
		
func 初始化移动():
	$".".移动()

func 检测存在状态():
	if 选定攻击 == null:
		return null

func 扫描数组():
	var 数量 = 已检测.size()
	for i in 数量:
		if 已检测.size() >= i and 已检测.has(i) and 已检测[i] == null:
			已检测.remove_at(i)
			i - 1

func 生成粒子():
	for i in 数量:
		var li = preload("res://Object/粒子/怪物消散粒子.tscn")
		var liins = li.instantiate()
		liins.position.x = position.x + randf_range(粒子生成范围.size.x,-粒子生成范围.size.x) + 生成偏移.x
		liins.position.y = position.y + randf_range(粒子生成范围.size.y,-粒子生成范围.size.y) + -生成偏移.y
		var qx = randi_range(0,1)
		if qx == 0:
			节点提供变量.粒子2.add_child(liins)
		else:
			节点提供变量.粒子.add_child(liins)
