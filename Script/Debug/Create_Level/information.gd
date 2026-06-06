extends Control

func _ready() -> void:
	$Version.text = str(ProjectSettings.get_setting("application/config/version"))
