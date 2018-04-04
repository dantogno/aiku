Shader "Kwaku/EdgeBuffer"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
	}
	
	SubShader
	{//give tags appropriate meanings. Queue transparent so that we can see its appearing on screen
//setting it to 300
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}
		Cull [_Culling]
		Lighting Off
//this is not a pass
		CGPROGRAM //the language will be using is CG

			//we will be using a surface shader on the vertices we are declearing
		#pragma surface surf Lambert vertex:vert nofog noshadow noambient nolightmap novertexlights noshadowmask nometa //keepalpha
		#pragma multi_compile _ PIXELSNAP_ON

		sampler2D _MainTex; //property definition
		fixed4 _Color; //defining the color we will use at fixed4
		float _EdgeAlphaCutoff; //we can mess with its alpha to turn it off and on

			struct Input
		{
			float2 uv_MainTex; //using input to get the points around the MainsTexture
			//fixed4 color;
		};

		//always will declear vert but this void vert  will take the input of appdate_full
		//appdata_full : position, tangent, normal, four texture coordinates and color.
		
		//input is a struct that we define that carries the structs texture with its uvs
		
		void vert(inout appdata_full v, out Input o)
		{
			#if defined(PIXELSNAP_ON)
			v.vertex = UnityPixelSnap(v.vertex); //nap() to round up the post-processed vertices so they align with screen-space coordinates.
			#endif

			UNITY_INITIALIZE_OUTPUT(Input, o); //nitializes the variable name of given type to zero.
			//o.color = v.color;
		}

//Here’s a Shader that sets the surface color to “white”. It uses the built-in Lambert (diffuse) lighting model.
		void surf(Input IN, inout SurfaceOutput o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);// * IN.color; //this is where we might want to return c to get the outline of the object
			if (c.a < _EdgeAlphaCutoff) discard;

			float alpha = c.a * 99999999;

			//general settings we use for diffuse
			o.Albedo = _Color * alpha;
			o.Alpha = alpha;
			o.Emission = o.Albedo;
		}
			
			ENDCG
		
	}
Fallback "Transparent/VertexLit"
}
