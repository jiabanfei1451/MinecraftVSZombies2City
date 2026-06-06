extends VBoxContainer

func _ready() -> void:
	for i in 4:
		var s : PackedScene = preload("res://Scene/Misc Scene/Music boom/List.tscn")
		var sp = s.instantiate()
		sp.id = i
		add_child(sp)
