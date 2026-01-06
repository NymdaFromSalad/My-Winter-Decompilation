Shader "Nature/Tree Soft Occlusion Leaves" {
Properties {
 _Color ("Main Color", Color) = (1,1,1,1)
 _MainTex ("Main Texture", 2D) = "white" { }
 _Cutoff ("Alpha cutoff", Range(0.25,0.9)) = 0.5
 _BaseLight ("Base Light", Range(0,1)) = 0.35
 _AO ("Amb. Occlusion", Range(0,10)) = 2.4
 _Occlusion ("Dir Occlusion", Range(0,20)) = 7.5
[HideInInspector]  _TreeInstanceColor ("TreeInstanceColor", Vector) = (1,1,1,1)
[HideInInspector]  _TreeInstanceScale ("TreeInstanceScale", Vector) = (1,1,1,1)
[HideInInspector]  _SquashAmount ("Squash", Float) = 1
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