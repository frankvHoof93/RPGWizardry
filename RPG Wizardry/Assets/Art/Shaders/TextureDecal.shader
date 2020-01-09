Shader "Custom/TextureDecal"
{    
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_DecalTex("Decal Texture", 2D) = "white" {}
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
		[HideInInspector] _RendererColor("RendererColor", Color) = (1,1,1,1)
		[HideInInspector] _Flip("Flip", Vector) = (1,1,1,1)
		_Decal1("Decal1", Vector) = (1,1,1,1)
	}

		SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		CGPROGRAM
		#pragma surface surf Lambert vertex:vert nofog nolightmap nodynlightmap keepalpha noinstancing
		#pragma multi_compile_local _ PIXELSNAP_ON
		#include "UnitySprites.cginc"

		sampler2D _DecalTex;
		fixed4 _Decal1;

		struct Input
		{
			float2 uv_MainTex;
			float2 vert;
		};

		void vert(inout appdata_full v, out Input o)
		{
			v.vertex = UnityFlipSprite(v.vertex, _Flip);

			#if defined(PIXELSNAP_ON)
			v.vertex = UnityPixelSnap(v.vertex);
			#endif

			UNITY_INITIALIZE_OUTPUT(Input, o);
			o.vert = v.vertex;
		}

		void surf(Input IN, inout SurfaceOutput o)
		{
			half4 c = SampleSpriteTexture(IN.uv_MainTex);
			half4 decal = tex2D(_DecalTex, IN.uv_MainTex);
			if (IN.vert.x == _Decal1.x)
			{
				o.Albedo = float3(255, 0, 0);
			}
			else
			{
				o.Albedo = float3(255,255,255);
			}
			o.Alpha = c.a;
		}
		ENDCG
	}

		Fallback "Transparent/VertexLit"
}