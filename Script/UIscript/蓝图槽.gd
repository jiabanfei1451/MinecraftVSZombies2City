extends Control
@export var 显示 : Label
var 槽 : int = -1
func _process(delta: float) -> void:
	if 全局变量.卡槽数量 != 槽:
		for i in $"卡槽栏".get_children():
			i.queue_free()
		for i in 全局变量.卡槽数量:
			print(i)
			var Card : PackedScene = preload("uid://c2y62prxcbege")
			var Cardins : Object = Card.instantiate()
			$"卡槽栏".add_child(Cardins)
			设定卡槽(Cardins,i)
		槽 = 全局变量.卡槽数量
func 设定卡槽(卡槽:Object,id:int):
	var qv:Array[int] = get_tree().current_scene.已选卡
	if 卡槽 != null:
		卡槽.pos = Vector2(0,-0.802) + Vector2( (71.642 * id) * 2 ,0)
		卡槽.器械ID = qv[id]
		卡槽.提示文字 = 显示
		if id+1 != 10:
			卡槽.绑定快捷键 = str(id+1)
		else:
			卡槽.绑定快捷键 = "0"
