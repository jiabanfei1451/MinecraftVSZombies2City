extends Button

signal que(name:String)
signal prebutton(name:String)
var p : bool = false
var t : float = 0
func _ready() -> void:
	pressed.connect(an)
	var Delete = Button.new()
	Delete.custom_minimum_size.x = size.y
	Delete.custom_minimum_size.y = size.y
	Delete.position = Vector2(0,0)
	Delete.text = "D"
	add_child(Delete)
	Delete.pressed.connect(delete)
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
func delete():
	emit_signal("que",name)
	queue_free()
