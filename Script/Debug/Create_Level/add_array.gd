extends Button


var arr

func _ready() -> void:
	arr = $"../Array".arr

func _on_pressed() -> void:
	var GDscr : GDScript = preload("uid://ccybp1x6lnr5m")
	var butt = GDscr.new()
	butt.custom_minimum_size.y = 40
	butt.custom_minimum_size.x = $"../Array/ScrollContainer".size.x
	butt.text = "array"
	butt.prebutton.connect(p)
	butt.que.connect(c)
	butt.name = str($"../Array/ScrollContainer/VBoxContainer".get_child_count())
	print(butt.get_script())
	$"../Array".arr.append([])
	$"../Array/ScrollContainer/VBoxContainer".add_child(butt)

func p(name:String):
	$"../Array".索引 = name
	$"../Array".valuearr = name
func c(name:String):
	$"../Array".arr.remove_at(int(name))
	
	
	
	
