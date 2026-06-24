extends Node
var 音乐音量 : float = 100
var 音效音量 : float = 100
static var 音乐列表 : Array[Array] = [[],[]]
func _ready() -> void:
	AddMusic(load("res://Sound/音乐/万圣夜.ogg"),"万圣夜")
	AddMusic(load("res://Sound/音乐/选卡.ogg"),"选卡")
	AddMusic(load("res://Sound/音乐/主界面.ogg"),"主界面")
	AddMusic(load("res://Sound/音乐/灾变行者.ogg"),"灾变行者")
	AddMusic(load("res://Sound/音乐/double-trouble.ogg"),"双重麻烦")
	AddMusic(load("res://Sound/音乐/double-trouble-BlackOut.ogg"),"双重麻烦Blackout")
	AddMusic(load("res://Sound/音乐/旧版零号病患(Redemption).mp3"),"早期零号病患")
	AddMusic(load("uid://ditbwx5dihme0"),"老零号病患")
	AddMusic(load("uid://ctm51bn6ysilv"),"零号病患")
func AddMusic(Music:AudioStream = load("res://Sound/音乐/万圣夜.ogg"),NBT:String = ""):
	音乐列表[0].append(Music)
	音乐列表[1].append(NBT)
