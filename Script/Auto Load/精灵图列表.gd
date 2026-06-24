extends Node

static var 怪物数组 : Array[PackedScene]

var Card_Vector : Array[Vector2]
var Card_Scale : Array[Vector2]
var Card_Pack : Array[PackedScene]
var Card_名称 : Array[String]
var Card_描述 : Array[String]
var Card_冷却 : Array[float]
var Card_消耗 : Array[int]
var Card_开局冷却减免百分比 : Array[float]
var Card_附加属性 : Array
var Card_附加属性名称 : Array[String]
var Card_内部名称 : Array[String]

static var Card_卡槽边框贴图 : Array[Array] = [[],[]]
static var Card_卡槽背景板贴图 : Array[Array] = [[],[]]
static var Card_空卡槽贴图 : Array[Array] = [[],[]]

var add_Card_添加空卡槽贴图 : Texture2D = preload("uid://ddqjnx1bcbukx")
var add_Card_添加卡槽背景板贴图 : Texture2D = preload("uid://dpri4otf6rdve")
var add_Card_添加卡槽边框贴图 : Texture2D = preload("uid://28o38kfo56m8")
var add_Card_添加空卡槽贴图pe : Texture2D
var add_Card_添加卡槽背景板贴图pe : Texture2D
var add_Card_添加卡槽边框贴图pe : Texture2D

var add_Card_数组添加向量 : Vector2 = Vector2(70,80)
var add_Card_数组添加缩放 : Vector2 = Vector2(2,2)
var add_Card_数组添加场景 : PackedScene
var add_Card_数组添加名称 : String = " "
var add_Card_添加内部名称 : String = " "
var add_Card_数组添加描述 : String = " "
var add_Card_数组添加消耗 : int = 0
var add_Card_数组添加冷却 : float = 7.5
var add_Card_数组添加开局冷却减免百分比 = 0
var add_Card_添加附加属性 : Array
var add_Card_添加附加属性名称 : Array[String]

func _ready() -> void:
	初始化消耗组()
	打包卡槽贴图组()
#region 初始化数组2
	生成怪物数组()
#endregion
func 清理临时数据():
	add_Card_数组添加描述 = ""
	add_Card_数组添加名称 = ""
	add_Card_数组添加向量 = Vector2(70,80)
	add_Card_数组添加缩放 = Vector2(2,2)
	add_Card_数组添加场景 = preload("res://Test/Error.tscn")
	add_Card_数组添加冷却 = 7.5
	add_Card_数组添加消耗 = 0
	add_Card_添加内部名称 = "Null:Object"

func 打包生成数组():
	Card_Vector.append(add_Card_数组添加向量)
	Card_Pack.append(add_Card_数组添加场景)
	Card_Scale.append(add_Card_数组添加缩放)
	Card_名称.append(add_Card_数组添加名称)
	Card_描述.append(add_Card_数组添加描述)
	Card_冷却.append(add_Card_数组添加冷却)
	Card_消耗.append(add_Card_数组添加消耗)
	Card_开局冷却减免百分比.append(clamp(add_Card_数组添加开局冷却减免百分比,0,100))
	Card_附加属性.append(add_Card_添加附加属性)
	Card_附加属性名称.append(add_Card_添加附加属性名称)
	Card_内部名称.append(add_Card_添加内部名称)
	清理临时数据()

func 打包卡槽贴图组():
	Card_空卡槽贴图[0].append(add_Card_添加空卡槽贴图)
	Card_卡槽背景板贴图[0].append(add_Card_添加卡槽背景板贴图)
	Card_卡槽边框贴图[0].append(add_Card_添加卡槽边框贴图)
	Card_空卡槽贴图[1].append(add_Card_添加空卡槽贴图pe)
	Card_卡槽背景板贴图[1].append(add_Card_添加卡槽背景板贴图pe)
	Card_卡槽边框贴图[1].append(add_Card_添加卡槽边框贴图pe)
	add_Card_添加空卡槽贴图 = preload("uid://ddqjnx1bcbukx")
	add_Card_添加卡槽背景板贴图 = preload("uid://dpri4otf6rdve")
	add_Card_添加卡槽边框贴图 = preload("uid://28o38kfo56m8")
	
func 返回贴图组(贴图组ID:int,设备:int):
	设备 = clamp(设备,0,1)
	var 返回值 : Array[Texture2D] 
	返回值.append(Card_空卡槽贴图[设备][贴图组ID])
	返回值.append(Card_卡槽背景板贴图[设备][贴图组ID])
	返回值.append(Card_卡槽边框贴图[设备][贴图组ID])
	return 返回值

func 生成怪物数组(地此:StringName = "僵尸.tscn"):
	var s = 地此
	怪物数组.append( load("res://Object/怪物/"+地此))

func 生成物体贴图(ID:int):
	var qx : PackedScene = Card_Pack[ID]
	var qxi = qx.instantiate()
	qxi.remove_from_group("UI判定")
	qxi.remove_from_group("判定")
	qxi.remove_from_group("射弹")
	qxi.remove_from_group("器械")
	qxi.remove_from_group("怪物")
	qxi.启用 = false
	qxi.position = Card_Vector[ID]
	qxi.scale = Card_Scale[ID]
	return qxi

func 初始化消耗组():
	#region 初始化数组
	add_Card_数组添加向量 = Vector2(85,190)
	add_Card_数组添加缩放  = Vector2(2,2)
	add_Card_数组添加场景 = preload("res://Object/怪物/僵尸.tscn")
	add_Card_数组添加名称 = "僵尸"
	add_Card_数组添加描述 = "一只普通的小僵尸"
	add_Card_数组添加消耗 = 50
	add_Card_数组添加冷却 = 0
	add_Card_添加内部名称 = "MVZ2:Zombies"
	打包生成数组()
	add_Card_数组添加向量 += Vector2(0,10)
	add_Card_数组添加场景 = preload("res://Object/器械/发射器.tscn")
	add_Card_数组添加名称 = "发射器"
	add_Card_数组添加描述 = "平平无奇"
	add_Card_添加内部名称 = "MVZ2:Transmitter"
	add_Card_数组添加消耗 = 100
	打包生成数组()
	add_Card_数组添加消耗 = 0
	add_Card_数组添加冷却 = 0
	add_Card_数组添加场景 = preload("res://Object/器械/熔炉.tscn")
	add_Card_数组添加名称 = "熔炉"
	add_Card_数组添加描述 = "每间隔一段时间产红石"
	add_Card_添加内部名称 = "MVZ2:Furnace"
	打包生成数组()
	add_Card_数组添加向量 += Vector2(0,10)
	add_Card_数组添加消耗 = 50
	add_Card_数组添加冷却 = 15
	add_Card_数组添加名称 = "黑曜石"
	add_Card_数组添加描述 = "在1.16之前原版生存中公认，最结实的方块（正数）"
	add_Card_添加内部名称 = "MVZ2:Obsidian"
	add_Card_数组添加场景 = preload("res://Object/器械/黑曜石.tscn")
	打包生成数组()
	add_Card_数组添加向量 += Vector2(0,10)
	add_Card_数组添加消耗 = 25
	add_Card_数组添加冷却 = 01
	add_Card_数组添加名称 = "地雷"
	add_Card_添加内部名称 = "MVZ2:Landmine"
	add_Card_数组添加描述 = "经过一段潜伏期后探出"
	add_Card_数组添加场景 = preload("res://Object/器械/地雷.tscn")
	打包生成数组()
	add_Card_数组添加向量 += Vector2(0,50)
	add_Card_添加内部名称 = "City:HuanChon_152"
	add_Card_数组添加名称 = "缓冲发射器"
	add_Card_数组添加消耗 = 175
	add_Card_数组添加冷却 = 7.5
	add_Card_数组添加描述 = "投稿器械：似有故人之姿。
以憎恶与悔恨之名！傲慢的发射器，闪亮登场！"#"投稿器械：似有故人之姿。
#缓冲发射器喜欢赤蛮奇，尤其是她的高冷与傲慢的气场！这就是为什么他喜欢穿成赤蛮奇的样子！
#为了表达自己对赤蛮奇的喜爱之情，特地向三头发射器请教了分头术。尽管他对于学习有着比较高的天赋，但仍然在没有对事物有热情的情况下非常懒惰，比如拥有死亡回归的能力，在轮回中无限怠惰之类的（？

#“……话说你是给吗？”
#“没有啦！为啥！你会这么想啊！！！”"
	add_Card_数组添加场景 = preload("res://Object/器械/投稿器械/缓冲发射器.tscn")
	打包生成数组()
	
#endregion
