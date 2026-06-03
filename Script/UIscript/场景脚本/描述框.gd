extends Control
@export_node_path("Control","ColorRect") var 父物体
var fe : bool = false
var 过度速度 : float = 0.1
# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	position = Vector2((152 - size.x) / 2,220)


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(_delta: float) -> void:
	if 全局变量.卡槽提示词动画 == true:
		过度速度 = 0.1
	else:
		过度速度 = 0
	if fe == false:
		var twe = create_tween()
		twe.tween_property($".","size",$"排列节点".size + Vector2(10,10),过度速度)
		twe.tween_property($".","position",Vector2((152 - size.x) / 2 ,220) * 1,过度速度)

func fr():
	fe = true
	var twe = create_tween()
	twe.tween_property($".","size",Vector2(0,0),过度速度 * 5).set_trans(Tween.TRANS_CUBIC)
	await twe.finished
	queue_free()
