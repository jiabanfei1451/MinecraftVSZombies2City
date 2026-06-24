extends Node
@export var 分辨率 : Vector2i = Vector2i(1152,648)
@export var 版本 : StringName = "beta0.1.1"
@export var OSNAME : String
@export var 标题提示语 : Array[String] = [
"看什么？",
"快去试试《Terraria》吧！",
"本作是Cuerzor制作的《MVZ2》的同人游戏",
"给这个同人作者打赏？认真的？",
"知道吗？该版本之前还只是创游上的小游戏而已",
"神权",
"(DEBUG)",
"/kill @e[name=!steve]",
"",
"幽匿天下！！！",
"Fabric",
"NeoFoger",
"Foger",
"DIE",
"有只僵尸在你的草评上",
"如有器械侵权？请反馈！",
OS.get_user_data_dir(),
"MVM vs MVZ = MMVMZ",
"1+1=3",
str(Time.get_time_string_from_system()),
"我把你户口开了你的IP是：" + "127.0.0.1",
"你去告诉加班费，说他忘记写窗口名了：）",
]
func _ready() -> void:
	有等待的ready()
	var s = FileAccess.open("user://778.json",FileAccess.WRITE_READ)
	s.store_string('{
"Level_name":"绝命矿坑",
"Level_Day":"第一天",
"Level_BGM":3,
"Level_monster":[[0.0], [0.0], [0.0], [0.0], [0.0]],
"Level_monster_Value":[1, 1, 2, 3, 5],
"Level_Wave":[false, false, false, false, true],
"Level_image":"",
"Level_create_version":"beta0.1.2",
}')
	OSNAME = OS.get_name()
func _input(event: InputEvent) -> void:
	if event.is_released() and event.as_text() == "P":
		重置标题语()

func 重置标题语(Title:String = ""):
	if Title == "":
		if OS.get_name() == "Windows":
			get_window().title = "MinecraftVSZombies2-City " + ProjectSettings.get_setting("application/config/version") + " " + 标题提示语.pick_random()
	else:
		if OS.get_name() == "Windows":
			get_window().title = "MinecraftVSZombies2-City " + ProjectSettings.get_setting("application/config/version") + " " + Title
func 生成日志(日志: Variant):
	var dataTime : String = Time.get_datetime_string_from_system()
	print("[",dataTime,"]",日志)

func 有等待的ready():
	get_window().title = "Loader... /---"
	await get_tree().create_timer(0.2).timeout
	get_window().title = "Loader... /*--"
	await get_tree().create_timer(0.2).timeout
	get_window().title = "Loader... //*-"
	await get_tree().create_timer(0.2).timeout
	get_window().title = "Loader... ///*"
	await get_tree().create_timer(0.2).timeout
	get_window().title = "Loader... ////"
	await get_tree().create_timer(0.2).timeout
	重置标题语()
