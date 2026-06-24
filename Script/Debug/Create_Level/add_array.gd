extends Button


var arr

func _ready() -> void:
	arr = $"../Array".arr

func _on_pressed() -> void:
	if $"../Array".索引 == -1:
		var GDscr : GDScript = preload("uid://ccybp1x6lnr5m")
		var butt = GDscr.new()
		butt.modulate = Color(0.0, 0.0, 0.0, 0.0)
		create_tween().tween_property(butt,"custom_minimum_size",Vector2($"../Array/ScrollContainer".size.x,40),1).set_trans(3)
		create_tween().tween_property(butt,"modulate",Color(1.0, 1.0, 1.0, 1.0),1).set_trans(1)
		butt.text = "array"
		butt.prebutton.connect(p)
		butt.que.connect(c)
		butt.name = str($"../Array/ScrollContainer/VBoxContainer".get_child_count())
		$"../Array".arr.append([])
		$"../Array/ScrollContainer/VBoxContainer".add_child(butt)
	else:
		$"../Array".arr[$"../Array".索引].append(0)
func p(name:String):
	$"../Array".索引 = name
	$"../Array".valuearr = name
func c(name:String):
	$"../Array".arr.remove_at(int(name))
	
	
	
	
