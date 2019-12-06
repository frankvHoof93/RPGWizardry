Shader "Custom/SeeThroughShader"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		[MaterialToggle] _UseSeeThrough ("Use SeeThrough", Float) = 0
		_SeeThroughAlpha ("Alpha Value SeeThrough", Range(0, 1)) = 0.3
		_SeeThroughCenter ("CenterPoint SeeThrough", Vector) = (0,0,0,0)
		_SeeThroughRadius ("Radius SeeThrough", float) = 30
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ PIXELSNAP_ON
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			#include "CircleFunction.cginc"
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 texcoord  : TEXCOORD0;
			};
			

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}

			sampler2D _MainTex;
			sampler2D _AlphaTex;
			float _UseSeeThrough;
			float _AlphaSplitEnabled;
			float _SeeThroughAlpha;
			float _SeeThroughRadius;

			
        UNITY_INSTANCING_BUFFER_START(Props)
           UNITY_DEFINE_INSTANCED_PROP(fixed4, _SeeThroughCenter)
        UNITY_INSTANCING_BUFFER_END(Props)

			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);

#if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
				if (_AlphaSplitEnabled)
					color.a = tex2D (_AlphaTex, uv).r;
#endif //UNITY_TEXTURE_ALPHASPLIT_ALLOWED

				return color;
			}

			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 c = SampleSpriteTexture (IN.texcoord) * IN.color;
				if (_UseSeeThrough == 1)
					c.a = IsInCircle(IN.vertex, UNITY_ACCESS_INSTANCED_PROP(Props, _SeeThroughCenter).xy, _SeeThroughRadius) ? _SeeThroughAlpha : c.a;
				c.rgb *= c.a;
				return c;
			}
		ENDCG
		}
	}
}