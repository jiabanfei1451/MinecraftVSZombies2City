extends Area2D
@export var 速度 : Vector2
var 击中数量 : int = 0
# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	生成日志("生成")

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _physics_process(delta: float) -> void:
	position += 速度 * 60 * delta
	if position.x >= 689:
		queue_free()


func _on_area_entered(area: Area2D) -> void:
	if area.is_in_group("怪物") == true:
		if not area.is_in_group("判定"):
			if area.是否死亡 == false and 击中数量 < 1:
				击中数量 += 1
				area.减少血量(1)
				queue_free()

func 生成日志(日志: String):
	var dataTime : String = Time.get_datetime_string_from_system()
	if get_tree().current_scene.DEBUG == true:
		print("[",dataTime,"]",name,日志)
