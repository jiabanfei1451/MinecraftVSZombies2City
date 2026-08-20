extends HSlider

func _on_drag_ended(value_changed: bool) -> void:
	全局变量.窗口缩放 = value
