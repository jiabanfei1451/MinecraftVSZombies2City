extends "res://Script/组件/器械组件.gd"

func _ready():
	_开启大招时.connect(_on_大招触发)
	_受击时.connect(_on_受击时)
	_死亡时.connect(_on_死亡时)
	_倒计时完成.connect(_on_倒计时完成)
	初始化()

func _process(delta: float) -> void:
	循环判定(delta)

func _on_倒计时完成():
	if 选定攻击 != null and 大招 == false:
		var 箭矢 = preload("uid://caymc0p7rsog")
		$"发射音效".play()
		$"动画".play("发射")
		var 箭矢i = 箭矢.instantiate()
		箭矢i.position = position + Vector2(19,-10)
		节点提供变量.射弹.add_child(箭矢i)

func _on_受击时(伤害:float,攻击者:Area2D):
	pass

func _on_大招触发():
	$"动画".play("大招(开启中)")
	for i in 60:
		$"发射音效".play()
		var 箭矢 = preload("uid://caymc0p7rsog")
		var 箭矢i = 箭矢.instantiate()
		箭矢i.position = position + Vector2(19,-10)
		节点提供变量.射弹.add_child(箭矢i)
		await get_tree().create_timer(0.05).timeout
	$"动画".play("发射")
	大招 = false

func _on_死亡时():
	pass
