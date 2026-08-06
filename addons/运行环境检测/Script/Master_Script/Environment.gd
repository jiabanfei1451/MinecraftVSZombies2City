extends Node

@export var Start_Environment : String = "GDScript"

func _ready() -> void:
	await  get_tree().create_timer(0.1).timeout
	print("Environment:" + Start_Environment)
