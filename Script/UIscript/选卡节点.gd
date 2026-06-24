extends Control
var X : int = 0
var Y : int = 0

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	await get_tree().create_timer(2).timeout
	$"节点/Label".visible = false
	生成卡槽()
	生成卡槽(1)
	生成卡槽(2)
	生成卡槽(3)
	生成卡槽(4)
	for s in 35:
		生成卡槽(randi_range(0,5))
	

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	for i in get_child_count():
		get_children()[i]
func 生成卡槽(器械ID:int = 0):
	var 卡槽 : PackedScene = preload("res://UI/card/卡槽.tscn")
	var 实列 = 卡槽.instantiate()
	实列.调用功能 = 1
	实列.器械ID = 器械ID
	实列.pos = Vector2(0 + (144 * 0.5 * X),0 + (215.5 * 0.5 * Y))
	实列.scale = Vector2(0.5,0.5)
	$"节点".add_child(实列)
	X += 1
	if X > 7:
		Y += 1
		X = 0
