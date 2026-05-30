extends Sprite2D
@export var 绑定物体 : Area2D
@export var 坐标偏移 : Vector2
@export var 缩放 : Vector2
@export var 完成绑定 : bool
# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	visible = false
	for i in 5:
		await get_tree().create_timer(0.2).timeout
	if 绑定物体 == null:
		queue_free()

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	if 绑定物体 != null:
		visible = true
		完成绑定 = true
		position = 绑定物体.position + 坐标偏移 - Vector2(0,(绑定物体.get_node("物理组件").高度.x + 绑定物体.get_node("物理组件").高度.y) *-1)
		scale = 缩放
	else:
		if 完成绑定 == true:
			queue_free()
