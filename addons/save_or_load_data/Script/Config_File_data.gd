@tool
@icon("uid://csusj4vdseyph")
## 适用于config的存档
class_name ConfigFileData
extends Data
var saveing : bool = false
## 读取存档路径
@export var Dataname : String = "user://"
var config = ConfigFile.new()

func add_Data(name:String = "node",keydata_name:String = "node",array:Array = []):
	dataname.append(name)
	keydataname.append(keydata_name)
	if array.size() > 1:
		data.append(array)
	else:
		data.append(array[0])
	print(data)
func load_Data(name:String = "", key:String = "", Value:Array = [],filepath:String = Dataname):
	config.load(filepath)
	if config.load(filepath) == OK:
		var v
		if Value.size() > 0:
			if Value.size() < 2:
				v = Value[0]
			else:
				v = Value
		print(Dataname)
		print(config.get_value(name,key,v))
		if config.get_value(name,key,v) != null:
			print("not null")
			return config.get_value(name,key,v)
		else:
			print("is null")
			if v.size > 1:
				return v
			else:
				return v[0]
func save_Data(path:String = "user://",pathname:String = "data",passworld:bool = false):
	if saveing == false:
		saveing = true
		if dataname.size() > 0:
			for i in data.size():
				var name : String = dataname[i]
				var keyname : String = keydataname[i]
				config.set_value(name,keyname,data[i])
			config.save(ProjectSettings.globalize_path(path+pathname+".cfg"))
		saveing = false
		data.clear()
		dataname.clear()
		keydataname.clear()
	else:
		printerr("错误-2-当前节点已有其他的保存进程正在执行")
