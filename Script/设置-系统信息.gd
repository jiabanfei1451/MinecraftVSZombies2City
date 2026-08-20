extends Control
func _process(delta: float) -> void:
	$"系统名称".text = str(OS.get_name(),OS.get_version_alias())
	var a = str("memory：",snapped(float(OS.get_memory_info()["physical"]) / 1024 / 1024/1024,0.1),"GB/",
	snapped(float(OS.get_memory_info()["free"]) / 1024 / 1024 / 1024,0.1),"GB")
	$"占用".text = a
	$CPU.text = "CPU：" + OS.get_processor_name()
	$GPU.text = "型号：" + OS.get_model_name()
	$"临时目录".text = "临时目录" + OS.get_temp_dir()
	$"临时目录/LinkButton".uri = OS.get_temp_dir()
	$"目录".text = "数据目录" + ProjectSettings.globalize_path("user://")
	$"目录/LinkButton".uri = ProjectSettings.globalize_path("user://")
