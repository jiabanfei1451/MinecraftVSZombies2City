extends AudioStreamPlayer
class_name 音频引擎
@export_enum("音乐","音效") var 选项 : String = "音乐"
@export var 可调用音乐引擎 : bool = false
@export var 自动播放 : bool = true
var jv : int = -1
var misc : float = 0
@export_enum("Null:-1","万圣夜:0",
"选卡:1",
"主界面:2",
"灾变行者:3",
"双重麻烦:4",
"双重麻烦-BlackOut:5",
"零号病患Old plus:6",
"零号病患Old:7",
"零号病患:8") var 音乐选项 : int = -1
# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	finished.connect(自动重播)
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	if 选项 == "音乐":
		if 音乐选项 > -1:
			if 音乐列表.音乐音量 != 0:
				volume_db = -25 + 音乐列表.音乐音量 / 4
			else:volume_db = -100
			if not playing:
				if 自动播放 == true:
					play(0)
			if 可调用音乐引擎 == true and jv != 音乐选项:
				if 音乐选项 < 音乐列表.音乐列表.size():
					stream = 音乐列表.音乐列表[音乐选项]
					jv = 音乐选项
		else:
			stop()
	if 选项 == "音效":
		if 音乐列表.音效音量 != 0:
			volume_db = -25 + 音乐列表.音效音量 / 4
		else:volume_db = -100
func 自动重播():
	if 选项 == "音乐":
		play(0)
