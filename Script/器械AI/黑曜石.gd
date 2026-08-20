extends "res://Script/组件/器械组件.gd"

func _ready():
	_开启大招时.connect(_on_大招触发)
	_受击时.connect(_on_受击时)
	_死亡时.connect(_on_死亡时)
	_倒计时完成.connect(_on_倒计时完成)
	初始化()

func _process(delta: float) -> void:
	循环判定(delta)

func _on_倒计时完成() -> void:
	pass

func _on_受击时(伤害:float,攻击者:Area2D) -> void:
	pass

func _on_大招触发() -> void:
	$"升级".play()
	血量 *= 2
	最大血量 *= 2

func _on_死亡时() -> void:
	pass
