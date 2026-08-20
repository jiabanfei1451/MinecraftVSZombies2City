extends FoldableContainer


func _on_3D加速点击时() -> void:
	全局变量.窗口模式 = 0


func _on_常规点击时() -> void:
	全局变量.窗口模式 = 4


func _on_等比例点击时() -> void:
	全局变量.窗口模式 = 1


func _on_button_pressed() -> void:
	pass # Replace with function body.
