//UNITY_SHADER_NO_UPGRADE

Shader "Unlit/TerrainShader"
{
Properties
	{
		ColorIndex("ColorIndex",float) = 8.0
		_PointLightColor("Point Light Color", Color) = (0, 0, 0)
		_PointLightPosition("Point Light Position", Vector) = (0.0, 0.0, 0.0)
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
			uniform float3 _PointLightColor;
			uniform float3 _PointLightPosition;
            
           
			struct vertIn
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float4 normal : NORMAL;

			};

			struct vertOut
			{
				float4 vertex : SV_POSITION;
				float4 color : COLOR;
				float4 worldVertex : TEXCOORD0;
				float3 worldNormal : TEXCOORD1;

			};
            
			// Implementation of the vertex shader
			vertOut vert(vertIn v)
			{
				vertOut o;
				float4 worldVertex = mul(unity_ObjectToWorld, v.vertex);
				float3 worldNormal = normalize(mul(transpose((float3x3)unity_WorldToObject), v.normal.xyz));
				
				if( v.vertex.y > 3){
                    v.color.r = (v.vertex.y/ColorIndex);
                    v.color.b = (v.vertex.y/ColorIndex);
                    if(v.color.r > 0.7 || v.color. b > 0.7){
                     v.color.g = (v.vertex.y/ColorIndex);
                    }
				}
                v.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
                o.worldVertex = worldVertex;
				o.worldNormal = worldNormal; 
                o.vertex = v.vertex;
                o.color = v.color;
				return o;
			}

			// Implementation of the fragment shader
			fixed4 frag(vertOut v) : SV_Target
			{
				float3 interpNormal = normalize(v.worldNormal);

				// Calculate ambient RGB intensities
				float Ka = 1;
				float3 amb = v.color.rgb * UNITY_LIGHTMODEL_AMBIENT.rgb * Ka;

				// Calculate diffuse RBG reflections, we save the results of L.N because we will use it again
				// (when calculating the reflected ray in our specular component)
				float fAtt = 0.5;
				float Kd = 0.5;
				float3 L = normalize(_PointLightPosition - v.worldVertex.xyz);
				float LdotN = dot(L, interpNormal);
				float3 dif = fAtt * _PointLightColor.rgb * Kd * v.color.rgb * saturate(LdotN);

				// Calculate specular reflections
				float Ks = 1;
				float specN = 5; // Values>>1 give tighter highlights
				float3 V = normalize(_WorldSpaceCameraPos - v.worldVertex.xyz);
				// Using classic reflection calculation:
				//float3 R = normalize((2.0 * LdotN * interpNormal) - L);
				//float3 spe = fAtt * _PointLightColor.rgb * Ks * pow(saturate(dot(V, R)), specN);
				// Using Blinn-Phong approximation:
				specN = 0.5; // We usually need a higher specular power when using Blinn-Phong
				float3 H = normalize(V + L);
				float3 spe = fAtt * _PointLightColor.rgb * Ks * pow(saturate(dot(interpNormal, H)), specN);

				// Combine Phong illumination model components
				float4 returnColor = float4(0.0f, 0.0f, 0.0f, 0.0f);
				returnColor.rgb = amb.rgb + dif.rgb + spe.rgb;
				returnColor.a = v.color.a;

				return returnColor;
			}
			ENDCG
		}
	}
}
