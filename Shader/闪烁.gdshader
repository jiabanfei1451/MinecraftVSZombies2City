shader_type canvas_item;
uniform vec4 colo : source_color;
uniform sampler2D text : hint_screen_texture;
uniform float speed = 1;
void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	
	vec4 t = texture(TEXTURE,UV);
	vec4 tmi = t+t;
	COLOR=mix(tmi * 0.75,(t * colo) * sin(TIME*speed),0.5);
}

//void light() {
//	// Called for every pixel for every light affecting the CanvasItem.
//	// Uncomment to replace the default light processing function with this one.
//}
