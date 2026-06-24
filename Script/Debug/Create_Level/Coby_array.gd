extends Button



func _on_pressed() -> void:
	var arr : Array[Array] = $"../Array".arr
	var node : Control = $"../Array"
	if $"../Array".索引 == -1:
		node.arr.append([[]])
		arr.duplicate()
func p(name:String):
	$"../Array".索引 = name
	$"../Array".valuearr = name
func c(name:String):
	$"../Array".arr.remove_at(int(name))
	
	
	
	
