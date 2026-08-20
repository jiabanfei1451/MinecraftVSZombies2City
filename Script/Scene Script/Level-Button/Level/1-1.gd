extends "res://Script/Scene Script/Level-Button/Level-Button.gd"


func _on_加载关卡时() -> void:
	get_tree().change_scene_to_file("res://Scene/Level/序章.tscn")
