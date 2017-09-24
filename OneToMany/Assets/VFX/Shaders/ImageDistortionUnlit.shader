Shader "Custom/ImageDistortionUnlit"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
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

                // sample ahead and blend
                float inc = 0.02 * _SinTime.w;
                fixed2 nextUV = fixed2(i.uv.x + inc, i.uv.y + 0.);
                if (nextUV.x > 1.) { nextUV.x -= 1.; }
                if (nextUV.y > 1.) { nextUV.y -= 1.; }

                fixed4 nextCol = tex2D(_MainTex, nextUV);

                col = fixed4((col.w + nextCol.w) / 2.,
                             (col.x + nextCol.x) / 2.,
                             (col.y + nextCol.y) / 2.,
                             (col.z + nextCol.z) / 2.
                             );

				return col;
			}
			ENDCG
		}
	}
}
