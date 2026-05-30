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
	var array : Array[Area2D] = 父节点.已检测
	if area.is_in_group("器械"):
		area.减少血量(1,$".")
		print(area)
		if array.has(area) == false:
			print(area)
			array.append(area)
		父节点.扫描数组()

func _on_area_exited(area: Area2D) -> void:
	var array : Array[Area2D] = 父节点.已检测
	if array.has(area) == false:
		print(area)
		array.erase(area)
		父节点.扫描数组()
