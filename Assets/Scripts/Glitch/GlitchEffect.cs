using UnityEngine;
using System.Collections;

/// <summary>
/// 
/// </summary>
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Distortion Glitch")]
public class GlitchEffect : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Glitch interval time (seconds)")]
    private float disableAfterSeconds = 0;

	[SerializeField, Range(0, 1)]
    [Tooltip("")]
	private float _GlitchInterval = .16f;

	[SerializeField, Range(0, 1)]
    [Tooltip("")]
    private float _DispProbability = 0.022f;

	[SerializeField, Range(0, 1)]
    [Tooltip("")]
    public float _DispIntensity = 0.09f;    // DW made public

	[SerializeField, Range(0, 1)]
    [Tooltip("")]
    private float _ColorProbability = 0.02f;

	[SerializeField, Range(0, 1)]
    [Tooltip("")]
    public float _ColorIntensity = 0.07f;   // DW made public

	//[SerializeField, Range(0, 1)]
	//	float _scanLineJitter = 0;

	private Texture2D tex;
	private Texture t;
	private Renderer t_rend;
	private Material m_tex;

	
	[SerializeField]
    [Tooltip("")]
    private Shader _shader;
	private Material _material;


    private void Start()
    {
        if (disableAfterSeconds > 0)
            Destroy(this, disableAfterSeconds);
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (_material == null)
		{
			_material = new Material(_shader);

		}

		_material.SetFloat("_GlitchInterval", _GlitchInterval);
		_material.SetFloat("_DispIntensity", _DispIntensity);
		_material.SetFloat("_DispProbability", _DispProbability);
		_material.SetFloat("_ColorIntensity", _ColorIntensity);
		_material.SetFloat("_ColorProbability", _GlitchInterval);

		Graphics.Blit(source, destination, _material);
		
	}
}

