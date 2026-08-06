@tool
extends EditorPlugin


func _enable_plugin() -> void:
	add_autoload_singleton("Start_Environment","res://addons/运行环境检测/Script/Master_Script/Environment.gd")
	add_autoload_singleton("Set_Environment","res://addons/运行环境检测/Script/Set_Environment.cs")


func _disable_plugin() -> void:
	remove_autoload_singleton("Start_Environment")
	remove_autoload_singleton("Set_Environment")


func _enter_tree() -> void:
	# Initialization of the plugin goes here.
	pass


func _exit_tree() -> void:
	# Clean-up of the plugin goes here.
	pass
