extends TouchColorRectButton

func _ready() -> void:
	R2()
	点击时void.connect(dian)
func dian():
	$"../AudioStreamPlayer".play()
	存档.create_数据()
	存档.add_数据("Window","Window_Scale",[全局变量.窗口缩放])
	存档.add_数据("Window","Window_size_mode",[全局变量.窗口拉伸模式])
	存档.add_数据("Window","Window_mode",[全局变量.窗口模式])
	存档.save_数据("Window-set.cfg")
	$"../ColorRect".visible=true
	await get_tree().create_timer(1).timeout
	$"../ColorRect".visible=false
