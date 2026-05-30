shader_type canvas_item;
uniform sampler2D text : hint_screen_texture;
uniform vec4 color_3: source_color;
uniform sampler2D text_2 : repeat_enable;

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	vec2 pack = (texture(text_2,UV + TIME * 0.02).rg * 0.01) ;
	vec2 uv = vec2(SCREEN_UV.x,SCREEN_UV.y) + pack;
	vec4 color = texture(text,uv);
	float color_1 = (color.r + color.g + color.b) / 255.0;
	color_1 = pow(color_1,1);
	float V = 1.0 - UV.x;
	 V += color_1;
	V = clamp(V,0.0,0.7);
	COLOR = mix(color,color_3,V = color_1);
}
