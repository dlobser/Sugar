Shader "Unlit/VelocityToPosition"
{
	Properties
	{
		_MainTex ("Acceleration", 2D) = "white" {}
		_Position ("Position",2D) = "black"{}
		_Speed ("_Speed",Float) = 1
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

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _Position;
			float _Speed;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 acc = tex2D(_MainTex, i.uv);
				fixed4 pos = tex2D(_Position, i.uv);
				fixed3 vel = (pos.xyz)+acc.xyz*_Speed;
				// apply fog
				return float4(vel.x,vel.y,vel.z,1.0);
			}
			ENDCG
		}
	}
}
