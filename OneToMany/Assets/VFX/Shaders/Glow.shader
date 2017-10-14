Shader "Custom/MeshGlow"
{
	Properties
	{
		_Color ("Color", Color) = (1,1,1,1)
		_Density("Density", Float) = 0.25
		_Scale("Scale", Float) = 1.0
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent+101" "DisableBatching"="True"}
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

			#pragma multi_compile CAMERA_DEPTH CAMERA_DEPTHNORMALS

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
				float3 camPos : TEXCOORD4;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;


			sampler2D _CameraDepthTexture;
			sampler2D _CameraDepthNormalsTexture;

			// Z buffer depth to linear 0-1 depth
			// Handles orthographic projection correctly
			float LinearizeDepth(float z)
			{
				float isOrtho = unity_OrthoParams.w;
				float isPers = 1.0 - unity_OrthoParams.w;
				z *= _ZBufferParams.x;
				return (1.0 - isOrtho * z) / (isPers * z + _ZBufferParams.y);
			}

			// Depth/normal sampling functions
			float SampleDepthProj(float4 uv)
			{
			#if defined(CAMERA_DEPTH)
				float d = LinearizeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(uv)));
			#elif defined(CAMERA_DEPTHNORMALS)
				float4 cdn = tex2Dproj(_CameraDepthNormalsTexture, UNITY_PROJ_COORD(uv));
				float d = DecodeFloatRG(cdn.zw);
			#else
				float d = 0;
			#endif
				return d * _ProjectionParams.z;
			}
			
			v2f vert (appdata v)
			{
				v2f o;
				o.clipPos = o.vertex = UnityObjectToClipPos(v.vertex);
   				o.scrPos=ComputeScreenPos(o.clipPos);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.camPos = UnityObjectToViewPos(v.vertex).xyz;
				return o;
			}
			float4 _Color;
			float _Density;
			float _Scale;

			float4 frag (v2f i, int face : VFACE) : SV_Target
			{

				float depth = SampleDepthProj(i.scrPos);
				float3 dir = i.camPos/i.camPos.z;
				float bgDist = length(dir * depth);

				float dist = length(i.camPos);
				float amt = min(dist,bgDist);

				float4 col = _Color * amt * _Density * _Scale;
				if(face>0) {
					col = -col;
				}
				return col;
			}
			ENDCG
		}
	}
}