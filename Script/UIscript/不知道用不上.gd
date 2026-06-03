extends ColorRect
var 进入 : bool = false
var 鼠标向量 :Vector2
var 增加向量 : Vector2
var 物体向量 : Vector2
var 动画 : Tween
var d : Vector2

func _process(delta: float) -> void:
	var 动画 = create_tween()
	if Input.is_action_pressed("左键"):
		if 增加向量 != 鼠标向量 - get_viewport().get_mouse_position():
			增加向量 = 鼠标向量 - get_viewport().get_mouse_position()
			d = 增加向量
		else:
			d *= 0.75
		动画.tween_property($"../VBoxContainer","position",Vector2(0,Vector2(物体向量 + ((鼠标向量 - get_viewport().get_mouse_position())*-1)).y),0.2).set_trans(Tween.TRANS_SINE)
	elif Input.is_action_just_pressed("鼠标滚轮下"):
		动画.tween_property($"../VBoxContainer","position",Vector2(0,物体向量.y - 500),0.5)
	elif Input.is_action_just_pressed("鼠标滚轮上"):
		动画.tween_property($"../VBoxContainer","position",Vector2(0,物体向量.y + 500),0.5)
	else:
		物体向量 = $"../VBoxContainer".position
		鼠标向量 = get_viewport().get_mouse_position()
		动画.tween_property($"../VBoxContainer","position",Vector2(0,Vector2(物体向量 + d * -1).y),0.2).set_trans(Tween.TRANS_SINE)
		d *= 0.9
func _on_mouse_entered() -> void:
	进入 = true


func _on_mouse_exited() -> void:
	进入 = false
