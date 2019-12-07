Shader "Custom/SeeThroughMulti1Shader"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		[HideInInspector] _UseSeeThrough ("Use SeeThrough", int) = 0
		[HideInInspector] _SeeThroughCenter1 ("CenterPoint SeeThrough (1)", Vector) = (0,0,0,0)
		[HideInInspector] _SeeThroughCenter2 ("CenterPoint SeeThrough (2)", Vector) = (0,0,0,0)
		[HideInInspector] _SeeThroughRadii ("Radii", Vector) = (0,0,0,0)
		[HideInInspector] _SeeThroughLength ("Amount (Max 4)", int) = 0
		_SeeThroughAlpha ("Alpha Value SeeThrough", Range(0, 1)) = 0.3
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
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 texcoord  : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			

			v2f vert(appdata_t IN)
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
			
			UNITY_INSTANCING_BUFFER_START(Props)
				UNITY_DEFINE_INSTANCED_PROP(fixed4, _SeeThroughCenter1)
				UNITY_DEFINE_INSTANCED_PROP(fixed4, _SeeThroughCenter2)
				UNITY_DEFINE_INSTANCED_PROP(int, _UseSeeThrough)
				UNITY_DEFINE_INSTANCED_PROP(int, _SeeThroughLength)
				UNITY_DEFINE_INSTANCED_PROP(fixed4, _SeeThroughRadii)
			UNITY_INSTANCING_BUFFER_END(Props)

			fixed4 frag(v2f IN) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(IN);
				fixed4 c = tex2D (_MainTex, IN.texcoord) * IN.color;
				if (UNITY_ACCESS_INSTANCED_PROP(Props, _UseSeeThrough) == 1 && UNITY_ACCESS_INSTANCED_PROP(Props, _SeeThroughLength) > 0)
				{
					int len = UNITY_ACCESS_INSTANCED_PROP(Props, _SeeThroughLength);
					float4 vec1 = UNITY_ACCESS_INSTANCED_PROP(Props, _SeeThroughCenter1);
					float4 vec2 = UNITY_ACCESS_INSTANCED_PROP(Props, _SeeThroughCenter2);
					fixed4 radius = UNITY_ACCESS_INSTANCED_PROP(Props, _SeeThroughRadii);
					float2 positions[4] = {float2(vec1.x, vec1.y), float2(vec1.z, vec1.w),float2(vec2.x, vec2.y), float2(vec2.z, vec2.w)};
					float radii[4] = {radius.x, radius.y, radius.z, radius.w};
					if (IsInAnyCircle(IN.vertex, positions, radii, len))
						c.a = _SeeThroughAlpha;
				}
				c.rgb *= c.a;
				return c;
			}
		ENDCG
		}
	}
}