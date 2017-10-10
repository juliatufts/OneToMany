Shader "Unlit/SkyBaller"
{
	Properties
	{
		_ColorPhase("Color Phase (XYZ) Color Offset (W)", Vector) = (0,0,0,.5)
		_ColorFreq("Color Freq (XYZ) Color Amp (W)", Vector) = (1,1,1,.5)
		_Shape ("Speed (X) GlobalFreq(Y) BonusWarp(W)", Vector) = (1,1,1,.01)

	}
		SubShader
	{
		Tags{ "Queue" = "Background" "RenderType" = "Background" "PreviewType" = "Skybox" }
		Cull Off ZWrite Off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			float4 _ColorPhase;
			float4 _ColorFreq;
			float4 _Shape;

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 pos : TEXCOORD0;
			};
						
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.pos = v.vertex;
				return o;
			}
			
			fixed4 frag(v2f i) : SV_Target
			{
				float3 pos = i.pos;
				pos *= _Shape.y;
				float3 a = float3(
					atan2(i.pos.x, i.pos.y),
					atan2(i.pos.y, i.pos.z),
					atan2(i.pos.z, i.pos.x)
				);
				float3 d = float3(length(pos.xy), length(pos.yz), length(pos.zx));
				float time = _Time.y*_Shape.x;
				float f = 
					(pos.x*7.+sin(pos.y*4.-time*.3+d.y*2) * (sin(pos.z*1.3 - time*.15)*_Shape.w + 1.) + 3. + time*.5) +
					(pos.y*5.+sin(pos.z*3.-time+d.z*3) * (sin(pos.x + time*.1)*_Shape.w + 1.) - time*.6) +
					(pos.z*4.+sin(pos.x*2.+time*.7+d.x*1.5) * (sin(pos.y*1.3 - time*.23)*_Shape.w + 1.) + time*1.1);

				fixed3 col = sin(f*_ColorFreq.xyz + _ColorPhase.xyz)*_ColorFreq.w + _ColorPhase.w;

				return float4(col, 1.);
			}
			ENDCG
		}
	}
}
