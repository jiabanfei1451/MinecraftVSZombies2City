extends Control
func  _ready() -> void:
	$".".position.y = 676
	create_tween().tween_property($".","position:y",324,1).set_trans(Tween.TRANS_CUBIC)
