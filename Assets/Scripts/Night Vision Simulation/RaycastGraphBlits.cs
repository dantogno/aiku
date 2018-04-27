using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastGraphBlits : MonoBehaviour
{
	/// <summary>
	/// Similar to Graph.blit(source, destination,material)
	/// this is needed to display shader image effects
	/// Place this scr on the main camera or any gameobject that has the shader fills
	///Night Vision , Glitch or Simulate
	/// </summary>
	private static Camera _camera;


	//A camera can build a screen-space depth texture. 
	//This is mostly useful for image post-processing effects
	void OnEnable()
	{
		_camera = GetComponent<Camera>();
		_camera.depthTextureMode = DepthTextureMode.Depth;
	}
	/// <summary>
	/// Given viewport coordinates, calculates the view space vectors 
	/// pointing to the four frustum corners at the specified camera depth.

	/// </summary>
	/// This can be used to efficiently calculate the world space position of a pixel
	///  in an image effect shader.
	/// <param name="dest"></param>
	/// <param name="mat"></param>
	/// 
	public static void RaycastCornerBlit(RenderTexture source, RenderTexture dest, Material mat)
	{
		// Compute Frustum Corners to calculate 4 corners at a specific caemra depth
		float camFar = _camera.farClipPlane;
		float camFov = _camera.fieldOfView;
		float camAspect = _camera.aspect;

		float fovWHalf = camFov * 0.5f;


		//projection matrices for top right left bottom 
		Vector3 toRight = _camera.transform.right * Mathf.Tan(fovWHalf * Mathf.Deg2Rad) * camAspect;
		Vector3 toTop = _camera.transform.up * Mathf.Tan(fovWHalf * Mathf.Deg2Rad);

		Vector3 topLeft = (_camera.transform.forward - toRight + toTop);
		float camScale = topLeft.magnitude * camFar;

		topLeft.Normalize();
		topLeft *= camScale;

		Vector3 topRight = (_camera.transform.forward + toRight + toTop);
		topRight.Normalize();
		topRight *= camScale;

		Vector3 bottomRight = (_camera.transform.forward + toRight - toTop);
		bottomRight.Normalize();
		bottomRight *= camScale;

		Vector3 bottomLeft = (_camera.transform.forward - toRight - toTop);
		bottomLeft.Normalize();
		bottomLeft *= camScale;

		// Custom Blit, encoding Frustum Corners as additional Texture Coordinates
		RenderTexture.active = dest;
		//This is where we set our mainTex which will be the shader we are currently using
		mat.SetTexture("_MainTex", source);

		GL.PushMatrix();
		GL.LoadOrtho();

		// activate the first shader pass (in this case we know it is the only pass)
		mat.SetPass(0);
		https://docs.unity3d.com/ScriptReference/Material.SetPass.html
		// draw a quad over whole screen
		GL.Begin(GL.QUADS);

		GL.MultiTexCoord2(0, 0.0f, 0.0f);
		GL.MultiTexCoord(1, bottomLeft);
		GL.Vertex3(0.0f, 0.0f, 0.0f);

		GL.MultiTexCoord2(0, 1.0f, 0.0f);
		GL.MultiTexCoord(1, bottomRight);
		GL.Vertex3(1.0f, 0.0f, 0.0f);

		GL.MultiTexCoord2(0, 1.0f, 1.0f);
		GL.MultiTexCoord(1, topRight);
		GL.Vertex3(1.0f, 1.0f, 0.0f);

		GL.MultiTexCoord2(0, 0.0f, 1.0f);
		GL.MultiTexCoord(1, topLeft);
		GL.Vertex3(0.0f, 1.0f, 0.0f);

		GL.End();

		//restores both projection and modelview matrices off the top of the matrix stack.
		GL.PopMatrix();
	}
}
