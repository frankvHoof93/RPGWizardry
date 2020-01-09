Shader "Custom/TextureDecal"
{    
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_DecalTex("Decal Texture", 2D) = "white" {}
		[HideInInspector] _RendererColor("RendererColor", Color) = (1,1,1,1)
		_DecalUV("Decal UVs", Vector) = (1,1,1,1)
		_Decal2UV("Decal2 UVs", Vector) = (1,1,1,1)
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
		#pragma surface surf NoLighting alpha:blend noshadow noambient nolightmap nodynlightmap nodirlightmap nofog noforwardadd noshadowmask 
		#include "UnitySprites.cginc"

		fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, fixed atten)
		{
			return fixed4(s.Albedo, s.Alpha);
		}

		sampler2D _DecalTex;
		fixed4 _DecalUV;
		fixed4 _Decal2UV;
		
		uniform float4 UVs[32];
		uniform int splatCount;

		struct Input
		{ 
			float2 uv_MainTex;
			float2 uv_DecalTex;
		};

		void surf(Input IN, inout SurfaceOutput o)
		{
			half4 c = SampleSpriteTexture(IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;

			[loop]
			for (int i = 0; i < splatCount; i++)
			{
				fixed4 _UV = UVs[i];
				float2 uv = float2(_UV.x * IN.uv_DecalTex.x + _UV.z, IN.uv_DecalTex.y * _UV.y + _UV.w);
				if (uv.x > 0 && uv.x < 1 && uv.y > 0 && uv.y < 1)
				{
					half4 decal = tex2D(_DecalTex, uv);
					if (decal.a > 0.01)
					{
						// Overwrite (Splat)
						o.Albedo = decal.rgb * decal.a;
						// Blend (Multiply) (Best on white MainTex)
						//o.Albedo = (o.Albedo * o.Alpha) * (decal.rgb * decal.a);
						//decal.a *= o.Alpha;
						// Blend (Additive) (Best on black/transparent MainTex)
						//o.Albedo = o.Albedo * o.Alpha + decal.rgb * decal.a / (o.Alpha + decal.a);
						//decal.a += o.Alpha;
						o.Alpha = decal.a > 1 ? 1 : decal.a;
					}
				}
			}
		}
		ENDCG
	}
	Fallback "Transparent/VertexLit"
}