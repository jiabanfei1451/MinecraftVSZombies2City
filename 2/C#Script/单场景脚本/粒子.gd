extends Node
var fr : float
func _process(delta: float) -> void:
	if $"..".开启缓动 == true:
		var tw = create_tween()
		tw.tween_property($"..","position",$"..".当前位置,0.15)
