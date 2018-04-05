using UnityEngine;
using System.Collections;

//[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Distortion Glitch")]
public class GlitchEffect : MonoBehaviour
	{
    [SerializeField]
    private float disableAfterSeconds = 0;
	//"Glitch interval time [seconds]"
	[SerializeField, Range(0, 1)]
	float _GlitchInterval = .16f;
	[SerializeField, Range(0, 1)]
	float _DispProbability = 0.022f;
	[SerializeField, Range(0, 1)]
	public float _DispIntensity = 0.09f;    // DW made public
	[SerializeField, Range(0, 1)]
	float _ColorProbability = 0.02f;
	[SerializeField, Range(0, 1)]
	public float _ColorIntensity = 0.07f;   // DW made public
	//[SerializeField, Range(0, 1)]
	//	float _scanLineJitter = 0;
	Texture2D tex;
	Texture t;
	Renderer t_rend;
	Material m_tex;

	
	[SerializeField] Shader _shader;
		Material _material;


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

		;

		Graphics.Blit(source, destination, _material);
		
		}
	}

