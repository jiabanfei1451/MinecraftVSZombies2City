extends Control

func _ready() -> void:
	$Panel.type = 设置存储.load_Data("Game","card_admin",[false])
	$TouchTextEdit.edit.text = str(设置存储.load_Data("Game","card_vertor2",[全局变量.选卡时镜头坐标]).x)
	$TouchTextEdit2.edit.text = str(设置存储.load_Data("Game","card_vertor2",[全局变量.选卡时镜头坐标]).y)
	$touchBar.value = 全局变量.游戏加速度
func _process(delta: float) -> void:
	$Label2.text = "游戏速度：" + str($touchBar.value)
	全局变量.选卡时镜头坐标 = Vector2(float($TouchTextEdit.文本),float($"TouchTextEdit2".文本))
	全局变量.卡槽提示词动画 = $Panel.type
	全局变量.游戏加速度 = $touchBar.value
