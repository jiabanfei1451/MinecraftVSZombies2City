extends Touchbutton

func _ready() -> void:
	初始化()
	点击时void.connect(Back)
func Back():
	get_tree().change_scene_to_file("res://Scene/主菜单.tscn")
