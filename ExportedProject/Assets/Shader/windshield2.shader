Shader "Custom/windshield2" {
Properties {
 _MainTex ("Src Texture", 2D) = "white" { }
 _Dry ("Dry Color", Color) = (1,1,1,1)
 _Wet ("Wet Color", Color) = (1,1,1,1)
 _AlphaDist ("AlphaDist", Float) = 1
 _DryingSpeed ("DryingSpeed", Float) = 0.01
 _FrontUV ("FrontUV", Vector) = (0,0,1,0.4)
 _SideUV ("SideUV", Vector) = (0,0.4,1,0.7)
 _RearUV ("RearUV", Vector) = (0,0.7,1,1)
 _FrontFlow ("FrontFlow", Vector) = (0,-10,0,0)
 _SideFlow ("SideFlow", Vector) = (0,-10,0,0)
 _RearFlow ("RearFlow", Vector) = (0,-10,0,0)
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