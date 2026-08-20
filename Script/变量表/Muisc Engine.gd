extends Node
var 音乐 : 音频引擎
var 二段音乐 : 音频引擎

func _ready() -> void:
	音乐 = 音频引擎.new()
	二段音乐 = 音频引擎.new()
	音乐.选项 = "音乐"
	二段音乐.选项 = "音乐"
	音乐.可调用音乐引擎 = true
	二段音乐.可调用音乐引擎 = true
	add_child(音乐)
	add_child(二段音乐)
