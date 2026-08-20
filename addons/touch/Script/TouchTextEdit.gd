@tool
@icon("uid://bp7hd1u0j7wg5")
## 适用于手机端用户的文本输入事件
extends Touchpad
class_name TouchTextEdit
@export var 文本 : String = ""
@export var 占位符 : String = "Text..."
@export_enum("XY:0","X:1","Y:2","Not:3") var 拖拽方向 : int = 0
@export var 滚动速率 : Vector2 = Vector2(0.5,0.25)
@export_group("物体状态")
@export var edit : TextEdit
@export var Ui : Control
@export var Object_array : Array[Control]
func _ready() -> void:
	初始化2()
func _process(delta: float) -> void:
	P3(delta)
func 焦点():
	edit.grab_focus(true)
func 失去焦点():
	edit.grab_focus(false)
	print(edit.get_focus_mode_with_override())
func _on_拖拽时(event: InputEventScreenDrag, 控制器: Touchpad) -> void:
	if 拖拽方向 == 2:
		edit.scroll_vertical -= event.relative.y * 滚动速率.y * scale.y
	elif 拖拽方向 == 1:
		edit.scroll_horizontal -= event.relative.x * 滚动速率.x * scale.x
	elif 拖拽方向 == 0:
		edit.scroll_horizontal -= event.relative.x * 滚动速率.x * scale.x
		edit.scroll_vertical -= event.relative.y * 滚动速率.y * scale.y

func 初始化2() -> void:
	初始化()
	print(OS.get_name())
	if OS.get_name() == "Windows":
		if Rect != null:
			Rect.queue_free()
	var Textnode : Control = Control.new()
	add_child(Textnode)
	Object_array.append(Textnode)
	Ui = Textnode
	Ui.clip_contents = true
	
	var Textedit : TextEdit = TextEdit.new()
	Textnode.add_child(Textedit)
	Textedit.modulate = Color(0,0,0,0)
	Object_array.append(Textedit)
	edit = Textedit
	按下时void.connect(焦点)
	外部抬起时void.connect(失去焦点)
	拖拽时.connect(_on_拖拽时)
	print("s")
func P3(delta:float) -> void:
	p2(delta)
	if Game_Ready.OSNAME == "Android":
		edit.set_caret_line(999)
		edit.set_caret_column(999)
	if Ui != null:
		Ui.position = Vector2(0,0)
		Ui.size = size
	if edit != null:
		文本 = edit.text
		edit.placeholder_text = 占位符
		edit.size = size
		edit.modulate = Color(1.0, 1.0, 1.0, 1.0)
