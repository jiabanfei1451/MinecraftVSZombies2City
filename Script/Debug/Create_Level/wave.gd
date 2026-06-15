extends Control
var mValue : int = -1
var co : Control
func _ready() -> void:
	co = $"../Monster Array/Array"
func _process(delta: float) -> void:
	if co != null and co.Wave.size() != mValue:
		for i in $ScrollContainer/VBoxContainer.get_children():
			i.queue_free()
		mValue = co.Wave.size()
		var k : int = 0
		for i in co.Wave.size():
			var b = preload("res://Scene/Debug/Create_Level/Wave_Button.tscn")
			var bi = b.instantiate()
			bi.key = k
			bi.ArrayNode = $"../Monster Array/Array"
			bi.custom_minimum_size = Vector2($ScrollContainer.size.x,40)
			k += 1
			$ScrollContainer/VBoxContainer.add_child(bi)
