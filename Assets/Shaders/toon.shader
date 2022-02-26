Shader "Custom/CurvedToon"
{
	//show values to edit in inspector
	Properties {
        [Header(Base Parameters)]
		_Color ("Tint", Color) = (0, 0, 0, 1)
		_MainTex ("Texture", 2D) = "white" {}
		_Curvature("Curvature", Float) = 0.001
		[HDR] _Emission ("Emission", color) = (0 ,0 ,0 , 1)

        [Header(Lighting Parameters)]
		_ShadowColor("Shadow Color", Color) = (0.66, 0.66, 0.66, 1)
		_Outline("Outline Color", Color) = (1, 1, 1, 1)
		_OutlineWidth("Outline Width", Float) = 0.1
		_OutlineLimits("Outline Direction", Vector) = (0, -0.5, -0.5, 1)
		_OutlineBrightIntensity("Outline Intensity", Range(0,1)) = 0.17
	}
	SubShader {
		//the material is completely non-transparent and is rendered at the same time as the other opaque geometry
		Tags{ "RenderType"="Opaque"}

		CGPROGRAM

		//the shader is a surface shader, meaning that it will be extended by unity in the background to have fancy lighting and other features
		//our surface shader function is called surf and we use our custom lighting model
		//fullforwardshadows makes sure unity adds the shadow passes the shader might need
		#pragma surface surf Stepped vertex:vert addshadow
		#pragma target 3.0

		sampler2D _MainTex;
		fixed4 _Color;
		float _Curvature;
		half3 _Emission;

        float3 _ShadowColor;
		float3 _Outline;
		float _OutlineWidth;
		float3 _OutlineLimits;
		float _OutlineBrightIntensity;

		//our lighting function. Will be called once per light
		float4 LightingStepped(SurfaceOutput s, float3 lightDir, half3 viewDir, float shadowAttenuation){
			//how much does the normal point towards the light?
			float towardsLight = dot(s.Normal, lightDir);
            // make the lighting a hard cut
            float towardsLightChange = fwidth(towardsLight);
            float lightIntensity = smoothstep(0, towardsLightChange, towardsLight);

        #ifdef USING_DIRECTIONAL_LIGHT
            //for directional lights, get a hard vut in the middle of the shadow attenuation
            float attenuationChange = fwidth(shadowAttenuation) * 0.5;
            float shadow = smoothstep(0.5 - attenuationChange, 0.5 + attenuationChange, shadowAttenuation);
        #else
            //for other light types (point, spot), put the cutoff near black, so the falloff doesn't affect the range
            float attenuationChange = fwidth(shadowAttenuation);
            float shadow = smoothstep(0, attenuationChange, shadowAttenuation);
        #endif
            lightIntensity = lightIntensity * shadow;

            //calculate shadow color and mix light and shadow based on the light. Then tint it based on the light color
            float3 shadowColor = s.Albedo * _ShadowColor;
            float4 color;
			float towardsView = dot(s.Normal, normalize(viewDir));
			if (towardsView < _OutlineWidth && s.Normal.x > _OutlineLimits.x && s.Normal.y > _OutlineLimits.y && s.Normal.z > _OutlineLimits.z)
				color.rgb = lerp(s.Albedo, _Outline, _OutlineBrightIntensity) ;
            else 
				color.rgb = lerp(shadowColor, s.Albedo, lightIntensity) * _LightColor0.rgb;
            color.a = s.Alpha;
            return color;
		}


		//input struct which is automatically filled by unity
		struct Input {
			float2 uv_MainTex;
		};

		// This is where the curvature is applied
		void vert(inout appdata_full v)
		{
			// Transform the vertex coordinates from model space into world space
			float4 vv = mul(unity_ObjectToWorld, v.vertex);

			// Now adjust the coordinates to be relative to the camera position
			vv.xyz -= _WorldSpaceCameraPos.xyz;

			// Reduce the y coordinate (i.e. lower the "height") of each vertex based
			// on the square of the distance from the camera in the z axis, multiplied
			// by the chosen curvature factor
			vv = float4(0.0f, (vv.z * vv.z) * -_Curvature, 0.0f, 0.0f);

			// Now apply the offset back to the vertices in model space
			v.vertex += mul(unity_WorldToObject, vv);
		}


		//the surface shader function which sets parameters the lighting function then uses
		void surf (Input i, inout SurfaceOutput o) {
			//sample and tint albedo texture
			fixed4 col = tex2D(_MainTex, i.uv_MainTex);
			col *= _Color;
			o.Albedo = col.rgb;

			o.Emission = _Emission;
		}
		ENDCG
	}
	FallBack "Standard"
}