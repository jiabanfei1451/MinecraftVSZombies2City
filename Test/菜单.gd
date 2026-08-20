extends CanvasLayer
var q : bool = false
var 文字提示 : Array[String] = [
"这里真的没什么
你就真的没有如何事情了吗？",

"没事你继续刷你的",

"你知道吗这个同人
游戏是以“MVZ2”为原型",

"你要给这个同人作者打赏？
你认真的？",

"游戏二次制作为
cuerzor58制作的MVZ2",

"huh?没了？真的！",

"看完局势后就别再按”ESC“了
因为你刷到数组最后一个了！",

"看完了吗？老娘要睡觉！",

"想着这个版本的作者是谁？
那就在B站搜”痊愈加班费“",

"老伙计又回来了？",

"听说反重力板和狙击是CP
那把他两放置在一块吧！！！",

"要给原作打赏？那可以在爱发电
搜索”cuerzor“看看吧！"
] 

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	收起(0)
# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	if Input.is_action_just_released("ESC"):
		if q == false:
			收起(0.5)
		else:
			展开(0.5)
func 收起(速度:float):
	var tw = create_tween()
	var d = create_tween()
	tw.tween_property($".","scale",Vector2(0,0),速度).set_trans(Tween.TRANS_SINE)
	d.tween_property($".","offset",Vector2(1154,648)*0.5,速度).set_trans(Tween.TRANS_SINE)
	q = true
func 展开(速度:float):
	$"区域/按钮/Mc文字2".text = 文字提示.pick_random()
	var tw = create_tween()
	var d = create_tween()
	tw.tween_property($".","scale",Vector2(1,1),速度).set_trans(Tween.TRANS_SINE)
	d.tween_property($".","offset",Vector2(0,0)*0.25,速度).set_trans(Tween.TRANS_SINE)
	q = false
