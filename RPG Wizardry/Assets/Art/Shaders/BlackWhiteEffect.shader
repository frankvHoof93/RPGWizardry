Shader "Hidden/BlackWhiteEffect"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {} // Texture Received from Camera
		_bwBlend ("Black & White blend", Range(0,1)) = 0 // Float-Value (0-1) that determines BW-amount
    }
    SubShader
    {
		Pass
		{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag

			#include "UnityCG.cginc"

			uniform sampler2D _MainTex;
			uniform float _bwBlend;

			float4 frag(v2f_img IN) : COLOR 
			{
				// Read Pixel from Texture
				float4 c = tex2D(_MainTex, IN.uv); 
 
				// Calculate Luminosity (magic numbers based on human eye color-perception)
				float lum = c.r*.3 + c.g*.59 + c.b*.11; 
				// Create RGB from Luminosity
				float3 bw = float3( lum, lum, lum ); 
 
				// Create output-pixel from input (copy Alpha from Input-Texture)
				float4 result = c; 
				// Set RGB by lerping between original and luminosity-value based on Blend-Amount
				result.rgb = lerp(c.rgb, bw, _bwBlend); 
				return result;
			}
			ENDCG
		}   
    }
}
