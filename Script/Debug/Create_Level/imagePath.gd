extends LineEdit

func _process(delta: float) -> void:
	var s = Image.new()
	s.load(text)
	var t = ImageTexture.new()
	t.set_image(s)
	var ass : AtlasTexture = AtlasTexture.new()
	ass.atlas = t
	$"../HouseImage2".texture = ass
	var ts : Texture2D = load(text)
	$"../HouseImage3".texture = ts
