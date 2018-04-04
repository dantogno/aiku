Shader "Kwaku/EdgeShader"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
	//	_OutlineWidth("Outline width", Range(1.0,5.0)) = 1.01
	}
	SubShader
	{
		Pass
		{
			Tags{ "RenderType" = "Opaque" } //will be using the render
			LOD 200
			ZTest Always
			ZWrite Off
			Cull Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0 //- which shader target to compile to. 
			
			#include "UnityCG.cginc"
			
			sampler2D _MainTex;
			float4 _MainTex_ST;

			//float _OutlineWidth;

			sampler2D _OutlineSource;

			struct v2f
			{
				float4 position : SV_POSITION; //finding the position and calling its function
				float2 uv : TEXCOORD0; //finding the textures cordinates again
			};

			v2f vert (appdata_img v) // using the appdata_img v makes the effect only work on the image we are using
			{
				//v.vertex.xyz *=  _OutlineWidth;
				v2f o;
				o.position = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;

				return o;
			}
			float _LineThicknessX;
			float _LineThicknessY;
			
			uniform float4 _MainTex_TexelSize;

			half4 frag(v2f input) : COLOR //half4 can return the color of the object so it will be needed
			{
				float2 uv = input.uv;
				#if UNITY_UV_STARTS_AT_TOP
				if (_MainTex_TexelSize.y < 0)
					uv.y = 1 - uv.y;
				#endif

				half4 outlineSource = tex2D(_OutlineSource, UnityStereoScreenSpaceUVAdjust(uv, _MainTex_ST));
				//there is no point declearing the stipid thickness of line
				const float h = .95f;
			///thickness
				half4 sample1 = tex2D(_OutlineSource, uv + float2(_LineThicknessX,0.0));
				half4 sample2 = tex2D(_OutlineSource, uv + float2(-_LineThicknessX,0.0));
				half4 sample3 = tex2D(_OutlineSource, uv + float2(.0,_LineThicknessY));
				half4 sample4 = tex2D(_OutlineSource, uv + float2(.0,-_LineThicknessY));
			
	bool red = sample1.r > h || sample2.r > h || sample3.r > h || sample4.r > h;
			///thickness
				bool green = sample1.g > h || sample2.g > h || sample3.g > h || sample4.g > h;
				bool blue = sample1.b > h || sample2.b > h || sample3.b > h || sample4.b > h;
				 
				if ((red && blue) || (green && blue) || (red && green))
					return float4(0,0,0,0);
				else
				return outlineSource; //we return the color to itself. whichever color we decide to use

			}
			
			ENDCG
		}

	Pass //is this the revert?
		{
			Tags { "RenderType"="Opaque" }
			LOD 200
			ZTest Always
			ZWrite Off
			Cull Off
			
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _OutlineSource;

			struct v2f {
			   float4 position : SV_POSITION;
			   float2 uv : TEXCOORD0;
			};
			
			v2f vert(appdata_img v)
			{
			   	v2f o;
				o.position = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;
				
			   	return o;
			}
			float _LineThicknessX;
			float _LineThicknessY;
			half4 _LineColor1;
			float _LineIntensity;
			int _Dark;
			float _FillAmount;
			int _CornerOutlines;
			uniform float4 _MainTex_TexelSize;

			half4 frag (v2f input) : COLOR
			{	
								
				float2 uv = input.uv; //getting the inputs uv. now we have the choice to revert to the orignal

					#if UNITY_UV_STARTS_AT_TOP
						if (_MainTex_TexelSize.y < 0)
							uv.y = 1 - uv.y;
					#endif

				half4 originalPixel = tex2D(_MainTex, UnityStereoScreenSpaceUVAdjust(input.uv, _MainTex_ST));
				half4 outlineSource = tex2D(_OutlineSource, UnityStereoScreenSpaceUVAdjust(uv, _MainTex_ST));
								
				const float h = .95f;
				half4 outline = 0;
				bool hasOutline = false;
			///thickness
				half4 sample1 = tex2D(_OutlineSource, uv + float2(_LineThicknessX,0.0));
				half4 sample2 = tex2D(_OutlineSource, uv + float2(-_LineThicknessX,0.0));
				half4 sample3 = tex2D(_OutlineSource, uv + float2(.0,_LineThicknessY));
				half4 sample4 = tex2D(_OutlineSource, uv + float2(.0,-_LineThicknessY));
				bool red = sample1.r > h || sample2.r > h || sample3.r > h || sample4.r > h;


				bool outside = outlineSource.a < h;
				bool outsideDark = outside && _Dark;
				if (_CornerOutlines) // making it more smoother
				{
					// TODO: Conditional compile
					half4 sample5 = tex2D(_OutlineSource, uv + float2(_LineThicknessX, _LineThicknessY));
					half4 sample6 = tex2D(_OutlineSource, uv + float2(-_LineThicknessX, -_LineThicknessY));
					half4 sample7 = tex2D(_OutlineSource, uv + float2(_LineThicknessX, -_LineThicknessY));
					half4 sample8 = tex2D(_OutlineSource, uv + float2(-_LineThicknessX, _LineThicknessY));

					if (sample1.r > h || sample2.r > h || sample3.r > h || sample4.r > h ||
						sample5.r > h || sample6.r > h || sample7.r > h || sample8.r > h)
					{
						outline = _LineColor1 * _LineIntensity * _LineColor1.a;
						if (outsideDark)
							originalPixel *= 1 - _LineColor1.a;
						hasOutline = true;
					}

					if (!outside)
						outline *= _FillAmount;
				}
				else // EVERYTHING HAPPENS HERE
				{
					if (sample1.r > h || sample2.r > h || sample3.r > h || sample4.r > h)
					{
						outline = _LineColor1 * _LineIntensity * _LineColor1.a;
						if (outsideDark)
							originalPixel *= 1 - _LineColor1.a;
						hasOutline = true;
					}

					if (!outside)
						outline *= _FillAmount;
				}					
					
				//return outlineSource;		
				if (hasOutline)
					return lerp(originalPixel + outline, outline, _FillAmount);
				else
					return originalPixel;
			}
			
			ENDCG
		}
	}
FallBack "Diffuse"
}
