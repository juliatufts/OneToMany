Shader "Custom/MeshGlow"
{
	Properties
	{
		_Color ("Color", Color) = (1,1,1,1)
		_Density("Density", Float) = 0.25
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" "Queue"="Transparent+101" "DisableBatching"="True"}
		LOD 100
		Cull Off
		Blend One One
		ZWrite Off
		ZTest Off
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.5

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 scrPos : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 clipPos : TEXCOORD1;
				float3 worldPos: TEXCOORD3;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.clipPos = o.vertex = UnityObjectToClipPos(v.vertex);
   				o.scrPos=ComputeScreenPos(o.vertex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				return o;
			}
			float4 _Color;
			float _Density;
			sampler2D _CameraDepthTexture;
			sampler2D _CameraDepthNormalsTexture;
			float4 frag (v2f i, int face : VFACE) : SV_Target
			{
				float4 depthSample = tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.scrPos));
				
				//float depthSample = 0;
			    //float3 normalValues = (0, 0, 0);
				//DecodeDepthNormal(tex2D(_CameraDepthNormalsTexture, UNITY_PROJ_COORD(i.scrPos)), depthSample, normalValues);

				float depth = LinearEyeDepth(depthSample);
				float pDivide = 1/i.clipPos.w;
				float3 worldDir = (i.worldPos-_WorldSpaceCameraPos)*pDivide;
				float bgDist = length(worldDir*depth);

				float dist = length(i.worldPos-_WorldSpaceCameraPos);
				float amt = min(dist,bgDist);

				float4 col = _Color * amt * _Density;
				if(face>0) {
					col = -col;
				}
				return col;
			}
			ENDCG
		}
	}
}