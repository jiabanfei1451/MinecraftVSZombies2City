extends Label
var FPS:float
var 最大: float
var 最小: float
var timefps : float
var mouse : bool
func _ready() -> void:
	定时清空()

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	if Input.is_action_just_released("空格"):
		if visible == false:
			visible = true
		else:
			visible = false
	text = "FPS：" + str(FPS) + " 最大:" + str(最大) + " 最小:" + str(最小)
	FPS = Performance.get_monitor(Performance.TIME_FPS)
	if FPS > 最大:
		最大 = FPS
	if FPS <= 最小:
		最小 = FPS
func 定时清空():
	while true:
		await get_tree().create_timer(10).timeout
		最大 = 0
		最小 = 99999
