extends CanvasLayer
@export var 选定器械 : PackedScene

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	asdd()


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass
func asdd():
	$"卡槽".position.y -= 150
	var ss = create_tween()
	ss.tween_property($"卡槽","position",$"卡槽".position + Vector2(0,150),1).set_trans(Tween.TRANS_EXPO)
