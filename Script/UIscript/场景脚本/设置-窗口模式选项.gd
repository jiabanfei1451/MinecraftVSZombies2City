extends TouchColorRectButton
@export var 触发功能ID : int
func _ready() -> void:
	R2()
	点击时void.connect(点)
func _process(delta: float) -> void:
	P3(delta)
	
func 点():
	$"../../../../../../AudioStreamPlayer".play()
	全局变量.窗口模式 = 触发功能ID
