extends Control
var arr : Array[Array]
var 索引 : int
var 展开 : bool = false
var child_int : int = 0
var valuearr : int = -1
func _process(delta: float) -> void:
	if 展开 == false:
		$ScrollContainer.visible = true
		$intArray.visible = false
	else:
		$ScrollContainer.visible = false
		$intArray.visible = true
	if child_int != $ScrollContainer/VBoxContainer.get_child_count():
		var p : int = 0
		child_int = $ScrollContainer/VBoxContainer.get_child_count()
		for i in $ScrollContainer/VBoxContainer.get_children():
			i.name = str(p)
			i.text = str(arr[p])
			p += 1
	if valuearr != -1:
		展开 = true
