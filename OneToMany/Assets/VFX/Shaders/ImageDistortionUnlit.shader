Shader "Custom/ImageDistortionUnlit"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Touch ("Touch", float) = 0.0
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"
			#define PI 3.14159265359
			#define count 3.0

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);

				float2 uv = i.uv;
				float4 c = float4(0.0, 0.0, 0.0, 0.0);
				float d = length(uv);
				float a = atan2(uv.x, uv.y);
				float t = _Time.y;
				float f = 1;
				float amp = 1;

				for (float j = 0.; j <= PI*2.; j += PI / count*2.)
				{
					uv.x = cos(a)*(d + d*amp*sin(a*f + t + j));
					uv.y = sin(a)*(d + d*amp*cos(a*f + t + j));

					c += tex2D(_MainTex, abs(((uv - .5) % (2.0)) - 1.0));
				}
				col = (c*3.0 / c.a);
				
				//col = tex2D(_MainTex, i.uv);
				return col;
			}
			ENDCG
		}
	}
}
