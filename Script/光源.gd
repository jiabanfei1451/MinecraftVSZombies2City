extends Area2D
@export var 绑定物体 : Node2D
@export var 生成滤镜中的节点 : Node2D
@export var 绑定偏移 : Vector2
@export var 绑定缩放 : Vector2
@export var 绑定颜色 : Color
@export var 使用绑定颜色 : bool = false
## 如果这个节点名称不存在或者是在此节点之后生成的话会无限循环下去直到报错
@export var 自动绑定父节点 : bool = true
@export var 父节点 : Node
@export var 直接绑定根节点 : bool = false
@export_enum("关卡场景:0","变量:1") var 决定启用HDR变量类型 : int
@export_enum("光源强度:0","滤镜强度:1") var 使用变量 : int = 0
@export var 反转光照颜色 : bool = false
@export_enum("Add:1","SUB:2","Mix:0") var 光照模式 : int = 2
@export var 反转HDR光照颜色 : bool = false
@export_enum("Add:0","SUB:1","Mix:2") var HDR光照模式 : int = 0
@export var 层级 : int = 0
@export var 无需绑定 : bool = false
var 完成绑定 : bool
var 当前色 : Color
func _ready() -> void:
	$"HDR光源".blend_mode = HDR光照模式
	当前色 = modulate
	if 无需绑定 == false:
		if 自动绑定父节点 == true:
			父节点 = $".."
		if 直接绑定根节点 == true:
			父节点 = get_tree().current_scene
func _process(delta: float) -> void:
	z_index = 层级
	var 量 : float
	var 量2 : float
	var 颜色 : Color
	if 使用绑定颜色 == true:
		颜色 = 绑定颜色
	else:
		颜色 = 当前色

	if 使用变量 == 0:
		量 = get_tree().current_scene.光源强度
		量2 = get_tree().current_scene.光源贴图强度
	else:
		量 = get_tree().current_scene.滤镜强度
		量2 = get_tree().current_scene.滤镜贴图强度
	if 反转光照颜色 == true:
		modulate = 颜色 * -1
	else:
		modulate = 颜色
	
	if 绑定物体 != null:
		global_position = 绑定物体.global_position + 绑定偏移
		scale = 绑定缩放
	else:
		if 无需绑定 == false:
			销毁()
	if $"HDR光源".visible == true:
		if  反转HDR光照颜色 == true:
			$"HDR光源".color = 颜色 * -1
		else:
			$"HDR光源".color = 颜色
	if 节点提供变量.遮罩 != null:
		if 生成滤镜中的节点 == null:
			var s = Sprite2D.new()
			s.texture = $Bubble.texture
			s.visible = false
			var c = CanvasItemMaterial.new()
			c.blend_mode = 光照模式
			节点提供变量.遮罩.add_child(s)
			s.material = c
			生成滤镜中的节点 = s
		else:
			生成滤镜中的节点.visible = true
			生成滤镜中的节点.z_index = 层级
			if 决定启用HDR变量类型 == 0:
				生成滤镜中的节点.global_position = global_position + 绑定偏移 + get_tree().current_scene.坐标偏移
			生成滤镜中的节点.scale = scale
			生成滤镜中的节点.modulate = modulate
	if 决定启用HDR变量类型 == 0:
		if get_tree().current_scene.HDR光源 == true:
			$"HDR光源".blend_mode = HDR光照模式
			$"HDR光源".visible = true
			if $Bubble != null:
				$Bubble.visible = false
			if 生成滤镜中的节点 != null:
				生成滤镜中的节点.visible = false
		else:
			$"HDR光源".visible = false
			if $Bubble != null:
				$Bubble.visible = true
			if 生成滤镜中的节点 != null:
				生成滤镜中的节点.visible = true

	if 生成滤镜中的节点 != null:
		生成滤镜中的节点.modulate.a = 量
	$Bubble.modulate.a = 量2
	$"HDR光源".energy = 量
func _on_tree_exited() -> void:
	if 生成滤镜中的节点 != null:
		生成滤镜中的节点.queue_free()
func 销毁():
	var sdsd = create_tween().tween_property($".","绑定缩放",Vector2(0,0),0.1).set_trans(Tween.TRANS_EXPO)
	var sdd = create_tween().tween_property($".","scale",Vector2(0,0),0.1).set_trans(Tween.TRANS_EXPO)
	await get_tree().create_timer(2).timeout
	queue_free()
