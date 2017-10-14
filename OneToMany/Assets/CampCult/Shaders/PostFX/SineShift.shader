Shader "Camp Cult/Displacement/SineShift" {
Properties {
	_MainTex("MainTex", 2D) = "white" {} 
	_X("X freq/phase/amp", Vector) = (0,0,0,0)
	_Y("Y freq/phase/amp", Vector) = (0,0,0,0)
}
SubShader{
	Pass{
	ZTest Always Cull Off ZWrite Off
	Fog{ Mode off }
CGPROGRAM
#include "UnityCG.cginc"
#pragma vertex vert_img
#pragma fragment frag

uniform float4 _X;
uniform float4 _Y;
uniform sampler2D _MainTex;
uniform float4 _MainTex_TexelSize;

fixed4 frag(v2f_img i) : COLOR
{
	i.uv += float2(sin(i.uv.y*_X.x+_X.y)*_X.z, sin(i.uv.x*_Y.x + _Y.y)*_Y.z)*_MainTex_TexelSize.xy;
	return  tex2D(_MainTex, i.uv); 
}
ENDCG
}
}
FallBack "Unlit"
}
