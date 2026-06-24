extends Area2D
var FPS : float
@export var 父节点 : Area2D
# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	FPS = delta

func 重启检测器():
	position += Vector2(9999,9999)
	await get_tree().create_timer(FPS).timeout
	position -= Vector2(9999,9999)


func _on_area_entered(area: Area2D) -> void:
	$".."._检测进入(area)


func _on_area_exited(area: Area2D) -> void:
	$".."._检测离开(area)
