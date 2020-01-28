Shader "Custom/OpacityLarge"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {} // Texture from Renderer
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0 // ?
		_SeeThroughAlpha ("Alpha Value SeeThrough", Range(0, 1)) = 0.3 // Alpha-Value inside Circle
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
			

			v2f vert(appdata_t IN) // Vertex-Function copied from Sprites-Default
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
			fixed _AlphaSplitEnabled;
			float _SeeThroughAlpha;
			// uniform to set in code
			uniform int _UseSeeThrough;
			uniform int _SeeThroughLength; // max 64
			uniform float centers[128];
			uniform float radii[64];

			fixed4 frag(v2f IN) : SV_Target
			{
				// Grab Pixel-Value from Input-Texture
				fixed4 c = tex2D (_MainTex, IN.texcoord) * IN.color;

				// Check if SeeThrough-Alpha should be applied
				if (_UseSeeThrough == 1 && _SeeThroughLength > 0)
				{
					// Create Array for Positions
					float2 midpoints[64];
					// Load Positions to Vector2s
					for (int i = 0; i < _SeeThroughLength; i++)
						midpoints[i] = float2(centers[i*2], centers[i*2+1]);

					// Check if Pixel is inside a Circle
					if (IsInAnyCircleLarge(IN.vertex, midpoints, radii, _SeeThroughLength))
					{
						c.rgb *= c.a; // Apply alpha from texture
						c.a = _SeeThroughAlpha; // TRUE: Apply Alpha
					}
				}
				c.rgb *= c.a; // Apply Alpha to RGB (copied from Sprites-Default)
				return c;
			}
		ENDCG
		}
	}
}