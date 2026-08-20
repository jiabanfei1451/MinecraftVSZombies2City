extends CanvasLayer
@export var 选定器械 : PackedScene

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	asdd()


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	选定器械 = get_tree().current_scene.当前器械
	if 选定器械 != null:
		$"选定的卡槽".texture = 精灵图列表.img[get_tree().current_scene.当前器械ID]
		$"选定的卡槽".visible = true
		$"选定的卡槽".position = get_viewport().get_mouse_position() - Vector2(10,10)
	else:
		$"选定的卡槽".visible = false
func asdd():
	$"卡槽".position.y -= 150
	var ss = create_tween()
	ss.tween_property($"卡槽","position",$"卡槽".position + Vector2(0,150),1).set_trans(Tween.TRANS_EXPO)
