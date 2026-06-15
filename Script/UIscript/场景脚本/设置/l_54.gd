extends "res://Script/UIscript/场景脚本/展开选项卡.gd"
func _ready() -> void:
	R2()
	$"显示选项".color = 收起颜色
	点击时void.connect(展开)
	for i in get_children():
		if 选项物体 != null:
			if i is VBoxContainer:
				选项 = i
	for i in $"选项".get_children():
		i.启用 = false
func _process(delta: float) -> void:
	P3(delta)
	create_tween().tween_property($".","初始大小:x",$"..".size.x,1)
	size.x = $"..".size.x
	$Label.text = Text
	if 初始大小.x == 0 :
		初始大小 = size
