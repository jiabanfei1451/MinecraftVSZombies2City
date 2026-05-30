shader_type canvas_item;
uniform float shendu = 1;
uniform float fff = 0.05;
uniform float a = 1;
uniform sampler2D co:hint_screen_texture;

void vertex() {
	float ti = TIME;
	clamp(ti,0,2);
	COLOR = vec4(0,0,0,0.2);
}

void fragment() {
	vec2 pingmu = vec2(SCREEN_UV.x,SCREEN_UV.y + UV.y * -fff);
	vec2 pingmu2 = vec2(SCREEN_UV.x - UV.x * -fff,SCREEN_UV.y);
	vec2 pingmu3 = vec2(SCREEN_UV.x,SCREEN_UV.y + UV.y * fff);
	vec2 pingmu4 = vec2(SCREEN_UV.x - UV.x * fff,SCREEN_UV.y);
	vec2 pingmu5 = vec2(SCREEN_UV.x,SCREEN_UV.y);
	vec2 pingmu6 = vec2(SCREEN_UV.x,SCREEN_UV.y + UV.y * -fff * 0.5);
	vec2 pingmu7 = vec2(SCREEN_UV.x - UV.x * -fff * 0.5,SCREEN_UV.y);
	vec2 pingmu8 = vec2(SCREEN_UV.x,SCREEN_UV.y + UV.y * fff * 0.5);
	vec2 pingmu9 = vec2(SCREEN_UV.x - UV.x * fff * 0.5,SCREEN_UV.y);
	vec2 pingmu10 = vec2(SCREEN_UV.x+ UV.x * -fff * 0.5 ,SCREEN_UV.y + UV.y * -fff * 0.5);
	vec2 pingmu11 = vec2(SCREEN_UV.x - UV.x * -fff * 0.5,SCREEN_UV.y + UV.y * -fff * 0.5);
	vec2 pingmu12 = vec2(SCREEN_UV.x + UV.x * -fff * 0.5,SCREEN_UV.y + UV.y * fff * 0.5);
	vec2 pingmu13 = vec2(SCREEN_UV.x - UV.x * fff,SCREEN_UV.y + UV.y * -fff);
	vec2 pingmu14 = vec2(SCREEN_UV.x+ UV.x * -fff ,SCREEN_UV.y + UV.y * -fff);
	vec2 pingmu15 = vec2(SCREEN_UV.x - UV.x * -fff,SCREEN_UV.y + UV.y * -fff);
	vec2 pingmu16 = vec2(SCREEN_UV.x + UV.x * -fff,SCREEN_UV.y + UV.y * fff);
	vec2 pingmu17 = vec2(SCREEN_UV.x - UV.x * fff,SCREEN_UV.y + UV.y * -fff);
	vec4 p = texture(co,pingmu);
	vec4 p2 = texture(co,pingmu2);
	vec4 p3 = texture(co,pingmu3);
	vec4 p4 = texture(co,pingmu4);
	vec4 p5 = texture(co,pingmu5);
	vec4 p6 = texture(co,pingmu6);
	vec4 p7 = texture(co,pingmu7);
	vec4 p8 = texture(co,pingmu8);
	vec4 p9 = texture(co,pingmu9);
	vec4 p10 = texture(co,pingmu10);
	vec4 p11 = texture(co,pingmu11);
	vec4 p12 = texture(co,pingmu12);
	vec4 p13 = texture(co,pingmu13);
	vec4 p14 = texture(co,pingmu14);
	vec4 p15 = texture(co,pingmu15);
	vec4 p16 = texture(co,pingmu16);
	vec4 p17 = texture(co,pingmu17);
	vec2 d = vec2(vec2(pingmu2.r , pingmu4.g).r,vec2(pingmu3.r,pingmu.g).g);
	COLOR = vec4(vec4(p + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17).rgb * (a / 17.0),shendu);
}

//void light() {
//	// Called for every pixel for every light affecting the CanvasItem.
//	// Uncomment to replace the default light processing function with this one.
//}
