extends Control
var s : int
func _input(event: InputEvent) -> void:
	if event is InputEventScreenTouch:
		position = event.position - Vector2(30,40)
	if event is InputEventScreenDrag:
		position = event.position - Vector2(30,40)
	if event is InputEventMouseMotion:
		position = event.global_position - Vector2(30,40)
func _process(delta: float) -> void:
	var id : int = 精灵图列表.Card_Pack.find(get_tree().current_scene.当前器械)
	if id != -1:
		visible = true
		if id != s:
			for i in get_children():
				i.queue_free()
			s = id
			var ins = 精灵图列表.生成物体贴图(id)
			add_child(ins)
	else:
		visible = false
