extends Button

signal que(name:String)
signal prebutton(name:String)
var p : bool = false
var t : float = 0
func _ready() -> void:
	pressed.connect(an)
	button_down.connect(button_d)
	button_up.connect(button_u)
func an():
	emit_signal("prebutton",name)
func _process(delta: float) -> void:
	if p == true:
		t += delta
	else:
		t = 0
	if t >= 1:
		emit_signal("que",name)
		queue_free()
func button_d():
	p = true
func button_u():
	p = false
