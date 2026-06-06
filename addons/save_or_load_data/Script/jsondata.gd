@icon("uid://d4bvjudktko6e")
## 适用于json的数据存储
class_name JsonData
extends Data
@export var FilePath : String = "user://"
@export var Filename : String = "node"
@export var echo : bool = true
var js : JSON = JSON.new()

func add_Data(name:String = "node",array:Array = []):
	dataname.append(name)
	if array.size() > 1:
		data.append(array)
	else:
		data.append(array[0])
## 以Json格式存储数据
func save_json(path:String = "null",name:String = "null"):
	# 你看你妈呢?
	if path == "null":
		path = FilePath
	if name == "null":
		name = Filename
	path = ProjectSettings.globalize_path(path) + name + ".json"
	# 不会想说AI闹麻了吧？
	var text : String = "{
	"
	# 这里是文件转换
	for i in data.size():
		if data[i] is String or data[i] is StringName:
			text += '"' + dataname[i] + '"' + ":" + '"' + str(data[i]) + '"' + ",
	"
		else:
			text += '"' + dataname[i] + '"' + ":" + str(data[i]) + ",
	"
	text += "
	}"
	text = text.replacen("(","[")
	text = text.replacen(")","]")
	var file = FileAccess.open(path,FileAccess.WRITE)
	file.store_string(text)
	data.clear()
	dataname.clear()
	keydataname.clear()
## 解析某路径的json
func load_data(path:String = "null",name:String = "null"):
	if path == "null":
		path = FilePath
	if name == "null":
		name = Filename
	path = ProjectSettings.globalize_path(path) + name + ".json"
	var file = FileAccess.open(path,FileAccess.READ) #创建变量
	var js : JSON = JSON.new()
	if file != null:
		if js.parse(file.get_as_text()) == OK: # 解析
			if echo == true:
				print("解析完成:",js.data)
			return js
		else:
			if echo == true:
				print("报错:",js.get_error_line())
			return null
	else:
		return null
## 嵌套数据
func datadata(name:Array[String],arrayData:Array[Array]):
	var data : String = "{}"
	for i in arrayData.size():
		var fordata
		var textarray = arrayData[i]
		var type
		if textarray != null:
			if textarray.size() > 1:
				fordata = textarray
			else:
				fordata = textarray[0]
		if fordata is String:
			type = '"' + str(fordata) + '"'
		elif fordata is Vector2 or fordata is Vector2i or fordata is Vector3 or fordata is Vector3i or fordata is Vector4 or fordata is Vector4i or fordata is Rect2 or fordata is Rect2i or fordata is Color:
			type = str(fordata).replace('(',"[")
			type = str(fordata).replace(']',"]")
		else:
			type = str(fordata)
		data = data.insert(1,'"' +name[i] + '"' + ' : ' + str(type))
		print(data)
	return data
	
	
	
	
	
	
	
