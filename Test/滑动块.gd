extends TextureRect
var mouse : bool
var mouses : bool
var mousex : float
var 虚距 : float = position.x
# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	虚距 = $"..".size.x


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	if mouse == true:
		if Input.is_action_just_pressed("左键"):
			mouses = true
			mousex = get_viewport().get_mouse_position().x
	if Input.is_action_just_released("左键") and mouses == true:
		mouses = false
		虚距 = position.x
		var e = create_tween()
		e.tween_property($".","modulate",Color(1.0, 1.0, 1.0, 1.0),0.1)
		var s : int = int($"..".value)
		$"..".value = s
		position.x = s * ($"..".size.x / $"..".max_value)
	if mouses == true:
		var e = create_tween()
		e.tween_property($".","modulate",Color(0.5, 0.5, 0.5, 1.0),0.1)
		position.x = 虚距 + 计算距离()
		$"..".value = position.x / $"..".size.x * 100
		if position.x > $"..".size.x:
			position.x = $"..".size.x
		if position.x < 0:
			position.x = 0
func 计算距离():
	return ((mousex - get_viewport().get_mouse_position().x) * -1) / $"..".scale.x

func _on_mouse_entered() -> void:
	mouse = true


func _on_mouse_exited() -> void:
	mouse = false


func _on_进度条_value_changed(value: float) -> void:
	音量.音效音量 = $"..".value
