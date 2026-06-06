extends "res://Script/组件/场景组件/场景初始化.gd"
signal 怪物生成前(物体:Area2D)
signal 怪物生成后(物体:Area2D)
signal 一大波执行时()
signal 最后一波执行时()
signal 开始生成()
signal 计时开始时()
signal 计时完成后()
signal 条件判断为真()

@export_group("生成")
var 最大波数 : int
var 当前波数 : int
@export var 生成列表 : Array[Array]
@export var 生成数量 : Array[int]
@export var 开始倒计时 : int = 30
@export var 剩余数量 : Array[Area2D]
@export var 下一波倒计时 : float = 45
@export var 大波数组 : Array[int]
@export_subgroup("生成延迟")
@export var 生成延迟 : float = 0.02
@export var 生成坐标 : Vector2 = Vector2(500,45)
@export var 生成倍率 : float = 80
@export var 生成线路 : Vector2 = Vector2(3,-3)
@export_group("Level_Json")
@export var JSONPath : String = "res://2/data/level_data/"
@export var LoadJSON : bool = false
var 生成中 : bool = false
var 剩余计时 = 0
var 暂停生成 : bool = false
var FPS : float
var 正在排除中 : bool = false

func _ready() -> void:
	level_ready2()
func _process(delta: float) -> void:
	level_process2(delta)
	
func level_ready2():
	level_ready()
	if LoadJSON == true:
		读取()
func level_process2(delta: float):
	level_process(delta)
	最大波数 = 生成数量.size()
	FPS = delta
	排除空数组()
	条件()
func 生成开始():
	生成中 = true
	emit_signal("开始生成")
	while 生成中 and 当前波数 < 生成数量.size() and 即将重置 == false:
		当前波数 += 1
		for i in 大波数组:
			if 当前波数 == i:
				提示一大波怪物()
				await get_tree().create_timer(2).timeout
				if i == 大波数组.back():
					最后一波()
					await get_tree().create_timer(2).timeout
		生成波(当前波数-1)
		await 条件判断为真
		await get_tree().create_timer(FPS).timeout
	当前波数 = 0

func 生成波(波:int):
	if 波 < 生成数量.size():
		if 生成数量[波] != null and 即将重置 == false:
			for i in 生成数量[波]:
				怪物生成(波)

func 条件():
	if 剩余数量.size() == 0 and 暂停生成 == false and 即将重置 == false:
		emit_signal("条件判断为真")
func 怪物生成(列表ID:int):
	var scone : PackedScene = 精灵图列表.怪物数组[生成列表[列表ID].pick_random()]
	var sx = scone.instantiate()
	sx.position = Vector2(500,45+(80 * int(randf_range(3,-3))))
	emit_signal("怪物生成前",sx)
	$"怪物".add_child(sx)
	剩余数量.append(sx)
	emit_signal("怪物生成后",sx)
func 排除空数组():
	if 正在排除中 == false and 即将重置 == false:
		正在排除中 = true
		if 剩余数量.size() != 0 and 即将重置 == false:
			for i in 剩余数量.size():
				if i <= 剩余数量.size() -1 and 剩余数量[i] == null and 即将重置 == false:
					剩余数量.remove_at(i)
					print("排除成功")
				await get_tree().create_timer(FPS).timeout
		正在排除中 = false

## 描述？
func 添加波(数量:int = 1,arr:Array[int] = [],大波:bool = false):
	var siz = 生成列表.size()
	生成列表.append([])
	生成数量.append(数量)
	siz = 生成列表.size()
	生成列表.back().append_array(arr)
	if 大波 == true and 即将重置 == false:
		大波数组.append(siz)
	print(大波数组)

func 提示一大波怪物():
	emit_signal("一大波执行时")
	var 文字 = get_tree().current_scene.get_node("文字/文字")
	var 初始缩放 : Vector2 = 文字.scale
	var 音效 : PackedScene = preload("res://Object/一次性音效.tscn")
	var 音效实例化 : AudioStreamPlayer = 音效.instantiate()
	音效实例化.stream = preload("res://Sound/一大波怪物进攻音效.ogg")
	音效实例化.autoplay = true
	文字.scale = 初始缩放 * 2
	文字.modulate = Color(1,1,1,0)
	add_child(音效实例化)
	文字.text = 获取Json文本.获取Json内容("Level_Text.json",0,0)
	var tweenscale = create_tween()
	var tewwncolor = create_tween()
	tweenscale.tween_property(文字,"scale",初始缩放 * 0.9,0.9).set_trans(Tween.TRANS_QUART)
	tewwncolor.tween_property(文字,"modulate",Color(1.0, 1.0, 1.0, 1.0),0.75).set_trans(Tween.TRANS_QUART)
	await get_tree().create_timer(0.5).timeout
	tewwncolor = create_tween()
	tewwncolor.tween_property(文字,"modulate",Color(1.0, 1.0, 1.0, 0.0),0.75).set_trans(Tween.TRANS_QUART)

func 最后一波():
	emit_signal("最后一波执行时")
	var 文字 = get_tree().current_scene.get_node("文字/文字")
	var 初始缩放 : Vector2 = 文字.scale
	var 音效 : PackedScene = preload("res://Object/一次性音效.tscn")
	var 音效实例化 : AudioStreamPlayer = 音效.instantiate()
	音效实例化.stream = preload("res://Sound/最后一波.ogg")
	音效实例化.autoplay = true
	文字.scale = 初始缩放 * 2
	文字.modulate = Color(1,1,1,0)
	add_child(音效实例化)
	文字.text = 获取Json文本.获取Json内容("Level_Text.json",0,1)
	var tweenscale = create_tween()
	var tewwncolor = create_tween()
	tweenscale.tween_property(文字,"scale",初始缩放,0.9).set_trans(Tween.TRANS_QUART)
	tewwncolor.tween_property(文字,"modulate",Color(1.0, 1.0, 1.0, 1.0),0.75).set_trans(Tween.TRANS_QUART)
	await  get_tree().create_timer(0.5).timeout
	tewwncolor = create_tween()
	tewwncolor.tween_property(文字,"modulate",Color(1.0, 1.0, 1.0, 0.0),0.75).set_trans(Tween.TRANS_QUART)
func 读取():
	var s = JsonData.new()
	var d = s.load_data(JSONPath,"")
	var MapName = d.data.Level_name
	var MapDay = d.data.Level_Day
	var LevelBGM = d.data.Level_BGM
	var monsterArray = d.data.Level_monster
	var monsterValue = d.data.Level_monster_Value
	var Wave = d.data.Level_Wave
	if MapDay != null and MapDay != null:
		关卡信息 = MapName + "-" + MapDay
	if LevelBGM != null:
		音乐ID = LevelBGM
	if monsterArray != null:
		var V : int = 0
		var arrayint : Array[int] = []
		for i in monsterArray:
			arrayint.clear()
			arrayint.append_array(i)
			添加波(monsterValue[V],arrayint,Wave[V])
			V += 1
