extends Node2D

func _ready() -> void:
	get_window().focus_exited.connect(失去焦点)
func 失去焦点():
	get_window().grab_focus()
	Tween
	print("Window失去焦点")
