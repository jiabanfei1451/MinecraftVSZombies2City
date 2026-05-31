extends CanvasLayer
@export var 卡槽节点 : Control

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	开始过度()


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass

func 开始过度():
	var x = create_tween()
	$"蓝图展示".position.y += 700
	$"卡槽".position.y -= 109
	var 动画 = create_tween()
	var d = create_tween()
	动画.tween_property($"蓝图展示","position",Vector2(0,-50.75),1.0).set_trans(Tween.TRANS_EXPO)
	d.tween_property($"卡槽","position",Vector2(0,0),1.0).set_trans(Tween.TRANS_EXPO)

func 完成选卡():
	var x = create_tween()
	var 动画 = create_tween()
	动画.tween_property($"蓝图展示","position",Vector2(0,700),1.0).set_trans(Tween.TRANS_EXPO)
	await get_tree().create_timer(1).timeout
	#var d = create_tween()
	#d.tween_property($"卡槽","position",Vector2(190.063,0),1.5).set_trans(Tween.TRANS_QUAD)
	get_tree().current_scene.完成选卡()
	queue_free()

func 音效():
	$"准备开始/播放音效".play()
func _on_完成选卡_pressed() -> void:
	pass # Replace with function body.
