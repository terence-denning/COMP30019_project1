//UNITY_SHADER_NO_UPGRADE

Shader "Unlit/WaveShader"
{
    
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		WaveSpeed ("WaveSpeed", Float) = 2.0
		WaveReduct("WaveReduct",Float) = 2.0
	}
	SubShader
	{
		Pass
		{
			Cull Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
    
			uniform sampler2D _MainTex;	
			float WaveSpeed;
			float WaveReduct;
            
			struct vertIn
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				
			};

			struct vertOut
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			// Implementation of the vertex shader
			vertOut vert(vertIn v)
			{
				// Displace the original vertex in model space
				float4 displacement = float4(0.0f,sin(_Time.y*(WaveSpeed)+v.vertex.x+v.vertex.z)/WaveReduct, 0.0f, 0.0f);
				float2 uvdisplacement = float2(cos(_Time.y + v.uv.x)*0.1, sin(_Time.y+v.uv.y)*0.1);
				vertOut o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.vertex += displacement;
				v.uv += uvdisplacement;
				o.uv = v.uv;    
				return o;
			}
			
			// Implementation of the fragment shader
			fixed4 frag(vertOut v) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, v.uv);
				return col;
			}
			ENDCG
		}
	}
}