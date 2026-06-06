extends touchBar
var b : bool = false
var tw : Array[Tween]
func _process(delta: float) -> void:
	p2(delta)
	var s : AudioStream = $"../音频引擎".stream
	
	if s != null:
		tw.append(create_tween())
		tw.back().tween_property($".","maxvalue",s.get_length(),1)
	if b == false:
		tw.append(create_tween())
		tw.back().tween_property($".","value",$"../音频引擎".get_playback_position(),1)

func _on_滑动按下时void() -> void:
	for i in tw:
		if i != null:
			i.stop()
	b = true


func _on_滑动结束时void() -> void:
	$"../音频引擎".play(value)
	b = false
