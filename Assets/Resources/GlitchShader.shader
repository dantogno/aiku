Shader "Glitch/Effect"
{
    Properties
    {
        _MainTex ("-", 2D) = "" {}
    }
    CGINCLUDE

    #include "UnityCG.cginc"

    sampler2D _MainTex;
    float2 _MainTex_TexelSize;

    float2 _ScanLineJitter; // (displacement, threshold)
    float2 _ColorDrift;     // (amount, time)
	float _GlitchInterval;
	float _DispIntensity;
	float _DispProbability;
	float _ColorIntensity;
	float _ColorProbability;
	float _DispGlitchOn;
	float _ColorGlitchOn;
	float _WrapDispCoords;

  float rand(float x, float y){return frac(sin(x*12.9898 + y*78.233)*43758.5453);}

    half4 frag(v2f_img i) : SV_Target
    {
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
		
        // Color drift
       // float drift = sin(jump + _ColorDrift.y) * _ColorDrift.x;

       // half4 src1 = tex2D(_MainTex, frac(float2(u + jitter + drift)));
		
		//atleast we use the RGBA of the object and flip its displacement
		half4 c = fixed4(0.0,0.0,0.0,0.0);
		
	//	half4 src1 = tex2D(_MainTex, frac(float2(u + jitter )
		fixed4 rShifted = tex2D(_MainTex, float2(i.uv.x + jitter + rShiftRandom, i.uv.y  + rShiftRandom));
		fixed4 gShifted = tex2D(_MainTex, float2(i.uv.x + jitter + gShiftRandom,  i.uv.y + gShiftRandom));
		fixed4 bShifted = tex2D(_MainTex, float2(i.uv.x + jitter + bShiftRandom,  i.uv.y  + bShiftRandom));

		c.r = rShifted.r;
		c.g = gShifted.g;
		c.b = bShifted.b;
	//	half4 src1 = tex2D(_MainTex, frac(float2(u + jitter + drift)));
		
		return c;
        //return half4(src1.r, src1.g, src1.b, 1);
    }

    ENDCG
    SubShader
    {
        Pass
        {
            ZTest Always Cull Off ZWrite Off
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #pragma target 3.0
            ENDCG
        }
    }
}
