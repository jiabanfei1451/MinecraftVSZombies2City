@icon("uid://bvgyfyblhy6wr")
@tool
class_name touchBar
extends Panel
signal 滑动按下时(name:String)
signal 值变化时(name:String)
signal 滑动结束时(name:String)
signal 滑动按下时void
signal 值变化时void
signal 滑动结束时void
@export_group("Bar")
@export var 乘数 : float = 0.75
@export var 乘数校准 : Vector2 = Vector2(2,2)
@export_subgroup("Value")
@export var value : float
@export var stop : float = 1
@export var maxvalue : float = 100
@export var minvalue : float = 0
@export_subgroup("Theme")
@export var 滑块偏移 : Vector2
@export var 临界值偏移 : Vector2
@export var 滑块大小 : Vector2 = Vector2(40,40)
@export var eme : Theme = preload("uid://dvka2rktyelip")
@export var Valuecolor : Color = Color(1.0, 1.0, 1.0, 0.0)
@export_group("物体状态")
@export var 实例化场景状态 : Panel
@export var 滑块 : Panel
@export var 进度条颜色 : ColorRect
var 当前值 : float
func _ready() -> void:
	当前值 = value
	var huakuai : PackedScene = preload("uid://ceom1v7ilwyw3")
	var huakuaii = huakuai.instantiate()
	add_child(huakuaii)
	huakuaii.get_child(0).父级 = $"."
	进度条颜色 = ColorRect.new()
	进度条颜色.z_index = 0
	滑块 = get_child(0)
	滑块.get_child(0).按下时给予焦点给 = $"."
	滑块.add_theme_stylebox_override("panel",eme.get_stylebox("Bottom","TouchBar"))
	滑块.name = "滑块"
	进度条颜色.name = "值颜色"
	add_child(进度条颜色)
func _process(delta: float) -> void:
	
	if value != 当前值:
		emit_signal("值变化时",name)
		emit_signal("值变化时void")
		当前值 = value
	if 进度条颜色 != null:
		进度条颜色.color = Valuecolor
		进度条颜色.size= Vector2(((value - minvalue) / (maxvalue - minvalue)) * size.x,size.y)
	if 滑块 != null:
		滑块.size = 滑块大小
	if value > maxvalue:
		value = maxvalue
	elif value < minvalue:
		value = minvalue
