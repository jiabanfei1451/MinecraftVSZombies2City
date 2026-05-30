extends Control
var jd : bool = false

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	var tw = create_tween()
	tw.tween_property($".","position",position - Vector2(0,40),0.5).set_trans(Tween.TRANS_SINE)
	tw.tween_property($".","position",position + Vector2(randf_range(20,-20),0),0.5).set_trans(Tween.TRANS_BACK)


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	if jd == true:
		if Input.is_mouse_button_pressed(MOUSE_BUTTON_LEFT):
			queue_free()
			get_tree().current_scene.器械能 += 25


func _on_focus_entered() -> void:
	jd = true


func _on_focus_exited() -> void:
	jd = false
