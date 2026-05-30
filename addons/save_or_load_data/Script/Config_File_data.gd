@tool
@icon("uid://csusj4vdseyph")
## 适用于config的存档
class_name ConfigFileData
extends Data
var config = ConfigFile.new()

func add_Data(name:String = "node",keydata_name:String = "node",array:Array = []):
	dataname.append(name)
	keydataname.append(keydata_name)
	if array.size() > 1:
		data.append(array)
	else:
		data.append(array[0])
	print(data)
func save_Data(path:String = "user://",pathname:String = "data",passworld:bool = false):
	if dataname.size() > 0:
		for i in data.size():
			var name : String = dataname[i]
			var keyname : String = keydataname[i]
			config.set_value(name,keyname,data[i])
		await get_tree().create_timer(1).timeout
		config.save(ProjectSettings.globalize_path(path+pathname+".cfg"))
