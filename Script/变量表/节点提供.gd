extends Node

static var 阴影 : Node2D
static var 器械 : Node2D
static var 怪物 : Node2D
static var 射弹 : Node2D
static var 光源 : Node2D
static var 特效 : Node2D
static var 粒子 : Node2D
static var 粒子2 : Node2D
static var 摄像头 : Camera2D

func 生成节点(节点:PackedScene,生成节点:String = "pass",层级:int = 0,自动排序:String = "pass"):
	var 实例化 = 节点.instantiate()
	实例化.z_index = 层级
	if 自动排序 == "pass":
		实例化.z_as_relative = false
		实例化.y_sort_enabled = false
	elif 自动排序 == "x":
		实例化.z_as_relative = true
		实例化.y_sort_enabled = false
	elif 自动排序 == "y":
		实例化.z_as_relative = false
		实例化.y_sort_enabled = true
	elif 自动排序 == "xy":
		实例化.y_sort_enabled = true
		实例化.z_as_relative = true
	elif 自动排序 == "yx":
		实例化.y_sort_enabled = true
		实例化.z_as_relative = true
	if 生成节点 == "pass":
		get_tree().current_scene.get_node(生成节点).add_child(实例化)
	else:
		get_tree().current_scene.add_child(实例化)
	return 实例化
