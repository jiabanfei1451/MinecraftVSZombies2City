extends "res://Script/组件/器械组件.gd"

func _ready():
	初始化()
	if 启用 == true:
		_开启大招时.connect(_on_大招触发)
		_受击时.connect(_on_受击时)
		_死亡时.connect(_on_死亡时)
		_倒计时完成.connect(_on_倒计时完成)
		计时器节点.stop()
		计时器节点.start(5)
func _process(delta: float) -> void:
	循环判定(delta)

func _on_倒计时完成() -> void:
	var d = create_tween()
	d.tween_property($AnimatedSprite2D,"modulate",Color(1.491, 1.491, 1.491, 1.0),1).set_trans(Tween.TRANS_QUART)
	await d.finished
	var 红石 = preload("uid://dmfe85smdma7l")
	var shi = 红石.instantiate()
	d = create_tween()
	shi.position = position - Vector2(10,0)
	节点提供变量.粒子2.add_child(shi)
	d.tween_property($AnimatedSprite2D,"modulate",Color(1.0, 1.0, 1.0, 1.0),1).set_trans(Tween.TRANS_QUART)
	

func _on_受击时(伤害:float,攻击者:Area2D) -> void:
	pass

func _on_大招触发() -> void:
	for i in 6:
		var 红石 = preload("uid://dmfe85smdma7l")
		var shi = 红石.instantiate()
		shi.position = position - Vector2(10,0)
		节点提供变量.粒子2.add_child(shi)
		$"发射音效/开大产出".play()
		await get_tree().create_timer(0.25).timeout
	大招 = false

func _on_死亡时() -> void:
	pass
