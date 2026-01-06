Shader "Car/GlassReflect2DNew" {
Properties {
 _Color ("Main Color", Color) = (1,1,1,1)
 _ReflectColor ("Reflection Color", Color) = (1,1,1,0.5)
 _MainTex ("Base (RGB) RefStrength (A)", 2D) = "white" { }
 _Cube ("Reflection Cubemap", CUBE) = "_Skybox" { }
 _BumpMap ("Bumpmap (RGB Trans)", 2D) = "bump" { }
 _2DReflection ("Reflection (RGB)", CUBE) = "grey" { }
 _FresnelPower ("_FresnelPower", Range(0.05,5)) = 0.75
}
	//DummyShaderTextExporter
	
	SubShader{
		Tags { "RenderType" = "Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Lambert
#pragma target 3.0
		sampler2D _MainTex;
		struct Input
		{
			float2 uv_MainTex;
		};
		void surf(Input IN, inout SurfaceOutput o)
		{
			float4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
		}
		ENDCG
	}
}