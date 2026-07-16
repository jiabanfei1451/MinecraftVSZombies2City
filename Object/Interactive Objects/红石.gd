extends Node2D
var jd : bool = false

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	var tw = create_tween()
	tw.tween_property($".","position",position - Vector2(0,40),0.5).set_trans(Tween.TRANS_SINE)
	tw.tween_property($".","position",position + Vector2(randf_range(20,-20),0),0.5).set_trans(Tween.TRANS_BACK)
	$Touchbutton.点击时void.connect(red_Stone)
func red_Stone():
	queue_free()
	get_tree().current_scene.器械能 += 25
