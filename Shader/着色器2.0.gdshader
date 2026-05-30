shader_type canvas_item;
uniform vec2 fff;
uniform bool miao_bian;
uniform vec4 coloro : source_color = vec4(0,0,0,1);
uniform float lc;

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	if (miao_bian == true){
		vec2 uv = UV;
		vec2 x = uv + vec2(TEXTURE_PIXEL_SIZE.x * fff.x,0);
		vec2 ix = uv + vec2(-TEXTURE_PIXEL_SIZE.x * fff.x,0);
		vec2 y = uv + vec2(0,TEXTURE_PIXEL_SIZE.y * fff.y);
		vec2 iy = uv + vec2(0,-TEXTURE_PIXEL_SIZE.y * fff.y);
		
		float ffl = float(lc / 100.0);
		vec4 cx = texture(TEXTURE,x);
		vec4 cix = texture(TEXTURE,ix);
		vec4 cy = texture(TEXTURE,y);
		vec4 ciy = texture(TEXTURE,iy);
		
		vec4 coo = cx + cix + cy +ciy;
		coo.rgb = coloro.rgb;
		coo.a = min(coo.a,1.0);
		vec4 yuanshen = texture(TEXTURE,UV);
		
		COLOR = mix(coo,yuanshen,yuanshen.a) ;
		}
}

//void light() {
//	// Called for every pixel for every light affecting the CanvasItem.
//	// Uncomment to replace the default light processing function with this one.
//}
