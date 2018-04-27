Shader "Kwaku/NightVisionWGlitch"
{
    Properties
    {
        _MainTex ("-", 2D) = "" {}					//Properties that will display in inspector
		_DetailTex("Texture", 2D) = "white" {}		//Second Texture we will display on top of screen if needed

    }
	
    CGINCLUDE

    #include "UnityCG.cginc"
			 half _MidIntensity;					//Intensity of color multiplied
			 half _TrailIntensity;					//Intensity of trail Color
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
			sampler2D _MainTex;						 //MainTex decleared in Properties has to be declared as a sample2D texture
			float4 _MainTex_ST;						//MainTex's ST float represents the textures scale

			struct appdata							//We need appdata's struct to initial the v2fs postion and cords in world space
			{
					float4 vertex : POSITION;		//position of vertex
					float2 uv : TEXCOORD0;			 //the textures cordinates
					float4 ray : TEXCOORD1;			// textures cordinates from screen to world/ cam to world.
			};	
			struct v2f
			{
				float4 vertex : SV_POSITION;		//finding the position and calling its function
				float2 uv : TEXCOORD0; 
				float2 uv_depth : TEXCOORD1;		// we need a second textures screen to worldspace 
				float4 interpolatedRay : TEXCOORD2; //we use this to tell what the vector like an rayHitCast
	
			};

			float4 _MainTex_TexelSize; //finding the size and making sure the screen is not flipped upside down
			
			v2f vert (appdata v)		// We pass in the struct to make sure that v2f o is defined by the variables 
										//inside the v2f struct. //its like saying float rand(int i); float would be the v2f. 
										//v2f will intake our image to manipulate the effects
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex); //always clip the position to the world
					o.uv = v.uv.xy;		//geting its position in space
					o.uv_depth = v.uv.xy; // settings its position in world through camera

					#if UNITY_UV_STARTS_AT_TOP //making sure the uvs start up top and not flipped
					if (_MainTex_TexelSize.y < 0)
						o.uv.y = 1 - o.uv.y;
					#endif				

					o.interpolatedRay = v.ray; //finally setting the raycast to interpolated to define it
					return o;
				}
			
			sampler2D_float _CameraDepthTexture;
			float _ScanDistance;
			float _ScanWidth;
			float4 _WorldSpaceScannerPos;
			float4 _LeadColor;
			float4 _MidColor;
			float4 _TrailColor;

			float2 _ScanLineJitter;
			float2 _ColorDrift;   
			float _GlitchInterval;
			float _DispIntensity;
			float _DispProbability;
			float _ColorIntensity;
			float _ColorProbability;
			float _DispGlitchOn;
			float _ColorGlitchOn;
			float _WrapDispCoords;
			float rand(float x, float y){return frac(sin(x*12.9898 + y*78.233)*43758.5453);}

	   half4 frag(v2f i) : SV_Target
		{
		//this function will return the type half4 which I return a color and texture

			half4 col = tex2D(_MainTex, i.uv); //always make sure to have a half4 to set the texture to
			float rawDepth = DecodeFloatRG(tex2D(_CameraDepthTexture, i.uv_depth)); //sample depth value
			float linearDepth = Linear01Depth(rawDepth); //gives a value for depth
			float3 wsDir = linearDepth * i.interpolatedRay; //multiple depth to get world space 	//world space pointing from the camera		
			float3 wsPos = _WorldSpaceCameraPos + wsDir; 	//adding world space value in every pixel
			half4 scannerCol = half4(0, 0, 0, 0); //Empty half4 to add in later
			float dist = distance(wsPos, _WorldSpaceScannerPos); // we find the distance of worldspace and the scanning space origin
					
		//after getting the distance we figure out how far our distance is from the worldspace position, if it is too far we do not show anything,
		//the closer the better. We set its linear depth to less than one.
		//Diff finds the difference between the scanning distance from world space and divides by the with to make the with smalling or larger.
		if (dist < _ScanDistance && dist > _ScanDistance - _ScanWidth && linearDepth < 1)
			{
			float diff = 1 - (_ScanDistance - dist) / (_ScanWidth);
			half4 edge = lerp(_LeadColor, _MidColor, _MidIntensity);
			scannerCol = lerp(_TrailColor * _TrailIntensity, edge, diff);
		//Scanncolor has the mid/trail color that is timed by its intensity(float #) which is available in the NV.cs file
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

			//making sure to use the right postion
			float u = i.uv.x;
			float v = i.uv.y;
			//making sure the shaders glitch at a different Time Span
			float intervalTime = floor(_Time.y / _GlitchInterval) * _GlitchInterval;
			//Second interval for multiple glitch
			float intervalTime2 = intervalTime + 2.793;
		
			//different x and ys to transform on the maxtrix to cause them not to glitch at the same time
			float timePosVal = intervalTime + unity_ObjectToWorld[0][3] + unity_ObjectToWorld[1][3];
			float timePosVal2 = intervalTime2 + unity_ObjectToWorld[0][3] + unity_ObjectToWorld[1][3];
			//Random color glitch displacement
			float dispGlitchRandom = rand(timePosVal, -timePosVal);
			float colorGlitchRandom = rand(timePosVal, timePosVal);

			//get a random color intensity to magnify the effect
			float rShiftRandom = (rand(-timePosVal, timePosVal) - 0.5) * _ColorIntensity;
			float gShiftRandom = (rand(-timePosVal, -timePosVal) - 0.5) * _ColorIntensity;
			float bShiftRandom = (rand(-timePosVal2, -timePosVal2) - 0.5) * _ColorIntensity;

			//This value is the random offset each of the strip boundries get either up or down
			//Without this, each strip would be exactly a 5th of the sprite height, with this their height is slightly randomised
			float shiftLineOffset = float((rand(timePosVal2, timePosVal2) - 0.5) / 50);

			i.uv.x += (rand(floor(i.uv.y / (0.2 + shiftLineOffset)) - timePosVal, floor(i.uv.y / (0.2 + shiftLineOffset)) + timePosVal) - 0.5) * _DispIntensity;
       
			 // Scan line jitter
			float jitter = rand(v, _Time.x) * 2 - 1;
			jitter *= step(_ScanLineJitter.y, abs(jitter)) * _ScanLineJitter.x;
		
       
			//atleast we use the RGBA of the object and flip its displacement
			half4 c = fixed4(0.0,0.0,0.0,0.0);
		
			//We find the value of the uv and add how jittery we want the effect to be.
			fixed4 rShifted = tex2D(_MainTex, float2(i.uv.x + jitter + rShiftRandom, i.uv.y  + rShiftRandom));
			fixed4 gShifted = tex2D(_MainTex, float2(i.uv.x + jitter + gShiftRandom,  i.uv.y + gShiftRandom));
			fixed4 bShifted = tex2D(_MainTex, float2(i.uv.x + jitter + bShiftRandom,  i.uv.y  + bShiftRandom));

			//since half c is defined as a color, it is fixed at the rgb. 
			c.r = rShifted.r;
			c.g = gShifted.g;
			c.b = bShifted.b;
			//the easiest way to apply the glitch effect is to just add it to the diffence from the world space
			//to the screen space "+c" . 
			scannerCol *=diff + c;
					
			}
			//Since scannerCol is defined now, we return the Scanncolor + the col that has the world uv vertices and world space in it.
          return col + scannerCol; //this gives us the result we are looking for

		}

    ENDCG
    SubShader
    {
		
        Pass //when we pass a subshader, it can be either with its functions inside or its functions outside
			//this works as long as the functions it has is called in its CGPROGRAM - ENDCG
        {
			//Blending the image so it shows on the different aplha
		Blend One One
			//	Cull Off
			Tags
			{///making it see through
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			}
			//How should depth testing be performed.
			// Default is LEqual 
			//(draw objects in from or at the distance as existing objects; hide objects behind them).
            ZTest Always 
			//Controls which sides of polygons should be culled (not drawn)
			Cull Off 
			//you’re drawng solid objects, leave this on. However this object will be Transparent
			ZWrite Off
			
            CGPROGRAM
			// compilation directives for the image projected
            #pragma vertex vert
            #pragma fragment frag
			//DX9 shader model 3.0: derivative instructions, texture LOD sampling, 10 interpolators, more math/texture instructions allowed.
            #pragma target 3.0
            ENDCG
        }
    }
	 Fallback "Diffuse"
}

	
