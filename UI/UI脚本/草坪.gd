extends ColorRect
var 触碰到鼠标 : bool = false
@export var 生成节点 : Node2D
@export var 怪物生成节点 : Node2D
@export var 器械 : PackedScene
@export var Y偏移 : float = 0
@export var X偏移 : float = 0
@export var 绑定器械 : Area2D = null
@export var DEBUG : bool = false
@export var DEBUG_Color : Color = Color(0.149, 1.0, 0.0, 0.50)
@export_group("属性")
@export_group("坐标")
@export var 坐标 : Vector2
@export var 微偏移量 : Vector2 = Vector2(-452,121)
@export var 体积 : Vector2 = Vector2(81,79)
@export var 倍率 : Vector2 = Vector2(-1,1)
@export_enum("是","否",) var 是否自动更改 : String = "否"
func _ready() -> void:
	if DEBUG == false:
		$".".color = Color(0.0, 0.0, 0.0, 0.0)
	else:
		$".".color = DEBUG_Color
	await get_tree().create_timer(0.2).timeout
	if get_tree().current_scene.get_node("器械") != null:
		生成节点 = get_tree().current_scene.get_node("器械")
	if get_tree().current_scene.get_node("怪物") != null:
		怪物生成节点 = get_tree().current_scene.get_node("怪物")
func _process(delta: float) -> void:
	器械 = get_tree().current_scene.当前器械
	if 是否自动更改 == "是":
		position.x = 微偏移量.x - (坐标.x * 体积.x * 倍率.x)
		position.y = 微偏移量.y - (坐标.y * 体积.y * 倍率.y)
	visible =! get_tree().current_scene.正在使用其他属性
	$Touchbutton.启用 = visible
	if 触碰到鼠标 == true:
		if 绑定器械 == null and get_tree().current_scene.当前器械 != null:
			if not DEBUG:
				$".".color = green()
		elif get_tree().current_scene.当前器械 == null:
			if not DEBUG:
				$".".color = colornull()
		else:
			if get_tree().current_scene.当前器械 != null: 
				if not DEBUG:
					color = red()
	else:
		if not DEBUG:
			$".".color = colornull()

func _on_鼠标来了() -> void:
	触碰到鼠标 = true
	
func colornull():
	if not DEBUG:
		return Color(1.0, 1.0, 1.0, 0.0)
func green():
	if not DEBUG:
		return Color(0.0, 1.0, 0.0, 1.0)
func red():
	if not DEBUG:
		return Color(1.0, 0.0, 0.0, 1.0)

func _on_鼠标走了() -> void:
	触碰到鼠标 = false
func pre_up():
	触碰到鼠标 = false
func pre_down():
	触碰到鼠标 = true
func pre():
	if 绑定器械 == null and 器械 != null:
		get_tree().current_scene.来自.已被放置()
		var 器械实列 = 器械.instantiate()
		器械实列.position = position + pivot_offset + Vector2(pivot_offset.x * X偏移,pivot_offset.y * Y偏移)
		if 器械实列.is_in_group("怪物") == true:
			怪物生成节点.add_child(器械实列)
		else:
			生成节点.add_child(器械实列)
			绑定器械 = 器械实列
		if 绑定器械 != null and 绑定器械.is_in_group("器械") == false:
			绑定器械 = null
