Shader "Windshield/windshield" {
Properties {
 _Color ("Color", Color) = (1,1,1,1)
 _MainTex ("Albedo (RGB)", 2D) = "white" { }
 _BumpMap ("Normal", 2D) = "white" { }
 _Glossiness ("Smoothness", Range(0,1)) = 0.5
 _Rain ("Rain", 2D) = "zeroRGBA" { }
 _RainOpacity ("Rain Opacity", Range(0,1)) = 0.5
 _Raining ("Raining (On/Off)", Range(0,1)) = 0
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