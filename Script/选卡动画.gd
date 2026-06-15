@tool
extends Node2D
var pos : Vector2
var zoom : Vector2 = Vector2(1.1,1.1)
# Cal
func _ready() -> void:
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	var d = create_tween()
	d.tween_property($"摄像头","offset",pos,0.3)
	d.parallel().tween_property($"摄像头","zoom",zoom,0.3)
func 选卡():
	var s = create_tween().tween_property($".","pos",全局变量.选卡时镜头坐标,1)
	await s.finished
	var d = preload("res://UI/card/选卡UI.tscn")
	var di = d.instantiate()
	get_tree().current_scene.add_child(di)
func 完成选卡():
	var s = create_tween().tween_property($".","pos",全局变量.游戏中镜头坐标,1)
func 失败():
	var s = create_tween().tween_property($".","pos",全局变量.游戏失败镜头坐标,1)
