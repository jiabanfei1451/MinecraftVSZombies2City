extends CanvasLayer
var key = "asd4156asd456as4d6sad"
var 临时数据表 : ConfigFile
func _ready() -> void:
	创建文件夹("set")
	
func create_数据():
	var save = ConfigFile.new()
	临时数据表 = save
	return save
func add_数据(组:String,数据:String,表:Array):
	if 临时数据表 != null:
		临时数据表.set_value(组,数据,表)
	else:
		printerr("无法读取表格 错误代码 -2")
func save_数据(地此:String):
	if 临时数据表 != null:
		临时数据表.save("user://" + 地此)
		临时数据表 = null
	else:
		printerr("无法读取表格 错误代码 -2")
func load_数据(地此:String = "config.cfg:",组:String = "null",数据:String= "null",模式:int = 0):
	var load = ConfigFile.new()
	var err = load.load("user://"+地此)
	if 模式 == 0:
		if err == OK:
			return load.get_value(组,数据)
		else:
			printerr("无法读取文件 错误代码 -1")
			return null
	else:
		print(err)
		return null
func give_数据(地此:String = "config.cfg",组:Array = [],nam:StringName = "null"):
	var loa = ConfigFile.new()
	var load = loa.load("user://" + 地此)
	if load == OK:
		if loa.get_value(组[1],组[2]) != null:
			组[0].nam = loa.get_value(组[1],组[2])[0]
			print(loa.get_value(组[1],组[2]))

func 获取Json内容(语言文件:String = "Level_Text.json",语言:int = 0,加载条目:int = 0):
	var text :JSON
	if 语言 == 0:
		text = load("res://2/Text/Zh_CN/" + 语言文件)
		return text.data.Text[加载条目]
	else:
		text = load("res://2/Text/EN_US/" + 语言文件)
		return text.data.Text[加载条目]
func create_game_folder(folder_path: String) -> bool:
	# 1. 获取目录访问器（使用用户数据目录，这是推荐的安全路径）
	# user:// 是 Godot 的特殊路径，表示游戏的用户数据目录（不同平台路径不同）
	# 如果要创建到游戏安装目录，可使用 res://，但注意：部分平台（如 Windows）安装目录可能无写入权限
	var dir = DirAccess.open("user://")
	if dir == null:
		print("错误：无法访问用户数据目录！")
		return false
	
	# 2. 拼接完整路径并创建文件夹（make_dir_recursive 会自动创建多级目录，比如 a/b/c）
	var full_path = "user://" + folder_path
	if not dir.file_exists(full_path):  # 先检查文件夹是否已存在，避免重复创建
		dir.make_dir_recursive(full_path)
		print("文件夹创建成功：", full_path)
		return true
	else:
		print("文件夹已存在：", full_path)
		return true
func playLoad(路径:String) -> void:
	var config = ConfigFile.new()
	var err = config.load_encrypted_pass("user://"+路径,key)
	if err == OK:
		pass
	else:
		printerr("无法读写原因：未识别到文件夹")

func 创建文件夹(folder_path: String) -> bool:
	# 1. 获取目录访问器（使用用户数据目录，这是推荐的安全路径）
	# user:// 是 Godot 的特殊路径，表示游戏的用户数据目录（不同平台路径不同）
	# 如果要创建到游戏安装目录，可使用 res://，但注意：部分平台（如 Windows）安装目录可能无写入权限
	var dir = DirAccess.open("user://")
	if dir == null:
		printerr("错误：无法访问用户数据目录！")
		return false
	
	# 2. 拼接完整路径并创建文件夹（make_dir_recursive 会自动创建多级目录，比如 a/b/c）
	var full_path = "user://" + folder_path
	if not dir.file_exists(full_path):  # 先检查文件夹是否已存在，避免重复创建
		dir.make_dir_recursive(full_path)
		print("文件夹创建成功：", full_path)
		return true
	else:
		printerr("文件夹已存在：", full_path)
		return true
