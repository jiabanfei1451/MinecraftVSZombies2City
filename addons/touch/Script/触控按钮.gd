@tool
@icon("uid://chmpegjqy4btn")
## 可触摸的超链接按钮
extends Touchbutton
class_name LinkTouchButton
@export var URL : String = "https://space.bilibili.com/3546884492757767"

func _ready() -> void:
	初始化()
	点击时void.connect(_on_点击时void)

func _on_点击时void() -> void:
	OS.shell_open(URL)
