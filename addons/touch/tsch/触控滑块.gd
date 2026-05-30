extends Touchbutton
@export var 父级 : touchBar
var 累计增加 : float
func _ready() -> void:
	初始化()
	按下时void.connect(点)
	抬起时void.connect(抬)
func _process(delta: float) -> void:
	p2(delta)
	if 父级 != null:
		if 父级.value != 父级.maxvalue:
			$"..".position = Vector2((父级.size.x * ((父级.value - 父级.minvalue) / (父级.maxvalue - 父级.minvalue))) - $"..".size.x * 0.5,父级.size.y * 0.5 -$"..".size.y * 0.5) + 父级.滑块偏移
		else:
			$"..".position = Vector2((父级.size.x * ((父级.value - 父级.minvalue) / (父级.maxvalue - 父级.minvalue))) - $"..".size.x * 0.5,父级.size.y * 0.5 -$"..".size.y * 0.5) + 父级.临界值偏移
	while 累计增加 >= 父级.stop or 累计增加 <= -父级.stop:
		if 累计增加 >= 父级.stop:
			父级.value += 父级.stop
			累计增加 -= 父级.stop
		elif 累计增加 <= -父级.stop:
			父级.value -= 父级.stop
			累计增加 += 父级.stop
func 点():
	父级.emit_signal("滑动按下时",name)
	父级.emit_signal("滑动按下时void",name)
func 抬():
	父级.emit_signal("滑动结束时",name)
	父级.emit_signal("滑动结束时void",name)
func _input(event: InputEvent) -> void:
	if pre == true:
		if event is InputEventScreenDrag:
			if event.index == touchid:
				累计增加 += (cos(父级.rotation) * event.relative.x / 父级.scale.x) * 父级.乘数
				累计增加 += (sin(父级.rotation) * event.relative.y / 父级.scale.y) * 父级.乘数
	if event is InputEventScreenTouch:
		if event.pressed == true:
			touchid = event.index
		else:
			touchid = -1
