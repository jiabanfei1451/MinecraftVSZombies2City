extends Control
var id : int = 0
func _ready() -> void:
	await get_tree().create_timer(0.3).timeout
	$"..".scale.x = 0
	$"..".visible = true
func _process(delta: float) -> void:
	var b = $"../JsonData".res_load_data("res://2/data/Muisc Data/","Music list")
	$"名称".text = b.data[str(id)].名称
	$"出处".text = "出处:" + b.data[str(id)].出处
	$"出处2".text = b.data[str(id)].讲述
