extends TouchColorRectButton
@export var Text : String
@export var ID : String = "0"
@export var 底座颜色 : Color = Color(1.0, 1.0, 1.0, 1.0)
func _ready() -> void:
	R2()
	点击时void.connect(dianji)
func _process(delta: float) -> void:
	P3(delta)
	$Label.text = Text
	$"显示选项".color = 底座颜色
func dianji():
	$"../../../../../AudioStreamPlayer".play()
	$"../../../../../窗口背景".开启ID = ID
