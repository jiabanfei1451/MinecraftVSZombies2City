extends Control
@export var id : int = 0
@export var key : int = 0
@export var ArrayNode : Control
var freel : bool = false
func _process(delta: float) -> void:
	if ArrayNode != null:
		if freel != true:
			if key < ArrayNode.arr[id].size():
				if $Panel.text != str(ArrayNode.arr[id][key]):
					$Panel.text = str(ArrayNode.arr[id][key])
