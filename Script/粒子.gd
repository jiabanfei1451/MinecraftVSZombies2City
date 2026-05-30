extends Sprite2D
@export var 随机向量 : Vector2 = Vector2(50,50)
@export var 持续时间 : float = 0.3
@export var 弹跳力 : float = 1
@export var y加速度 : float = 10
@export var x偏移值 : Vector2 = Vector2(25,-25)
@export var y倍率 : Vector2 = Vector2(0.1,2)
var 最终向量 : Vector2
var 上升幅度 : float = 1
var 开始上升 : bool = true
var speedy : float
var speed : float = 0
var addspeed :float
var 开始偏移 : bool = false
var 目标x : float
var 上升速度 : float = randf_range(1,4)
var xxx : float
var 弹跳阈值 : float = 0
var 减少阈值 : float = 10
var 除数 : int = 1
# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	speedy = randf_range(y倍率.x,y倍率.y)
	最终向量 = position + Vector2(randf_range(随机向量.x * -1,随机向量.x),randf_range(随机向量.y * -1,随机向量.y))
	position = 最终向量
	目标x = 最终向量.x + randf_range(x偏移值.x,x偏移值.y)
	xxx = 最终向量.x
	await get_tree().create_timer(randf_range(2,2.5)).timeout
	queue_free()
# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	if 开始上升:
		if 上升幅度 > 90 or sin(上升幅度) <= -0.05:
			开始上升 = false
			if speed == 0:
				speed = 1
				addspeed = 4
		else:
			position += Vector2(0,((sin(上升幅度) * -上升速度) * speedy) * (delta * 60))
			上升幅度 += y加速度 * delta
	else:
		position += Vector2(0,(speed * 弹跳阈值) * (delta * 60))
		speed += delta * 10
	if 目标x != 0:
		var tw = create_tween()
		tw.tween_property($".","xxx",目标x,clampf(1 / speedy,0,0.5) / 除数)
	position.x = xxx
	if 弹跳阈值 < 1:
		弹跳阈值 += 减少阈值 * delta
		减少阈值 += 10 * delta
	else:
		弹跳阈值 = 1
		减少阈值 = 10
	if position.y > 最终向量.y:
		弹跳阈值 = (-speedy * (弹跳力 * 0.1) / 除数)
		除数 += 1
		position.y = 最终向量.y
