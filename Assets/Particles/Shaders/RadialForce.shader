 Shader "Unlit/RadialForce"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Speed("speed",float)=1
		_Pos("pos",Vector)=(0,0,0,0)
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
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
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			sampler2D _Feedback;
			float4 _MainTex_ST;
			float _Speed;
			float4 _Pos;
			float _Freq;
			float4 _Speeds;
			float _Which;
			float _SinAdd;
			float _Gravity;
			
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
				fixed4 col2 = tex2D(_MainTex, i.uv);
				float d = max(distance(col2.xyz,_Pos.xyz),.4);
				fixed3 delt = normalize(_Pos.xyz-col2.xyz)*_Speed;
				float3 gravity = (delt/(d*d));
				return float4(gravity.x,gravity.y,gravity.z,1.0);
			}
			ENDCG
		}
	}
}
