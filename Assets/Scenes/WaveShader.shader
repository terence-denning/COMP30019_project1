//UNITY_SHADER_NO_UPGRADE

Shader "Unlit/WaveShader"
{
    
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		WaveSpeed ("WaveSpeed", Float) = 2.0
		WaveReduct("WaveReduct",Float) = 2.0
		_PointLightColor("Point Light Color", Color) = (0, 0, 0)
		_PointLightPosition("Point Light Position", Vector) = (0.0, 0.0, 0.0)
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
			
            uniform float3 _PointLightColor;
			uniform float3 _PointLightPosition;
			uniform sampler2D _MainTex;	
			float WaveSpeed;
			float WaveReduct;
            
			struct vertIn
			{
				float4 vertex : POSITION;
				//float2 uv : TEXCOORD2;
				float4 normal : NORMAL;
				float4 color : COLOR;
				
			};

			struct vertOut
			{
				float4 vertex : SV_POSITION;
				//float2 uv : TEXCOORD2;
				float4 color : COLOR;
				float4 worldVertex : TEXCOORD0;
				float3 worldNormal : TEXCOORD1;
			};

			// Implementation of the vertex shader
			vertOut vert(vertIn v)
			{
			    vertOut o;
				// Displace the original vertex in model space
				float4 displacement = float4(0.0f,sin(_Time.y*(WaveSpeed)+v.vertex.x+v.vertex.z)/WaveReduct, 0.0f, 0.0f);
				//float2 uvdisplacement = float2(cos(_Time.y + v.uv.x)*0.1, sin(_Time.y+v.uv.y)*0.1);
				float4 worldVertex = mul(unity_ObjectToWorld, v.vertex);
				float3 worldNormal = normalize(mul(transpose((float3x3)unity_WorldToObject), v.normal.xyz));
			
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.vertex += displacement;
				//v.uv += uvdisplacement;
				//o.uv = v.uv;   
				o.color = v.color;
				o.worldVertex = worldVertex;
				o.worldNormal = worldNormal; 
				return o;
			}
			
			// Implementation of the fragment shader
			fixed4 frag(vertOut v) : SV_Target
			{
				float3 interpNormal = normalize(v.worldNormal);

				// Calculate ambient RGB intensities
				float Ka = 1.5;
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
				specN = 30; // We usually need a higher specular power when using Blinn-Phong
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