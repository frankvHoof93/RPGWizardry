Shader "Custom/SeeThroughShader"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {} // Texture from Renderer
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0 // ?
		[HideInInspector] _UseSeeThrough ("Use SeeThrough", Float) = 0 // Whether to enable SeeThrough
		[HideInInspector] _SeeThroughCenter ("CenterPoint SeeThrough", Vector) = (0,0,0,0) // Circle-Center for SeeThrough (x, y, 0, 0)
		_SeeThroughAlpha ("Alpha Value SeeThrough", Range(0, 1)) = 0.3 // Alpha-Value inside Circle
		_SeeThroughRadius ("Radius SeeThrough", float) = 30 // Radius for Circle
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
			#pragma multi_compile_instancing // Enable GPU-Instancing
			#include "UnityCG.cginc"
			#include "CircleFunction.cginc"
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 texcoord  : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			

			v2f vert(appdata_t IN) // Vertex-Function copied from Sprites-Default, GPU-Instancing added
			{
				v2f OUT;
                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_TRANSFER_INSTANCE_ID(IN, OUT);
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
			fixed _SeeThroughRadius;

			
			UNITY_INSTANCING_BUFFER_START(Props) // Set-Up GPU-Instanced Variables
				UNITY_DEFINE_INSTANCED_PROP(fixed4, _SeeThroughCenter)
				UNITY_DEFINE_INSTANCED_PROP(float, _UseSeeThrough)
			UNITY_INSTANCING_BUFFER_END(Props)

			fixed4 frag(v2f IN) : SV_Target
			{
				// Set-Up Instance-ID
				UNITY_SETUP_INSTANCE_ID(IN); 
				// Grab Pixel-Value from Input-Texture
				fixed4 c = tex2D (_MainTex, IN.texcoord) * IN.color; 

				// Check if SeeThrough-Alpha should be applied
				if (UNITY_ACCESS_INSTANCED_PROP(Props, _UseSeeThrough) == 1 && IsInCircle(IN.vertex, UNITY_ACCESS_INSTANCED_PROP(Props, _SeeThroughCenter).xy, _SeeThroughRadius))
					c.a = _SeeThroughAlpha; // TRUE: Apply Alpha
				c.rgb *= c.a; // Apply Alpha to RGB (copied from Sprites-Default)
				return c;
			}
		ENDCG
		}
	}
}