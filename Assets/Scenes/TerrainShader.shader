//UNITY_SHADER_NO_UPGRADE

Shader "Unlit/TerrainShader"
{
Properties
	{
		ColorIndex("ColorIndex",float) = 4.0
	}
	SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			float ColorIndex = 3;
            
           
			struct vertIn
			{
				float4 vertex : POSITION;
				float4 color : COLOR;

			};

			struct vertOut
			{
				float4 vertex : SV_POSITION;
				float4 color : COLOR;

			};
            
			// Implementation of the vertex shader
			vertOut vert(vertIn v)
			{
				vertOut o;
				if( v.vertex.y > 0){
                    v.color.r = (v.vertex.y/ColorIndex);
                    v.color.b = (v.vertex.y/ColorIndex);
                    if(v.color.r > 0.7 || v.color. b > 0.7){
                     v.color.g = (v.vertex.y/ColorIndex);
                    }
                   
				
				}
			v.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
			o.vertex = v.vertex;
               o.color = v.color;
				return o;
			}

			// Implementation of the fragment shader
			fixed4 frag(vertOut v) : SV_Target
			{
				return v.color;
			}
			ENDCG
		}
	}
}
