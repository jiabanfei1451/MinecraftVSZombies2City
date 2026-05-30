extends Node
var 怪物数量 : int
var 射弹 : PackedScene = preload("uid://caymc0p7rsog")
# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	怪物数量 = $"../..".怪物数量
func 攻击():
	if $"../..".大招 == false:
		var 射弹2 = 射弹.instantiate() 
		if 怪物数量 > 0:
			$"../../动画".play("发射")
			射弹2.position = $"../..".position + Vector2(16,-13)
			节点提供变量.射弹.add_child(射弹2)
