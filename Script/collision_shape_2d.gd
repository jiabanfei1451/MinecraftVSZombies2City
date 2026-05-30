extends CollisionShape2D


# Called when the node enters the scene tree for the first time
# 定义一个存储 Area2D 的数组
var area_2d_array: Array[Area2D] = []

func _ready():
	# 模拟向数组中添加一些 Area2D 节点
	var area1 = Area2D.new()
	area1.name = "Area1"
	add_child(area1)
	area_2d_array.append(area1)
	
	# 要检测的目标 Area2D
	var target_area = area1
	
	# 核心检测逻辑：使用 in 关键字
	if target_area in area_2d_array:
		print("目标 Area2D 存在于数组中！")
	else:
		print("目标 Area2D 不存在于数组中！")
