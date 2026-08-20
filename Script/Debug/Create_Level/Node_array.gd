extends Control
@export var arr : Array[Array]
@export var MonsterValue : Array[int]
@export var Wave : Array[bool]
var 索引 : int = -1
var 展开 : bool = false
var child_int : int = 0
var arraychild : int = -1
var arrayv : int = -1
var valuearr : int = -1
func _process(delta: float) -> void:
	MonsterValue.resize(arr.size())
	Wave.resize(arr.size())
	if 展开 == false:
		$ScrollContainer.visible = true
		$intArray.visible = false
	else:
		$ScrollContainer.visible = false
		$intArray.visible = true
	var w : int = 0
	child_int = $ScrollContainer/VBoxContainer.get_child_count()
	for i in $ScrollContainer/VBoxContainer.get_children():
		i.name = str(w)
		i.text = str(arr[w])
		w += 1
	var scene : int = 0
	for i in $intArray/ScrollContainer/VBoxContainer.get_children():
		i.key = scene
		scene += 1
	if valuearr != -1:
		展开 = true
		if 索引 != valuearr:
			arrayv = valuearr
		if arraychild != arr[valuearr].size():
			for i in $intArray/ScrollContainer/VBoxContainer.get_children():
				i.queue_free()
			for i in arr[索引].size():
				var p = preload("res://Scene/Debug/Create_Level/buttonID.tscn")
				var pi = p.instantiate()
				pi.id = 索引
				pi.ArrayNode = $"."
				pi.custom_minimum_size = Vector2($intArray/ScrollContainer.size.x,40)
				$intArray/ScrollContainer/VBoxContainer.add_child(pi)
				
			arraychild = arr[valuearr].size()
