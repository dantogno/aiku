using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightVision : MonoBehaviour
{
    // Night Mode is 
    public enum GlitchMode { GlitchOn, GlitchOff }
    [SerializeField] GlitchMode _mode = GlitchMode.GlitchOn;
    public GlitchMode mode { get { return _mode; } set { _mode = value; } }


    [Header("Color Intensity Settings")]
    #region Color
    [SerializeField, Range(0, 10)]
    private float _MidColorIntensity = 0.5f;
    [SerializeField, Range(0, 10)] private float _TrailColorIntensity = 0.5f;

    //Representation of RGBA colors in 32 bit format
    [Header("Color Gradiant Settings")]
    // Wave color
    [SerializeField]
    public Color32 _MidColor = new Color(255, 255, 255, 255);
    private Color midColor { get { return _MidColor; } set { _MidColor = value; } }

    //Trail Color
    [SerializeField] Color32 _TrailColor = new Color(255, 255, 255, 255);
    public Color trailColor { get { return _TrailColor; } set { _TrailColor = value; } }
    #endregion

    [Header("Glitch Settings")]

    #region Glitch Values
    [SerializeField, Range(0, 1)]
    private float _GlitchInterval = .16f;
    [SerializeField, Range(0, 1)]
    private float _DispProbability = 0.022f;
    [SerializeField, Range(0, 1)]
    private float _DispIntensity = 0.09f;
    [SerializeField, Range(0, 1)]
    private float _ColorProbability = 0.02f;
    [SerializeField, Range(0, 1)]
    private float _ColorIntensity = 0.07f;
    #endregion

    [Header("Scanning Settings")]
    #region Scanning Privates
    // Orgin for the sonar scan
    [SerializeField]
    public Transform _ScannerOrigin;
    private Transform scannerOrigin { get { return _ScannerOrigin; } set { _ScannerOrigin = value; } }

    [SerializeField] public Transform _playerPos;
    private Transform playerPos { get { return _playerPos; } set { _playerPos = value; } }

    //How far you want the scan to go
    [SerializeField, Range(0, 50)] public float scanDistance = 10;
    private float _ScanDistance { get { return scanDistance; } set { scanDistance = value; } }

    //Width of the scan (thickness)
    [SerializeField, Range(0, 20)] public float ScanWidth = 5;
    private float _ScanWidth { get { return ScanWidth; } set { ScanWidth = value; } }

    //Width of the scan (thickness)
    [SerializeField, Range(0, 50)] public float scanningDistanceMax = 20;
    private float _ScanningDistanceMax { get { return scanningDistanceMax; } set { scanningDistanceMax = value; } }

    //How fast to scan
    [SerializeField, Range(0, 20)] public float speed = 5;
    private float _Speed { get { return speed; } set { speed = value; } }

    [SerializeField] private bool isScanning;
    #endregion


    /// <summary>
    /// Getting required shader and mats
    /// </summary>
    [SerializeField]
    private Material _material;

    private Shader shader;
    private Camera _camera;


    public void OnEnable()
    {
        _camera = GetComponent<Camera>();
        _camera.depthTextureMode = DepthTextureMode.Depth;
    }

    /// <summary>
    /// 	Finding the objects to interact with
    /// 	Can be changed from C to something else. Not as input
    /// 	//option to either use the mouse click or press C.
    /// 	Just change InPut.GetKeyDown(KeyCode.C) to something else or even to the bool. 
    /// </summary>
    /// 

    public void StartScanning()
    {
        isScanning = true;
    }
    public void StopScanning()
    {
        isScanning = false;
    }

    public void Update()
    {
        Scan();
    }

    private void Scan()
    {
        if (isScanning)
        {
            _ScanDistance += Time.deltaTime * speed;
        }
        else
        {
            isScanning = false;
            _ScanDistance = 0;
        }


    }
    /// <summary>
    /// Object is displayed through computing frustun corners in order to find where the camera
    /// is and not display the effect on any other part except of the 4 corners of the camera. 
    /// It is set to show as a circle right. 
    /// This OnRenderImage requires us to create our own Graph.Blits in order for it to work
    /// </summary>


    #region ImageEffectFx
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (_material == null)
        {
            shader = Shader.Find("Kwaku/NightVisionWGlitch");
            _material = new Material(shader);
            _material.hideFlags = HideFlags.DontSave;
        }
        if (_mode == GlitchMode.GlitchOn)
        {
            _material.SetFloat("_GlitchInterval", _GlitchInterval);
            _material.SetFloat("_DispIntensity", _DispIntensity);
            _material.SetFloat("_DispProbability", _DispProbability);
            _material.SetFloat("_ColorIntensity", _ColorIntensity);
            _material.SetFloat("_ColorProbability", _GlitchInterval);
        }
        else
        {
            _material.SetColor("_MidColor", midColor);
            _material.SetColor("_TrailColor", trailColor);
            _material.SetFloat("_MidIntensity", _MidColorIntensity);
            _material.SetFloat("_TrailIntensity", _TrailColorIntensity);
        }

        _material.SetColor("_TrailColor", trailColor);
        _material.SetFloat("_MidIntensity", _MidColorIntensity);
        _material.SetFloat("_TrailIntensity", _TrailColorIntensity);


        _material.SetVector("_WorldSpaceScannerPos", playerPos.position);
        if (_ScanDistance < _ScanningDistanceMax)
        {
            _material.SetFloat("_ScanDistance", _ScanDistance);
        }
        else
        {
            _ScanDistance = 0;
        }
        _material.SetFloat("_ScanWidth", _ScanWidth);
        RaycastGraphBlits.RaycastCornerBlit(source, destination, _material);
    }
    #endregion
}