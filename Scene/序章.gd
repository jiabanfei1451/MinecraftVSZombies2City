extends "res://Script/组件/场景组件/怪物生成.gd"
func _ready() -> void:
	level_ready2()
	生成草坪()
	添加波(5,[0],true)
func _process(delta: float) -> void:
	level_process2(delta)

func 生成草坪():
	var cx : int = 0
	var cy : int = 0
	for d in 9:
		for i in 5:
			var 草 = preload("res://Object/可互动/草坪.tscn")
			var p = 草.instantiate()
			p.坐标 = Vector2(cx+0.18,cy)
			p.是否自动更改 = "是"
			cy += 1
			get_node("草坪").add_child(p)
		cx += 1
		cy = 0


func _on_选卡完成() -> void:
	生成开始()
