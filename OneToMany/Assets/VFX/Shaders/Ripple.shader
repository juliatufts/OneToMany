Shader "Custom/Ripple" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
        _ContactPoint ("Contact Point", Vector) = (0,0,0)
        _Strength ("Strength", Float) = 1.0
        _TouchRadius ("Touch Radius", Float) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
            float3 worldPos;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
        float3 _ContactPoint;
        float _Strength;
        float _TouchRadius;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_CBUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_CBUFFER_END

		void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Calculate distance between world pos and contact point
            float d = length(IN.worldPos - _ContactPoint);

			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;

            if (d > _TouchRadius)
            {
                o.Albedo = c.rgb * 0.0;
            }

			//o.Albedo = c.rgb * clamp(_Strength, 0.0, 1.0) * (0.5 * sin(30 * d) + 0.5);
            
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
