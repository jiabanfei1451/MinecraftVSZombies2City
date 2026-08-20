extends FileDialog



func _on_button_pressed() -> void:
	visible = true
	


func _on_dir_selected(dir: String) -> void:
	$"../Control/FinalEdit/Path".text = dir
