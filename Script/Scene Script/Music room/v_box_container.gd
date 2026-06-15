extends VBoxContainer

func _ready() -> void:
	for i in 7:
		var s : PackedScene = preload("res://Scene/Misc Scene/Music room/List.tscn")
		var sp = s.instantiate()
		sp.id = i
		add_child(sp)
