extends Control
var key : int = -1
var ArrayNode : Control

func _ready() -> void:
	$Button.b = ArrayNode.Wave[key]
	if ArrayNode != null and key < ArrayNode.Wave.size():
		$Button.text = str(ArrayNode.Wave[key])

func _process(delta: float) -> void:
	$Button.text = str($Button.b)
	if ArrayNode != null and key < ArrayNode.Wave.size():
		ArrayNode.Wave[key] = $Button.b
