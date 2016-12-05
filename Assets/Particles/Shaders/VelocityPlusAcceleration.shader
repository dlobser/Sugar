Shader "Unlit/VelocityToPosition"
{
	Properties
	{
		_MainTex ("Acceleration", 2D) = "white" {}
		_OldVelocity ("Old Velocity",2D) = "black"{}
		_Mass ("Mass",Float) = 1
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
			sampler2D _OldVelocity;
			float _Mass;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 acc = tex2D(_MainTex, i.uv);
				fixed4 prevVel = tex2D(_OldVelocity, i.uv);
				fixed3 vel = (prevVel.xyz/_Mass)+acc.xyz;
				return float4(vel.x,vel.y,vel.z,1.0);
			}
			ENDCG
		}
	}
}
