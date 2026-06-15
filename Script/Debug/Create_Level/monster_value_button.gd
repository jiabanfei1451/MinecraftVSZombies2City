extends Control
var key : int = -1
var ArrayNode : Control

func _ready() -> void:
	if ArrayNode != null and key < ArrayNode.MonsterValue.size():
		$Button.text = str(ArrayNode.MonsterValue[key])

func _process(delta: float) -> void:
	if ArrayNode != null and key < ArrayNode.MonsterValue.size():
		ArrayNode.MonsterValue[key] = int($Button.text)
