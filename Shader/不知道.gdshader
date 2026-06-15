shader_type canvas_item;
uniform float s;
uniform vec2 d = vec2(0.5,0.5);
void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	vec2 sd = vec2(d.x,d.y);
	vec2 u = UV;
	float p = distance(sd,u);
	float sep = step(p * s,0.5);
	vec2 sss = sd + u;
	COLOR.a = sep;
}

//void light() {
//	// Called for every pixel for every light affecting the CanvasItem.
//	// Uncomment to replace the default light processing function with this one.
//}
