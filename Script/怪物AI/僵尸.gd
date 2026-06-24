extends "res://Script/组件/怪物.gd"
var 检测到的 : Array[Area2D]
@export var 移动速度 : float = 1


func _process(delta: float) -> void: 
	循环(delta)
	if 选定攻击 != null:
		选定攻击.减少血量((攻击力 * delta) * 3,$".")

func 触发死亡():
	$"受击音效".stream = 死亡
	$"受击音效".play()
	$"AnimationTree/腿部动画".stop()
	$"过渡节点/手部动画".play("死亡/死亡")
	var te = create_tween()
	te.tween_property($".","modulate",Color(1.0, 0.0, 0.0, 1.0),颜色持续时间 * 1.5)
	await get_tree().create_timer(1.5).timeout
	生成粒子()
	queue_free()
	
func 移动():
	手部()
	腿部()
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

func 手部() -> void:
	while 是否死亡 == false:
		if 选定攻击 == null:
			if $"过渡节点/手部动画".is_playing() == false or $"过渡节点/手部动画".assigned_animation != "手（摆动）":
				$"过渡节点/手部动画".play("手（摆动）")
		else:
			if $"过渡节点/手部动画".is_playing() == false or $"过渡节点/手部动画".assigned_animation != "手_挖掘":
				$"过渡节点/手部动画".play("手_挖掘")
		await get_tree().create_timer(FPS).timeout
func 腿部() -> void:
	while 是否死亡 == false:
		if 选定攻击 == null:
			if $"AnimationTree/腿部动画".is_playing() == false or $"AnimationTree/腿部动画".assigned_animation != "腿部_跑动":
				$"AnimationTree/腿部动画".play("腿部_跑动")
		else:
			if $"AnimationTree/腿部动画".is_playing() == true and $"AnimationTree/腿部动画".assigned_animation != "腿部_挖掘":
				$"AnimationTree/腿部动画".play("腿部_挖掘")
		await get_tree().create_timer(FPS).timeout
