Shader "ON/Effect/transparentTintWarp"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_WarpTex ("Warp", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
		_Warp("warpAmount",Float) = .3
		_Speed("Speed",Float) = 0
	}
	SubShader
	{
		Tags { "Queue" = "Transparent" "RenderType"="Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		Cull off
//		ZTest Always
		LOD 100

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
				float2 uv2 : TEXCOORD1;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
				UNITY_FOG_COORDS(2)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _WarpTex;
			float4 _WarpTex_ST;
			float4 _Color;
			float _Warp;
			float _Speed;
		
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv2 = TRANSFORM_TEX(v.uv2, _WarpTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 warp = (tex2D(_WarpTex, i.uv2 + _Time.y*_Speed));
				warp += (tex2D(_WarpTex, i.uv2 - _Time.y*_Speed));
				fixed4 col = (tex2D(_MainTex, float2(_Warp,_Warp)-float2(i.uv.x+warp.r*_Warp,i.uv.y+warp.b*_Warp)))*_Color;
//				col.a = (tex2D(_MainTex, i.uv)).a;
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
