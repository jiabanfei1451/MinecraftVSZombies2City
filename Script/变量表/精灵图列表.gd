extends Node
static var 怪物数组 : Array[PackedScene]

static var img : Array[Texture2D]
static var Pack : Array[PackedScene]
static var 名称 : Array[String]
static var 描述 : Array[String]
static var 冷却 : Array[float]
static var 消耗 : Array[int]

static var 卡槽边框贴图 : Array[Texture2D]
static var 卡槽背景板贴图 : Array[Texture2D]
static var 空卡槽贴图 : Array[Texture2D]

var 添加空卡槽贴图 : Texture2D = preload("uid://ddqjnx1bcbukx")
var 添加卡槽背景板贴图 : Texture2D = preload("uid://dpri4otf6rdve")
var 添加卡槽边框贴图 : Texture2D = preload("uid://28o38kfo56m8")
var 添加空卡槽贴图pe : Texture2D
var 添加卡槽背景板贴图pe : Texture2D
var 添加卡槽边框贴图pe : Texture2D

var 数组添加图片 : Texture2D
var 数组添加场景 : PackedScene
var 数组添加名称 : String = " "
var 数组添加描述 : String = " "
var 数组添加消耗 : int = 0
var 数组添加冷却 : float = 7.5


func _ready() -> void:
#region 初始化数组
	数组添加图片 = preload("res://Object/素材图/精灵图/僵尸.png")
	数组添加场景 = preload("res://Object/怪物/僵尸.tscn")
	数组添加名称 = "僵尸"
	数组添加描述 = "一只普通的小僵尸"
	数组添加消耗 = 50
	数组添加冷却 = 0
	打包生成数组()
	数组添加图片 = preload("res://Object/素材图/精灵图/发射器.png")
	数组添加场景 = preload("res://Object/器械/发射器.tscn")
	数组添加名称 = "发射器"
	数组添加描述 = "平平无奇"
	数组添加消耗 = 100
	打包生成数组()
	数组添加消耗 = 50
	数组添加图片 = preload("res://Object/素材图/精灵图/熔炉.png")
	数组添加场景 = preload("res://Object/器械/熔炉.tscn")
	数组添加名称 = "熔炉"
	数组添加描述 = "每间隔一段时间产红石"
	打包生成数组()
	数组添加消耗 = 50
	数组添加冷却 = 15
	数组添加名称 = "黑曜石"
	数组添加描述 = "在1.16之前原版生存中公认，最结实的方块"
	数组添加图片 = preload("res://Object/素材图/精灵图/黑曜石.png")
	数组添加场景 = preload("res://Object/器械/黑曜石.tscn")
	打包生成数组()
	数组添加消耗 = 25
	数组添加冷却 = 01
	数组添加名称 = "地雷"
	数组添加描述 = "经过一段潜伏期后探出"
	数组添加图片 = preload("res://Object/素材图/精灵图/地雷.png")
	数组添加场景 = preload("res://Object/器械/地雷.tscn")
	打包生成数组()
#endregion
	打包卡槽贴图组()
#region 初始化数组2
	生成怪物数组()
#endregion
func 清理临时数据():
	数组添加描述 = ""
	数组添加名称 = ""
	数组添加图片 = preload("res://UI/UI素材图/Error.png")
	数组添加场景 = preload("res://Test/Error.tscn")
	数组添加冷却 = 7.5
	数组添加消耗 = 0

func 打包生成数组():
	img.append(数组添加图片)
	Pack.append(数组添加场景)
	名称.append(数组添加名称)
	描述.append(数组添加描述)
	冷却.append(数组添加冷却)
	消耗.append(数组添加消耗)
	清理临时数据()

func 打包卡槽贴图组():
	空卡槽贴图.append(添加空卡槽贴图)
	卡槽背景板贴图.append(添加卡槽背景板贴图)
	卡槽边框贴图.append(添加卡槽边框贴图)
	空卡槽贴图.append(添加空卡槽贴图pe)
	卡槽背景板贴图.append(添加卡槽背景板贴图pe)
	卡槽边框贴图.append(添加卡槽边框贴图pe)
	添加空卡槽贴图 = preload("uid://ddqjnx1bcbukx")
	添加卡槽背景板贴图 = preload("uid://dpri4otf6rdve")
	添加卡槽边框贴图 = preload("uid://28o38kfo56m8")
	
func 返回贴图组(贴图组ID:int):
	var 返回值 : Array[Texture2D] 
	返回值.append(空卡槽贴图[贴图组ID])
	返回值.append(卡槽背景板贴图[贴图组ID])
	返回值.append(卡槽边框贴图[贴图组ID])
	return 返回值

func 生成怪物数组(地此:StringName = "僵尸.tscn"):
	var s = 地此
	怪物数组.append( load("res://Object/怪物/"+地此))
