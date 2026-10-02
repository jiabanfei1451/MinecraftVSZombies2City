shader_type canvas_item;
uniform float d : hint_range(-1, 1);
uniform float s : hint_range(-10.0, 10.0);
uniform float x : hint_range(0.0, 3.0);
void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	vec2 p;
	p.x =sin(UV.y * s + TIME * s) * d;
	p.y = cos(UV.x * s + TIME * s) * d;
	vec2 po = UV + p;
	COLOR = texture(TEXTURE,po);
}

//void light() {
//	// Called for every pixel for every light affecting the CanvasItem.
//	// Uncomment to replace the default light processing function with this one.
//}
