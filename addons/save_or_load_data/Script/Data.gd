## 空白的数据类
@tool
@icon("uid://cgdeowkohagco")
class_name Data
extends Node
@export var data : Array = []
@export var keydataname : Array[String] = []
@export var dataname : Array[String] = []

func create_dir(name:String,path:String = "user://"):
	var s = DirAccess.open(ProjectSettings.globalize_path(path))
	if s != null:
		s.make_dir_absolute(name)
