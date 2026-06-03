extends Button


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass


func _on_pressed() -> void:
	$AudioStreamPlayer.play()
	var s = create_tween()
	$"../..".完成选卡()
	mouse_filter = 2
	s.tween_property($"../../卡槽","position",$"../../卡槽".position - Vector2(800,0),1).set_trans(Tween.TRANS_EXPO)
