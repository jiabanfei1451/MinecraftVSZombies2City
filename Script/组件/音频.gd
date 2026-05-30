extends AudioStreamPlayer
@export_enum("音乐","音效") var 选项 : String = "音乐"
@export var 可调用音乐引擎 : bool = false
var jv : int = -1
var misc : float = 0
@export_enum("万圣夜:0",
"选卡:1",
"主界面:2",
"灾变行者:3") var 音乐选项 : int = 0
# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	if 选项 == "音乐":
		if 音量.音乐音量 != 0:
			volume_db = -25 + 音量.音乐音量 / 4
		else:volume_db = -100
		if not playing:
			play(0)
		if 可调用音乐引擎 == true and jv != 音乐选项:
			stream = 音量.音乐列表[音乐选项]
			jv = 音乐选项
	if 选项 == "音效":
		if 音量.音效音量 != 0:
			volume_db = -25 + 音量.音效音量 / 4
		else:volume_db = -100
