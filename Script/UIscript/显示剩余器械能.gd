extends Label
var qxn = 0

# Called when the node enters the scene tree for the first time.

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	var tw = create_tween()
	tw.tween_property($".","qxn",int(get_tree().current_scene.器械能+0.1),0.05)
	text = str(round(qxn))
