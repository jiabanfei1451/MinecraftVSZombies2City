extends Area2D
@export var 绑定物体 : Area2D
@export var 绑定偏移 : Vector2
@export var 绑定缩放 : Vector2
@export var 绑定颜色 : Color
## 如果这个节点名称不存在或者是在此节点之后生成的话会无限循环下去直到报错
@export var 自动绑定父节点 : bool = true
@export var 父节点名称 : String
@export var 直接绑定根节点 : bool = false
@export_enum("关卡场景:0","变量:1") var 决定启用HDR变量类型 : int
var 父节点
@export var 无需绑定 : bool = false
var 完成绑定 : bool
# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	if 无需绑定 == false:
		visible = false
		scale = 绑定缩放
		modulate = 绑定颜色
	else:pass

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	if 决定启用HDR变量类型 == 0:
		if get_tree().current_scene.HDR光源 == true:
			$"HDR光源".visible = true
			$Bubble.visible = false
		else:
			$"HDR光源".visible = false
			$Bubble.visible = true
	$"HDR光源".color = modulate
	if 无需绑定 == false:
		if 绑定物体 != null:
			visible = true
			完成绑定 = true
			position = 绑定物体.position + 绑定偏移
			scale = 绑定缩放
			modulate = 绑定颜色
		else:
			if 完成绑定 == true:
				queue_free()
func 自动绑定():
	var ds = $"."
	if 直接绑定根节点 == false:
		while ds.name != 父节点名称:
			ds.get_node("..")
			await get_tree().create_timer(0.02)
		父节点 = ds
	else:
		父节点 = get_tree().current_scene
