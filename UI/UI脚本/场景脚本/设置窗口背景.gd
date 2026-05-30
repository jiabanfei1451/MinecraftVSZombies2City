extends ColorRect
@export var cont : Array[Control]
@export var 开启ID : String = "0"
func _ready() -> void:
	cont.append_array(get_children(true))
func _process(delta: float) -> void:
	for i in cont:
		if i.name != 开启ID:
			i.visible = false
		else:
			i.visible = true
