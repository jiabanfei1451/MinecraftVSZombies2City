# meta-name : tool
extends "uid://xv5xcf841ce4"
class_name 怪物阵营
var 检测到的 : Array[Area2D]
@export var 移动速度 : float = 1

func _process(delta: float) -> void: 
	选定物体()
	FPS = delta
	$".".检测到的 = 已检测
	if 选定攻击 == null:
		$"检测".visible = true
	else:
		$"检测".visible = false

func 触发死亡():
	$"受击音效".stream = 死亡
	$"受击音效".play()
	var te = create_tween()
	te.tween_property($".","modulate",Color(1.0, 0.0, 0.0, 1.0),颜色持续时间)
	await get_tree().create_timer(1.5).timeout
	生成粒子()
	queue_free()
	
func 移动():
	while 是否死亡 == false:
		if 选定攻击 == null:
			position.x -= (20 * 移动速度) * FPS
		await get_tree().create_timer(FPS).timeout
		
func 受击时():
	$"受击音效".stream = 受击.pick_random()
	$"受击音效".play()
	var te = create_tween()
	te.tween_property($".","modulate",Color(1.0, 0.0, 0.0, 1.0),颜色持续时间)
	te.tween_property($".","modulate",Color(1.0, 1.0, 1.0, 1.0),颜色持续时间)
