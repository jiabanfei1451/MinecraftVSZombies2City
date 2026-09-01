shader_type canvas_item;

uniform vec4 color_EX : source_color;
// If you intend to read from mipmaps with `textureLod()` LOD values greater than `0.0`,
// use `filter_nearest_mipmap` instead. This shader doesn't require it.

void fragment() {
	vec4 text = texture(TEXTURE,UV);
	COLOR = text + vec4(color_EX.rgb,COLOR.a);
}