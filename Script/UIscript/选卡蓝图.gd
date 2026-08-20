extends Control
@export var 绑定数组 : int = 0
@export_group("贴图")
var 空卡槽贴图 : Texture2D
var 卡槽默认贴图 : Texture2D
var 卡槽边框默认贴图 : Texture2D
@export var 贴图组 : int = 0
@export_group("卡槽状态")
@export var 启用 : bool = true
var 器械 : PackedScene
@export var 器械ID : int = -1
@export var 消耗 : int = 50
@export var 冷却 : float = 7.5
@export var 冷却中 : bool = false
@export var UI:CanvasLayer
@export_subgroup("选卡蓝图")
var sdd :bool = false
var 过度速度 : float = 0.1
# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	if 全局变量.卡槽提示词动画 == true:
		过度速度 = 0.1
	else:
		过度速度 = 0
	var ttz = 精灵图列表.返回贴图组(贴图组)
	空卡槽贴图 = ttz[0]
	卡槽默认贴图 = ttz[1]
	卡槽边框默认贴图 = ttz[2]

	if 绑定数组 + 1 > 全局变量.卡槽数量:
		visible = false
	else:
		visible = true
func _鼠标进入():
	sdd = true
func _鼠标离开():
	sdd = false

func _点击():
	get_tree().current_scene.弃卡(绑定数组)
# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	if 启用 == true:
		器械ID = get_tree().current_scene.已选卡[绑定数组]
	设定贴图()
	添加名称描述()

func 设定贴图():
	if 器械ID <= -1:
		$"图片".visible = false
		$"贴图/边框".visible = false
		$"贴图/背景板".texture = 空卡槽贴图
	else:
		$"图片/显示图片".texture = 精灵图列表.img[器械ID]
		$"消耗".text = str(精灵图列表.消耗[器械ID])
		$"描述/名称".text = str(精灵图列表.名称[器械ID])
		$"描述/描述".text = str(精灵图列表.描述[器械ID])
		$"图片".visible = true
		$"贴图/边框".visible = true
		$"贴图/边框".texture = 卡槽边框默认贴图
		$"贴图/背景板".texture = 卡槽默认贴图
func 添加名称描述():
	var ss = create_tween()
	if 器械ID != -1 and sdd == true:
		if $"描述/名称".size.x > $"描述/描述".size.x:
			ss.tween_property($"描述","size",Vector2($"描述/名称".size.x + 10,91.55),过度速度) #$"描述".size.x =
		else:
			ss.tween_property($"描述","size",Vector2($"描述/描述".size.x + 10,91.5),过度速度) #$"描述".size.x =
			#$"描述".size.x = 
	else:
		ss.tween_property($"描述","size",Vector2(0,91.5),过度速度)
	if $"描述".size.x > 152:
		$"描述".position.x = ((152 - $"描述".size.x) / 2) * 1
