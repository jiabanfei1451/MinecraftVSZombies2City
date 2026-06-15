extends TouchColorRectButton
var shadermo : float = 0

func _ready() -> void:
	R2()
	var tween = create_tween()
	tween.parallel().tween_property($".","shadermo",0.05,0.5).set_trans(Tween.TRANS_SINE)
func _process(delta: float) -> void:
	P3(delta)
	点击时void.connect(dina)
	var ssd : ShaderMaterial = ShaderMaterial.new()
	var sdr : Shader = load("uid://cu3oaypq4i3xg")
	ssd.shader = sdr
	ssd.set_shader_parameter("fff",shadermo)
	$"../../ColorRect".set_material(ssd)
func dina():
	$"../AudioStreamPlayer".play()
	var tween = create_tween()
	tween.tween_property($"..","modulate",Color(0.0, 0.0, 0.0, 0.0),0.5).set_trans(Tween.TRANS_SINE)
	tween.parallel().tween_property($".","shadermo",0,0.5).set_trans(Tween.TRANS_SINE)
	await tween.finished
	$"../..".queue_free()
