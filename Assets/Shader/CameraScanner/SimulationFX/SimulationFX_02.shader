Shader "Kwaku/SimulationFX"
{
    Properties
    {
        _MainTex ("-", 2D) = "" {} //Properties that will display in inspector
     	_DetailTex("Texture", 2D) = "white" {} //Second Texture we will display on top of screen if needed


    }
	
    CGINCLUDE

    #include "UnityCG.cginc"
		
			sampler2D _MainTex;						 //MainTex decleared in Properties has to be declared as a sample2D texture
			float4 _MainTex_ST;						//MainTex's ST float represents the textures scale

			
			struct appdata
			{
					float4 vertex : POSITION; 	//position of vertex
					float2 uv : TEXCOORD0;		 //the textures cordinates
					float4 ray : TEXCOORD1;		// textures cordinates from screen to world/ cam to world.
		
			};	
			struct v2f
			{
				float4 vertex : SV_POSITION; //finding the position and calling its function
				float2 uv : TEXCOORD0;
				float2 uv_depth : TEXCOORD1; // we need a second texture wordinate 
				float4 interpolatedRay : TEXCOORD2; //we use this to tell what the vecters ray hit position is
	
			};

		float4 _MainTex_TexelSize;//finding the size and making sure the screen is not flipped upside down
			
	
			
			v2f vert (appdata v)
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
			
			sampler2D _DetailTex;
			sampler2D_float _CameraDepthTexture;
			float _ScanDistance;
			float _ScanWidth;
			float4 _WorldSpaceScannerPos;
			float4 _MidColor;
			float4 _TrailColor;
			half _Intensity;

		  float rand(float x, float y){return frac(sin(x*12.9898 + y*78.233)*43758.5453);}
float4 horizTex(float2 p)
			{
				return tex2D(_DetailTex,p);
			}
	   half4 frag(v2f i) : SV_Target
		{

			half4 col = tex2D(_MainTex, i.uv);
				float rawDepth = DecodeFloatRG(tex2D(_CameraDepthTexture, i.uv_depth)); //sample depgth value
				float linearDepth = Linear01Depth(rawDepth); //gives a value for depth
				float3 wsDir = linearDepth * i.interpolatedRay; //multiple depth to get world space 	//world space pointing from the camera		
				float3 wsPos = _WorldSpaceCameraPos + wsDir; 	//adding world space value in every pixel
				half4 scannerCol = half4(0, 0, 0, 0);
				half  color = (0); //first colired lines going in from x
				half4 color2 = (0,0,0,0); //second colored lines going from y
				half4 color3 = (0,0,0,0); //third colored line from the zDir
				half4 zColored = (0,0,0,0); //third color to complete the 4 fixed colors
				float dist = distance(wsPos, _WorldSpaceScannerPos); // we find the distance of worldspace and the scanning space origin
				


					float csin = abs(cos(wsPos.x * UNITY_PI * 4)); //using trig to calculate the lines draw on the screen
					float ssin = abs(cos(wsPos.y * UNITY_PI * 4));
					float zDir = abs(sin(wsPos.z * UNITY_PI * 4));

					color2 = (pow(csin,30)); 
					color3 = (pow(ssin,30));

					zColored = (pow(zDir,30));
					half4 mixedLerp = lerp(color2,color3,zColored);  //getting all the colors to lerp and show on the screen
					
		////after getting the distance we figure out how far our distance is from the worldspace position, if it is too far we do not show anything,
		//the closer the better. We set its linear depth to less than one.
		//Diff finds the difference between the scanning distance from world space and divides by the with to make the with smalling or larger.
	
				if (dist < _ScanDistance && dist > _ScanDistance - _ScanWidth && linearDepth < 1)
				{
					float diff = 1 - (_ScanDistance - dist) / (_ScanWidth);
					half4 edge = lerp(_MidColor * color2 * _Intensity, _MidColor * _Intensity *  color3 + horizTex(i.uv),  pow(diff, _Intensity));
					scannerCol = lerp(_MidColor * zColored * _Intensity, edge, diff);
				//Scanncolor has the mid/trail color that is timed by its intensity(float #) which is available in the NV.cs file
				//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

					color = lerp(mixedLerp,_MidColor,_Intensity);
					scannerCol *=diff * _MidColor;
					
				}
			//color is then timesed by the world screen space var when returning.
              return col + scannerCol * color;
		
		}

    ENDCG
    SubShader
    {
//no tags needed
        Pass
        {
			//How should depth testing be performed.
			// Default is LEqual 
			//(draw objects in from or at the distance as existing objects; hide objects behind them).
            ZTest Always
				Cull Off 
			//you’re drawng solid objects, leave this on. However this object will be Transparent
			ZWrite Off
			
            CGPROGRAM
			// compilation directives for the image projected
            #pragma vertex vert
            #pragma fragment frag
			//DX9 shader model 3.0: derivative instructions, texture LOD sampling, 10 interpolators, more math/texture instructions allowed.
            #pragma target 3.5
            ENDCG
        }
	
    }
	 Fallback "Diffuse"
}
	
