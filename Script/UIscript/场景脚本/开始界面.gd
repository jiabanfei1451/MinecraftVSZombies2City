extends CanvasLayer


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	$ColorRect.visible = true
	d()# Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass
	
func d():
	$"按钮".position.y += 200
	await get_tree().create_timer(1).timeout
	var dsss = create_tween()
	dsss.tween_property($ColorRect,"modulate",Color(0.0, 0.0, 0.0, 0.0),1).set_trans(Tween.TRANS_SINE)
	await get_tree().create_timer(0.6).timeout
	var ds = create_tween()
	ds.tween_property($"按钮","position",$"按钮".position - Vector2(0,200),1).set_trans(Tween.TRANS_SINE)
	$ColorRect.visible=false
	printerr("huh?")
