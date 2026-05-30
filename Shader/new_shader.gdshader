shader_type canvas_item;

// 和你 Inspector 面板里的参数名完全一致
uniform vec2 LightPos;
uniform float LightRadius = 200.0;
uniform float Intensity : hint_range(0.0, 3.0) = 1.5;
uniform vec4 LightColor : source_color = vec4(1.0, 0.9, 0.6, 1.0);

void fragment() {
    // 先正常显示你的纹理
    vec4 tex_color = texture(TEXTURE, UV);
    COLOR = tex_color;

    // 直接用 UV 计算像素在 Sprite2D 中的相对位置
    vec2 pixel_pos = UV * TEXTURE_PIXEL_SIZE;

    // 这里有个关键转换：LightPos 是世界坐标，而 pixel_pos 是纹理局部坐标
    // 为了简单，我们直接让光源位置和纹理位置对齐
    // 如果你希望光源可以在屏幕任意位置，我们可以用下面的方法：
    // vec2 screen_pos = SCREEN_UV * VIEWPORT_SIZE; （但 VIEWPORT_SIZE 部分版本也不支持）
    // 这里我们用一个最简单的方式：直接以纹理中心为原点
    vec2 center = vec2(0.5);
    float dist = length(UV - center);

    // 光照衰减
    float falloff = 1.0 - clamp(dist / (LightRadius / 1000.0), 0.0, 1.0);
    falloff *= falloff;

    // 叠加光照效果
    COLOR.rgb += LightColor.rgb * falloff * Intensity;
}