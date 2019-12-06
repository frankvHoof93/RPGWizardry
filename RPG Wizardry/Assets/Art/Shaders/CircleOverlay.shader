Shader "Hidden/CircleOverlay"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
		_OverlayColor ("Overlay (Color)", Color) = (0,0,0,1)
		_PlayerPos ("Player Position X", Vector) = (0,0,0,0)
		_CircleRadius ("Circle Radius", float) = 100
    }
    SubShader
    {
		Pass{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "CircleFunction.cginc"

			uniform sampler2D _MainTex;
			float4 _OverlayColor;
			float4 _PlayerPos;
			float _CircleRadius;

			float4 frag(v2f_img IN) : COLOR 
			{
				return IsInCircle(IN.pos, _PlayerPos.xy, _CircleRadius) 
				? tex2D(_MainTex, IN.uv) // Return original color (position is within Circle)
				: _OverlayColor; // Return Overlay-Color (Position is not within Circle)
			}
			ENDCG
		}   
    }
}
