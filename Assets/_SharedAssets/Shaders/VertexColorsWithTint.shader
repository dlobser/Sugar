Shader "ON/VertexColors/VertexColorsWithTint"
{
	Properties
	{
        [KeywordEnum(NONE, COLOR, TEXTURE)] _BAKE("Bake Mode", Float) = 0

		_OcclusionMap ("Occlusion Texture", 2D) = "white" {}
        _PaperTex ("Paper Texture", 2D) = "white" {}

        // Baked into Paper Data 

		_PaperTiling ("Paper Tiling", Float) = 1 		      
		_PaperColor ("Paper Color", Color) = (1,1,1,1) 
		 
         // Baked into Separate Textures
        
        _Color ("Main Color", Color) = (1,1,1,0.5) 
		_Shadow ("Occlusion Color", Color) = (1,1,1,0.1) 

        // Baked into Vertex Color

        _VInvert ("Invert", Color) = (1,1,1,0.5)
		_VertRColorTint ("VertR Color Tint", Color) = (1,1,1,1) 
		_VertGColorTint ("VertG Color Tint", Color) = (1,1,1,1) 
		_VertBColorTint ("VertB Color Tint", Color) = (1,1,1,1) 

        // Baked into Fog Textures

		_FogColor("Fog",Color) = (1,1,1,1)
		_FogDist("X:Near,Y:Mult,Z:TexMix,W:Pow", Vector) = (0,0,.5,3)

        // Data textures

        _PaperDataTex ("Paper Data Texture", 2D) = "white" {}
        _ColorTex ("Main Color Texture", 2D) = "white" {}
        _ShadowTex ("Shadow Color Texture", 2D) = "white" {}

        // Hidden properties

        [HideInInspector] _Mode ("__mode", Float) = 0.0
        [HideInInspector] _SrcBlend ("__src", Float) = 1.0
        [HideInInspector] _DstBlend ("__dst", Float) = 0.0
        [HideInInspector] _ZWrite ("__zw", Float) = 1.0

	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }

        Blend [_SrcBlend] [_DstBlend]
        ZWrite [_ZWrite]
        Cull Off

        LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
            #pragma target 3.0
			#include "UnityCG.cginc"
            #pragma shader_feature _BAKE_NONE _BAKE_COLOR _BAKE_TEXTURE


			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				fixed4 color : COLOR; 
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				fixed4 color : COLOR; 
				float4 vertex : SV_POSITION;
			};

			sampler2D _OcclusionMap;
			sampler2D _PaperTex;
			float4 _PaperTex_ST;
			float4 _OcclusionMap_ST;
			float4 _Color;
			float4 _VInvert;
			float4 _Shadow;
			float _PaperTiling;
			float _PaperContrast;
			float4 _VertRColorTint;
			float4 _VertGColorTint;
			float4 _VertBColorTint;
			float4 _FogDist;
			float4 _FogColor;
			float4 _PaperColor;

			sampler2D _PaperDataTex;
			sampler2D _ColorTex;
			sampler2D _ShadowTex;

		
			v2f vert (appdata v)
			{           

				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);

				o.uv = TRANSFORM_TEX(v.uv, _OcclusionMap);

				fixed4 vColor = v.color;

				fixed4 RColor = vColor;
				fixed4 GColor = vColor;
				fixed4 BColor = vColor;
				fixed4 vertColors = vColor;

				#ifdef _BAKE_NONE
					RColor = lerp(fixed4(1,1,1,1),_VertRColorTint, lerp(vColor.r,_VInvert.r-vColor.r,_VInvert.r));
					GColor = lerp(fixed4(1,1,1,1),_VertGColorTint, lerp(vColor.g,_VInvert.g-vColor.g,_VInvert.g));
					BColor = lerp(fixed4(1,1,1,1),_VertBColorTint, lerp(vColor.b,_VInvert.b-vColor.b,_VInvert.b));
					vertColors = RColor*GColor*BColor*_Color.a*2;
				#endif
					o.color = vertColors;
//				fixed4 vertColors = RColor*GColor*BColor;
//				o.color = fixed4(1,1,0,1);//vertColors;

				float d = length(mul (UNITY_MATRIX_MV,v.vertex));
				o.color.a = d;

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{

				
				float  _paperTiling;
				float4 _paperColor;
				float4 _color;
				float4 _shadow;

				#ifdef _BAKE_NONE

				_paperTiling = _PaperTiling;
				_color = _Color;
				_shadow = _Shadow;
				_paperColor = _PaperColor;

				#elif _BAKE_COLOR

				_paperTiling = _PaperTiling;
				_color = _Color;
				_shadow = _Shadow;
				_paperColor = _PaperColor;

			    #elif _BAKE_TEXTURE

			    half4 paperDataTex = tex2D(_PaperDataTex,i.uv);
			    fixed4 ColorTex = tex2D(_ColorTex,i.uv);
			    fixed4 ShadowTex = tex2D(_ShadowTex,i.uv);

				_paperTiling = paperDataTex.a*100;
				_color = ColorTex;
				_shadow = ShadowTex;
				_paperColor = paperDataTex;

				#endif

				fixed4 paperTex = tex2D(_PaperTex, i.uv * _paperTiling);
				fixed4 occlusionMap = tex2D(_OcclusionMap,i.uv);
//                float j;

				fixed4 paperCol = lerp(_paperColor,fixed4(1,1,1,1),paperTex);

				fixed4 occlusion = lerp(_shadow,_color,
					lerp(occlusionMap.r,fixed4(1,1,1,1),
					(1-_shadow.a*2*tex2D(_PaperTex, float2( i.uv.y,i.uv.x) * _paperTiling))));

				fixed4 paper = paperCol;//*_color.a*2.;
				fixed4 col = occlusion * paper * i.color;

				fixed4 fogMix = lerp(
					col,
					_FogColor * lerp(float4(1.0,1.0,1.0,1.0),paper,_FogDist.z),
					pow(min(1.0,max(0.0,i.color.a-_FogDist.x)*_FogDist.y),_FogDist.w));

				fixed4 finalMix = lerp(col,fogMix,_FogColor.a);

				finalMix.a = occlusionMap.a*_PaperColor.a;

//                #ifdef _BAKE_NONE
//                    finalMix = float4(1, 0, 0, 1);
//                #elif _BAKE_COLOR
//                    finalMix = float4(0, 1, 0, 1);
//                #elif _BAKE_TEXTURE
//                    finalMix = float4(0, 0, 1, 1);
//                #endif

				return finalMix;
			}
			ENDCG
		}
	}

    CustomEditor "VertexColorsWithTintGUI"
}
