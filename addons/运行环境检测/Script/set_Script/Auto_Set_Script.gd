extends Node
@export var CSharp_Script_Path : String = "res://"
@export var GDScript_Script_Path : String = "res://"

func _process(delta: float) -> void:
	await get_tree().create_timer(0.01).timeout
	if Start_Environment.Start_Environment == "GDScript":
		set_script(load(GDScript_Script_Path))
	else:
		print(CSharp_Script_Path)
		set_script(load(CSharp_Script_Path))
	_physics_process(0.033)
	_ready()
		
