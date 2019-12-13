Shader "Hidden/CircleOverlay"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {} // Texture Received from Camera
		_OverlayColor ("Overlay (Color)", Color) = (0,0,0,1) // Color for Overlay (outside of Circle)
		_PlayerPos ("Player Position X", Vector) = (0,0,0,0) // Position for Circle-Center
		_CircleRadius ("Circle Radius", float) = 100 // Radius for Circle
    }
    SubShader
    {
		Pass{
			CGPROGRAM
			#pragma vertex vert_img // Default Unity-Vertex-Function for Images
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "CircleFunction.cginc"

			uniform sampler2D _MainTex;
			float4 _OverlayColor;
			float4 _PlayerPos;
			float _CircleRadius;

			float4 frag(v2f_img IN) : COLOR 
			{
				return IsInCircle(IN.pos, _PlayerPos.xy, _CircleRadius) // Check if Position for Pixel is in Circle
				? tex2D(_MainTex, IN.uv) // TRUE: Return original color (position is within Circle)
				: _OverlayColor; // FALSE: Return Overlay-Color (Position is not within Circle)
			}
			ENDCG
		}   
    }
}
