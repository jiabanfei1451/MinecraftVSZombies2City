extends ColorRect
var 最大血量 : float
# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	await  get_tree().create_timer(0.1).timeout
	最大血量 = $"..".血量

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	var dsa = create_tween()
	dsa.tween_property($"当前值","size",Vector2($"..".血量 / 最大血量 * 70,9),1)
	$"显示".text = str($"..".血量) + "/" +str(最大血量)
