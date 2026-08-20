extends Sprite2D

# 获取场景文件的预览图（需在编辑器中为场景设置预览图）
func get_scene_preview_image(scene_path: String) -> Image:
	# 加载场景资源
	var scene = ResourceLoader.load(scene_path)
	if not scene:
		print("场景文件不存在：", scene_path)
		return null
	
	# 读取场景的预览图元数据（编辑器中设置的预览图）
	var preview_texture = scene.get_meta("editor_preview")
	if preview_texture and preview_texture is Texture2D:
		return preview_texture.get_image()
	else:
		print("该场景未设置预览图")
		return null

# 使用示例
func _ready():
	# 加载 "res://Level1.tscn" 场景的预览图
	var preview_image = get_scene_preview_image("res://Object/熔炉.tscn")
	if preview_image:
		# 保存预览图
		preview_image.save_png("res://Object/熔炉.tscn")
	
