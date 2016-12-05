Shader "ON/Toon/glslToon" {
Properties {
	 	_Color ("Color", Color) = (1,1,1,.5)
		_Tile ("Tiling", Float) = 12
				
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_CrossTex1("Sketch Darkest (RGB)", 2D) = "white" {}
		_CrossTex2("Sketch (RGB)", 2D) = "white" {}
		_CrossTex3("Sketch (RGB)", 2D) = "white" {}
		_CrossTex4("Sketch (RGB)", 2D) = "white" {}
}

SubShader {
	Pass { 
                 

         GLSLPROGRAM 
 		 
         #ifdef VERTEX // here begins the vertex shader
          varying vec2 uv;

         void main() {
         	gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
 			uv = gl_MultiTexCoord0.xy;
         }
          
         #endif // here ends the definition of the vertex shader
 
 
         #ifdef FRAGMENT // here begins the fragment shader
          	varying vec2 uv;

 			uniform vec4 _Color;
 			uniform float _Tile;
 			uniform sampler2D _MainTex; 
 			uniform sampler2D _CrossTex1; 
 			uniform sampler2D _CrossTex2; 
 			uniform sampler2D _CrossTex3; 
 			uniform sampler2D _CrossTex4; 
		
        void main()
         {
         	vec2 nUV = uv*_Tile;
         	vec4 Main = texture2D(_MainTex, uv);
 			vec4 CR1 = texture2D(_CrossTex1, nUV); CR1.a = CR1.r;
			vec4 CR2 = texture2D(_CrossTex2, nUV); CR2.a = CR2.r;
			vec4 CR3 = texture2D(_CrossTex3, nUV); CR3.a = CR3.r;
			vec4 CR4 = texture2D(_CrossTex4, nUV); CR4.a = CR4.r;
			
			vec4 emit = mix(vec4(0,0,0,0),
						   mix(CR1,
						   mix(CR2,
						   mix(CR3,
						   mix(CR4, vec4(1,1,1,1), 
						   		clamp((2.*_Color.a*Main.a-0.75)*10.0, 0.0, 1.0)),
						   		clamp((2.*_Color.a*Main.a-0.6)*10.0, 0.0, 1.0)),
						   		clamp((2.*_Color.a*Main.a-0.45)*10.0, 0.0, 1.0)),
						   		clamp((2.*_Color.a*Main.a-0.3)*10.0, 0.0, 1.0)),
						   		clamp((2.*_Color.a*Main.a-0.15)*10.0, 0.0, 1.0));
						   		

			vec4 L = mix(Main*_Color,Main,emit.a); 		
            gl_FragColor = L;
     
         }
 
         #endif // here ends the definition of the fragment shader
 
         ENDGLSL // here ends the part in GLSL 
      }
   }

}


