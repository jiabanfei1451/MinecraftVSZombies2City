extends CanvasLayer
var key = "asd4156asd456as4d6sad"
var 临时数据表 : ConfigFile

func 获取Json内容(语言文件:String = "Level_Text.json",语言:int = 0,加载条目:int = 0):
	var text :JSON
	if 语言 == 0:
		text = load("res://2/Text/Zh_CN/" + 语言文件)
		return text.data.Text[加载条目]
	else:
		text = load("res://2/Text/EN_US/" + 语言文件)
		return text.data.Text[加载条目]
