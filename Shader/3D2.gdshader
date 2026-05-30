shader_type canvas_item;

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	vec2 pl = UV;
	vec4 lll = texture(TEXTURE,UV / 1.0);
	COLOR = lll * 3.0;
}

//void light() {
//	// Called for every pixel for every light affecting the CanvasItem.
//	// Uncomment to replace the default light processing function with this one.
//}
