extends Sprite2D
@export var 乘数 = 4
@export var 大小 = Vector2(1400,1020)
@export var 自动隐藏贴图 : bool = true
func _ready() -> void:
	for x in 乘数:
		for y in 乘数:
			var textur = AtlasTexture.new()
			textur.atlas = texture.atlas
			textur.region = Rect2((大小.x/乘数) * x,(大小.y/乘数) * y,大小.x/乘数,大小.y/乘数)
			var 实例化 : Sprite2D = Sprite2D.new()
			实例化.texture = textur
			add_child(实例化)
			实例化.position = Vector2((大小.x/乘数) * x,(大小.y/乘数) * y)
			实例化.centered = false
		print(x)
	if 自动隐藏贴图 == false:
		texture = null
