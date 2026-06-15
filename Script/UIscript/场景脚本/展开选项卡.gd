extends TouchColorRectButton
@export_group("Color")
@export var 收起颜色 : Color = Color(1.0, 1.0, 1.0, 1.0)
@export var 展开颜色 : Color = Color(0.599, 0.599, 0.599, 1.0)
@export_group("")
@export var 播放音频 : AudioStreamPlayer
@export var Text : String
@export var 选项 : VBoxContainer
@export var 选项物体 : Array
@export var 已展开 : bool = false
var 初始大小 : Vector2
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
	$Label.text = Text
	if 初始大小.x == 0 :
		初始大小 = size
func 展开():
	播放音频.play()
	var tween = create_tween()
	tween.set_parallel(true)
	if 已展开 == true:
		已展开 = false
		for i in $"选项".get_children():
			i.启用 = false
		tween.tween_property($".","size:y",初始大小.y,0.5).set_trans(Tween.TRANS_QUART)
		tween.tween_property($".","custom_minimum_size:y",45,0.4).set_trans(Tween.TRANS_SINE)
		tween.tween_property($"显示选项","color",收起颜色,0.5).set_trans(Tween.TRANS_QUART)
	else:
		已展开 = true
		for i in $"选项".get_children():
			i.启用 = true
		tween.tween_property($".","custom_minimum_size:y",初始大小.y + ($"显示选项".size.y * 2) + 选项.size.y,0.4).set_trans(Tween.TRANS_SINE)
		tween.tween_property($".","size:y",初始大小.y + 选项.size.y + ($"显示选项".size.y * 2),0.5).set_trans(Tween.TRANS_QUART)
		tween.tween_property($"显示选项","color",展开颜色,0.5).set_trans(Tween.TRANS_QUART)
