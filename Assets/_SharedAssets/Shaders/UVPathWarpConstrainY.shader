Shader "ON/Effect/UVPathWarpConstrainY"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_UVTex("UV",2D) = "white"{}
		_Offset("offset",Vector) = (1,1,1,1)
		_Trans("transparency",Float) = 1
		_YPos("Constrain Y",Float) = 1
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent"}
		Cull Back
		Blend One One
		ZWrite Off
//		Blend SrcAlpha OneMinusSrcAlpha
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			//#pragma multi_compile_fog
			
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
				//UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			sampler2D _UVTex;
			float4 _MainTex_ST;
			float4 _UVTex_ST;
			float4 _Offset;
			float _YPos;
			float _Trans;

			v2f vert (appdata v)
			{
				v2f o;

				o.vertex = mul(UNITY_MATRIX_MVP, float4(v.vertex.x,_YPos,v.vertex.z,v.vertex.w));
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv2 = TRANSFORM_TEX(v.uv, _UVTex);
				//UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_UVTex, i.uv2);
				fixed4 warped = tex2D(_MainTex, col.xy * _Offset.zw + (float2(_Offset.x*_Time.x,_Offset.y*_Time.y)));
				warped.a *= col.a *= _Trans;
				warped*=col.a *= _Trans;
				warped*=warped.a * 2.;
				// apply fog
				//UNITY_APPLY_FOG(i.fogCoord, warped);
				return warped;
			}
			ENDCG
		}
	}
}
