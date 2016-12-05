Shader "Custom/ParticleBillboard" {
	Properties {
		_Offset ("offset", 2D) = "white" {}
		_Color ("color",Color) = (1,1,1,1)
		_SpriteTex("sprite", 2D) = "white" {}
		_LineWidth ("lineWidth", Float) = 12
	}

	SubShader {
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		LOD 100
		
		ZWrite Off
		Blend One One//SrcAlpha OneMinusSrcAlpha 
		Cull Off
		
		Pass {  
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile_fog
				#pragma glsl
				#pragma target 3.0
				#include "UnityCG.cginc"

				struct appdata_t {
					float4 vertex : POSITION;
					float4 color : COLOR;
					float2 texcoord : TEXCOORD0;
					float2 texcoord2 : TEXCOORD1;
				};

				struct v2f {
					float4 vertex : SV_POSITION;
					float4 color : COLOR;
					half2 texcoord : TEXCOORD0;
					half2 texcoord2 : TEXCOORD1;
					UNITY_FOG_COORDS(1)
				};

				sampler2D _Offset;
				sampler2D _SpriteTex;
				float _LineWidth;
				float4 _Color;
				float _UNPnts;

				float3 hsv2rgb(float3 c)
				{
				    float4 K = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
				    float3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);
				    return c.z * lerp(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
				}
				float4 place(float f,float b) {
					float4 off = tex2Dlod(_Offset,float4(f,b,0,0));
		            return  off;

		         }

				v2f vert (appdata_t v)
				{
					v2f o;
					o.vertex = v.vertex;
					o.texcoord = v.texcoord;
					o.texcoord2 = v.texcoord2;
					float4 position = o.vertex;
	            	float4 p0 = mul(UNITY_MATRIX_MVP, place(position.x,position.y ));
	            	o.vertex = p0+ float4(o.texcoord2.x,o.texcoord2.y*1.5,0,0)*_LineWidth;
	            	o.color = v.color;
					return o;
				}
				
				fixed4 frag (v2f i) : SV_Target
				{
					float4 tex = tex2D(_SpriteTex,i.texcoord2);
					return _Color*i.color*tex.a;//2*tex*tex.a;
				}
			ENDCG
		}
	}

}
