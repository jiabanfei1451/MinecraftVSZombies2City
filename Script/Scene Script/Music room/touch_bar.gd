extends touchBar
var b : bool = false
var tw : Array[Tween]
func _process(delta: float) -> void:
	p2(delta)
	乘数 = maxvalue * 0.001
	var s : AudioStream = MuiscEngine.音乐.stream
	if s != null:
		tw.append(create_tween())
		maxvalue = s.get_length()
	if b == false:
		tw.append(create_tween())
		tw.back().tween_property($".","value",MuiscEngine.音乐.get_playback_position(),1)
	var m : int
	var sm : float
	var m2 : int
	var sm2 : float
	sm = $".".value
	sm2 = $".".maxvalue
	while sm >= 60:
		m += 1
		sm -= 60
	while sm2 >= 60:
		m2 += 1
		sm2 -= 60
	$"../Label".text = str(m2) + ":" + str(snappedf(sm2,0.01)) + "/" + str(m) + ":" + str(snappedf(sm,0.01))
func _on_滑动按下时void() -> void:
	for i in tw:
		if i != null:
			i.stop()
	b = true


func _on_滑动结束时void() -> void:
	MuiscEngine.音乐.play(value)
	b = false
