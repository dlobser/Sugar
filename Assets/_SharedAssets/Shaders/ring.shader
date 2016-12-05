Shader "ON/Effect/ring"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color("Color",Color) = (1,1,1,1)
		_Size("Size",Float) = 1
		_Radius("Radius",Float) = 1
		_Pow("Power",Float) = 1
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
		LOD 100
		Blend One One
		ZWrite Off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _Size;
			float _Radius;
			float _Pow;
			float4 _Color;

			float4 f4(float f){
				return float4(f,f,f,f);
			}
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				float dist = distance(i.uv,float2(.5,.5))*_Size;
				float cDist = abs((cos(max(0.0,min(1.0,dist+_Radius))*3.28)));
				float mDist = pow(-cDist+1., _Pow);
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return f4(mDist)*col*_Color;
			}
			ENDCG
		}
	}
}
