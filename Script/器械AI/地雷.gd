extends "res://Script/组件/器械组件.gd"
var 贴图ID : int = 1
var 开启 : bool = false
var 爆了 : bool = false
func _ready():
	_开启大招时.connect(_on_大招触发)
	_受击时.connect(_on_受击时)
	_死亡时.connect(_on_死亡时)
	_倒计时完成.connect(_on_倒计时完成)
	初始化()
	

func _process(delta: float) -> void:
	循环判定(delta)
	if 贴图ID == 1:
		$"1".visible = true
	else:
		$"1".visible = false
	if 贴图ID == 2:
		$"2".visible = true
	else:
		$"2".visible = false
	if 贴图ID == 3:
		$"3".visible = true
	else:
		$"3".visible = false
	if 贴图ID == 4:
		$"4".visible = true
	else:
		$"4".visible = false
	if 贴图ID == 5:
		$"5".visible = true
	else:
		$"5".visible = false
	if 贴图ID == 6:
		$"6".visible = true
	else:
		$"6".visible = false
	if 选定攻击 != null and 开启 == true and 爆了 == false:
		爆了 = true
		$"检测".visible = false
		visible = false
		var h = preload("res://effect/boom.tscn")
		var boom = h.instantiate()
		boom.scale = Vector2(1.5,1.5)
		boom.position = position
		节点提供变量.特效.add_child(boom)
		var yc = preload("res://Object/一次性音效.tscn")
		var ycs = yc.instantiate()
		ycs.stream = preload("res://Sound/地雷爆炸.ogg")
		get_tree().current_scene.add_child(ycs)
		ycs.play()
		for i in 怪物.size():
			怪物[i].减少血量(90)
		for i in 20:
			节点提供变量.摄像头.pos = Vector2(randf_range(10,-10),randf_range(10,-10))
			await get_tree().create_timer(0.02).timeout
		节点提供变量.摄像头.position = 全局变量.游戏中镜头坐标
		queue_free()
		
func _on_倒计时完成() -> void:
	激活()
	计时器节点.stop()
func _on_受击时(伤害:float,攻击者:Area2D) -> void:
	pass

func _on_大招触发() -> void:
	pass

func _on_死亡时() -> void:
	pass

func 激活():
	var rect : RectangleShape2D = RectangleShape2D.new()
	rect.size = Vector2(20,30)
	$GPUParticles2D.emitting = true
	$CollisionShape2D.shape = rect
	var ds = AtlasTexture.new()
	ds.atlas = preload("res://Object/素材图/sactx-0-1024x2048-BC7-entities-d025fdc4.png")
	ds.region = Rect2(632,1227,48,1)
	ds.filter_clip = true
	$TNT.texture = ds
	var tw = create_tween()
	var taw = create_tween()
	var tdw = create_tween()
	$TNT/TNT2.visible = true
	$"出土".play()
	tw.tween_property(ds,"region",Rect2(632,1227,48,30),1).set_trans(Tween.TRANS_QUINT)
	taw.tween_property($TNT/TNT2,"offset",Vector2(0,-20),1).set_trans(Tween.TRANS_QUINT)
	tdw.tween_property($TNT,"offset",Vector2(0,-4),1).set_trans(Tween.TRANS_QUINT)
	for i in 6:
		贴图ID = i
		await get_tree().create_timer(0.08).timeout
	$GPUParticles2D.emitting = false
	开启 = true
