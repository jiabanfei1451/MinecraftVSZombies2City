extends Node

var 射弹 : PackedScene = preload("uid://caymc0p7rsog")
		
func 触发大招():
	$"../../动画".play("大招(开启中)")
	for i in 60:
		var 射弹2 = 射弹.instantiate() 
		射弹2.position = $"../..".position + Vector2(16,-13)
		节点提供变量.射弹.add_child(射弹2)
		await get_tree().create_timer(0.05).timeout
	$"../../动画".play("发射")
	$"../..".大招 = false
