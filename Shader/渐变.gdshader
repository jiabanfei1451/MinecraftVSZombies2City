shader_type canvas_item;
uniform vec4 colo : source_color;
uniform float RValue :hint_range(-255.0, 255.0, 0) = 0.5;
uniform float GValue :hint_range(-255.0, 255.0, 0) = 0.5;
uniform float BValue :hint_range(-255.0, 255.0, 0) = 0.5;
uniform float AValue :hint_range(-255.0, 255.0, 0) = 0.5;
uniform bool oneValue = false;
uniform bool setAlpha = false;
group_uniforms Color;
uniform float MixR :hint_range(-1.0, 1.0, 0) = 0.5;
uniform float MixG :hint_range(-1.0, 1.0, 0) = 0.5;
uniform float MixB :hint_range(-1.0, 1.0, 0) = 0.5;
uniform float MixA :hint_range(-1.0, 1.0, 0) = 0.5;
uniform float R = 1.0;
uniform float G = 1.0;
uniform float B = 1.0;
uniform float A = 1.0;
group_uniforms TimeValue;
group_uniforms TimeValue.Time;
uniform float timeR = 1;
uniform float timeG = 1;
uniform float timeB = 1;
uniform float timeA = 1;
uniform float timeR2 = 1;
uniform float timeG2 = 1;
uniform float timeB2 = 1;
uniform float timeA2 = 1;
group_uniforms TimeValue.multiplier;
uniform float multiplierR = 0;
uniform float multiplierG = 0;
uniform float multiplierB = 0;
uniform float multiplierA = 0;
uniform float multiplierR2 = 0;
uniform float multiplierG2 = 0;
uniform float multiplierB2 = 0;
uniform float multiplierA2 = 0;
group_uniforms TimeValue.Valuemultiplier;
uniform float ValuemultiplierR :hint_range(-1.0, 1.0, 0) = 1;
uniform float ValuemultiplierG :hint_range(-1.0, 1.0, 0) = 1;
uniform float ValuemultiplierB :hint_range(-1.0, 1.0, 0) = 1;
uniform float ValuemultiplierA :hint_range(-1.0, 1.0, 0) = 1;
uniform float ValuemultiplierR2 :hint_range(-1.0, 1.0, 0) = 1;
uniform float ValuemultiplierG2 :hint_range(-1.0, 1.0, 0) = 1;
uniform float ValuemultiplierB2 :hint_range(-1.0, 1.0, 0) = 1;
uniform float ValuemultiplierA2 :hint_range(-1.0, 1.0, 0) = 1;
group_uniforms TimeValue;
uniform bool One = false;
group_uniforms functionMode;
uniform int Function_styleR : hint_range(0, 2, 0);
uniform int Function_styleG : hint_range(0, 2, 0);
uniform int Function_styleB : hint_range(0, 2, 0);
uniform int Function_styleA : hint_range(0, 2, 0);
group_uniforms o;
group_uniforms o.Clamp;
uniform bool TypeClampR = false;
uniform float ClampR = 0.5;
uniform float ClampRMin = 0;
uniform bool TypeClampG = false;
uniform float ClampG = 0.5;
uniform float ClampGMin = 0;
uniform bool TypeClampB = false;
uniform float ClampB = 0.5;
uniform float ClampBMin = 0;
uniform bool TypeClampR2 = false;
uniform float ClampR2 = 0.5;
uniform float ClampRMin2 = 0;
uniform bool TypeClampG2 = false;
uniform float ClampG2 = 0.5;
uniform float ClampGMin2 = 0;
uniform bool TypeClampB2 = false;
uniform float ClampB2 = 0.5;
uniform float ClampBMin2 = 0;
group_uniforms o.Step;
uniform bool TypeStepR = false;
uniform float StepR : hint_range(-10.0, 10.0, 0.01) = 0.5;
uniform float StepRMin : hint_range(-10.0, 10.0, 0.01) = 0;
uniform bool TypeStepG = false;
uniform float StepG : hint_range(-10.0, 10.0, 0.01) = 0.5;
uniform float StepGMin : hint_range(-10.0, 10.0, 0.01) = 0;
uniform bool TypeStepB = false;
uniform float StepB : hint_range(-10.0, 10.0, 0.01) = 0.5;
uniform float StepBMin : hint_range(-10.0, 10.0, 0.01) = 0;
uniform bool TypeStepR2 = false;
uniform float StepR2 : hint_range(-10.0, 10.0, 0.01) = 0.5;
uniform float StepRMin2 : hint_range(-10.0, 10.0, 0.01) = 0;
uniform bool TypeStepG2 = false;
uniform float StepG2 : hint_range(-10.0, 10.0, 0.01) = 0.5;
uniform float StepGMin2 : hint_range(-10.0, 10.0, 0.01) = 0;
uniform bool TypeStepB2 = false;
uniform float StepB2 : hint_range(-10.0, 10.0, 0.01) = 0.5;
uniform float StepBMin2 : hint_range(-10.0, 10.0, 0.01) = 0;
group_uniforms o.Distance;
uniform bool TypedistanceR = false;
uniform float distanceR : hint_range(-10.0, 10.0, 0.01) = 0.5;
uniform float distanceRMin : hint_range(-10.0, 10.0, 0.01) = 0;
uniform bool TypedistanceG = false;
uniform float distanceG : hint_range(-10.0, 10.0, 0.01) = 0.5;
uniform float distanceGMin : hint_range(-10.0, 10.0, 0.01) = 0;
uniform bool TypedistanceB = false;
uniform float distanceB : hint_range(-10.0, 10.0, 0.01) = 0.5;
uniform float distanceBMin : hint_range(-10.0, 10.0, 0.01) = 0;
uniform bool TypedistanceA = false;
uniform float distanceA : hint_range(-10.0, 10.0, 0.01) = 0.5;
uniform float distanceAMin : hint_range(-10.0, 10.0, 0.01) = 0;
uniform bool TypedistanceR2 = false;
uniform float distanceR2 : hint_range(-10.0, 10.0, 0.01) = 0.5;
uniform float distanceRMin2 : hint_range(-10.0, 10.0, 0.01) = 0;
uniform bool TypedistanceG2 = false;
uniform float distanceG2 : hint_range(-10.0, 10.0, 0.01) = 0.5;
uniform float distanceGMin2 : hint_range(-10.0, 10.0, 0.01) = 0;
uniform bool TypedistanceB2 = false;
uniform float distanceB2 : hint_range(-10.0, 10.0, 0.01) = 0.5;
uniform float distanceBMin2 : hint_range(-10.0, 10.0, 0.01) = 0;
uniform bool TypedistanceA2 = false;
uniform float distanceA2 : hint_range(-10.0, 10.0, 0.01) = 0.5;
uniform float distanceAMin2 : hint_range(-10.0, 10.0, 0.01) = 0;
void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	float VR;
	float VR2;
	float VG = sin((TIME * multiplierG) + timeG);
	float VG2 = sin((TIME * multiplierG2) + timeG2);
	float VB = sin((TIME * multiplierB) + timeB);
	float VB2 = sin((TIME * multiplierB2) + timeB2);
	float VA = sin((TIME * multiplierA) + timeA);
	float VA2 = sin((TIME * multiplierA2) + timeA2);
	if (Function_styleR == 0){
		VR = sin((TIME * multiplierR) + timeR);
		VR2 = sin((TIME * multiplierR2) + timeR2);
	}
	else if(Function_styleR == 1){
		VR = cos((TIME * multiplierR) + timeR);
		VR2 = cos((TIME * multiplierR2) + timeR2);
	}
	else if(Function_styleR == 2){
		VG = tan((TIME * multiplierR) + timeR);
		VG2 = tan((TIME * multiplierR) + timeR2);
	}
	if (Function_styleR == 0){
		VG = sin((TIME * multiplierG) + timeG);
		VG2 = sin((TIME * multiplierG2) + timeG2);
	}
	else if(Function_styleG == 1){
		VG = cos((TIME * multiplierG) + timeG);
		VG2 = cos((TIME * multiplierG2) + timeG2);
	}
	else if(Function_styleG == 2){
		VG = tan((TIME * multiplierG) + timeG);
		VG2 = tan((TIME * multiplierG2) + timeG2);
	}
	if (Function_styleR == 0){
		VB = sin((TIME * multiplierB) + timeB);
		VB2 = sin((TIME * multiplierB2) + timeB2);
	}
	else if(Function_styleB == 1){
		VB = cos((TIME * multiplierB) + timeB);
		VB2 = cos((TIME * multiplierB2) + timeB2);
	}
	else if(Function_styleB == 2){
		VB = tan((TIME * multiplierB) + timeB);
		VB2 = tan((TIME * multiplierB2) + timeB2);
	}
	if (Function_styleR == 0){
		VA = sin((TIME * multiplierA) + timeA);
		VA2 = sin((TIME * multiplierA2) + timeA2);
	}
	else if(Function_styleA == 1){
		VA = cos((TIME * multiplierA) + timeA);
		VA2 = cos((TIME * multiplierA2) + timeA2);
	}
	else if(Function_styleA == 2){
		VA = tan((TIME * multiplierA) + timeA);
		VA2 = tan((TIME * multiplierA2) + timeA2);
	}
	VR *= ValuemultiplierR;
	VR2 *= ValuemultiplierR2;
	VG *= ValuemultiplierG;
	VG2 *= ValuemultiplierG2;
	VB *= ValuemultiplierB;
	VB2 *= ValuemultiplierB2;
	VA *= ValuemultiplierA;
	VA2 *= ValuemultiplierA2;
	if (One == true){
		VB2 = VB;
		VR2 = VR;
		VG2 = VG;
		VA2 = VA;
	}
	if (TypeClampR == true){
		VR = clamp(VR,ClampRMin,ClampR);
	}
	if (TypeClampG == true){
		VG = clamp(VR,ClampGMin,ClampG);
	}
	if (TypeClampB == true){
		VB = clamp(VR,ClampBMin,ClampB);
	}
	if (TypeClampR2 == true){
		VR2 = clamp(VR2,ClampRMin2,ClampR2);
	}
	if (TypeClampG2 == true){
		VG2 = clamp(VR2,ClampGMin2,ClampG2);
	}
	if (TypeClampB == true){
		VB2 = clamp(VR2,ClampBMin2,ClampB2);
	}
	
	if (TypedistanceR == true){
		VR = distance(vec2(distanceR),UV * distanceRMin);
	}
	if (TypedistanceG == true){
		VG = distance(vec2(distanceG),UV * distanceGMin);
	}
	if (TypedistanceB == true){
		VB = distance(vec2(distanceB),UV * distanceBMin);
	}
	if (TypedistanceA == true){
		VA = distance(vec2(distanceA),UV * distanceAMin);
	}
	if (TypedistanceR2 == true){
		VR2 = distance(vec2(distanceR2),UV * distanceRMin2);
	}
	if (TypedistanceG == true){
		VG2 = distance(vec2(distanceG2),UV * distanceGMin2);
	}
	if (TypedistanceB == true){
		VB2 = distance(vec2(distanceB2),UV * distanceBMin2);
	}
	if (TypedistanceA == true){
		VA2 = distance(vec2(distanceA2),UV * distanceAMin2);
	}
	
	if (TypeStepR == true){
		VR = step(VR * StepRMin ,StepR);
	}
	if (TypeStepG == true){
		VG = step(VG * StepGMin ,StepG);
	}
	if (TypeStepB == true){
		VB = step(VB * StepBMin ,StepB);
	}
	if (TypeStepR2 == true){
		VR2 = step(VR2 * StepRMin2 ,StepR2);
	}
	if (TypeStepG2 == true){
		VG2 = step(VG2 * StepGMin2 ,StepG2);
	}
	if (TypeStepB2 == true){
		VB2 = step(VB2 * StepBMin2 ,StepB2);
	}
	if (oneValue == false){
		COLOR.r = mix(UV.x * RValue * VR,colo.r * (VR2 + R),MixR);
		COLOR.g = mix(UV.x * GValue * VG,colo.g * (VG2 + G),MixG);
		COLOR.b = mix(UV.x * BValue * VB,colo.b * (VB2 + B),MixB);
		if (setAlpha == true){
		COLOR.a = mix(UV.x * AValue * VA,colo.b * (VA2 + A),MixA);
		}
	}
	else{
		COLOR.r = mix(UV.x * RValue * VR,colo.r * (VR2 + R),MixR);
		COLOR.g = mix(UV.x * RValue * VR,colo.r * (VR2 + R),MixR);
		COLOR.b = mix(UV.x * RValue * VR,colo.r * (VR2 + R),MixR);
		if (setAlpha == true){
		COLOR.a = mix(UV.x * AValue * VA,colo.b * (VA2 + A),MixA);
		}
	}
}

//void light() {
//	// Called for every pixel for every light affecting the CanvasItem.
//	// Uncomment to replace the default light processing function with this one.
//}
