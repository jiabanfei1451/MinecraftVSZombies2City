extends Sprite2D
var fl : float
var fl2 : float
var fl3 : float
var fl4 : float

func _process(delta: float) -> void:
	var tw = create_tween()
	$Sprite2D.add_point(position)
	tw.tween_property($".","position",Vector2(sin(fl) * 100 * fl2,0),0.15)
	fl += 0.1
	fl2 += 0.005
	print(int(sin(fl) * 90))
	if int(sin(fl) * 90) >= 40:
		fl3 += 1
	elif int(sin(fl) * 90) <= -40:
		fl3 += 1
	if $Sprite2D.get_point_count() >= 30:
		$Sprite2D.remove_point(0)
