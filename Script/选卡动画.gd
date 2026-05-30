extends AnimationPlayer


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass

func _on_animation_finished(anim_name: StringName) -> void:
	if anim_name == "选卡":
		var 场景 : PackedScene = preload("res://UI/选卡UI.tscn")
		var 实例化 = 场景.instantiate()
		get_tree().current_scene.add_child(实例化)
