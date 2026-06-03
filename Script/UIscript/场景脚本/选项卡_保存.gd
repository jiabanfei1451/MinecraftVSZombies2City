extends TouchColorRectButton

func _ready() -> void:
	R2()
	点击时void.connect(dian)
func dian():
	$"../AudioStreamPlayer".play()
	设置存储.add_Data("Window","Window_Scale",[全局变量.窗口缩放])
	设置存储.add_Data("Window","Window_size_mode",[全局变量.窗口拉伸模式])
	设置存储.add_Data("Window","Window_mode",[全局变量.窗口模式])
	设置存储.add_Data("Game","card_admin",[true])
	设置存储.save_Data("user://","settings")
	$"../ColorRect".visible=true
	await get_tree().create_timer(1).timeout
	$"../ColorRect".visible=false
