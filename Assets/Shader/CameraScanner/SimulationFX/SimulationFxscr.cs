using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationFxscr : MonoBehaviour
{

	#region Private Variables
	//intensity of simulation
	[Header("Intensity Settings")]
	[SerializeField, Range(0, 4)] float _intensity = 0.5f;
	public bool cameraBackgroundMatchesBaseColor = true;


	// Wave color
	[Header("Color Settings")]

	[SerializeField] public Color32 _MidColor = new Color(255, 255, 255, 255);
	private Color midColor { get { return _MidColor; } set { _MidColor = value; } }

	// Orgin for the sonar scan
	[Header("Scanning Origin Settings")]
	[SerializeField] public Transform _ScannerOrigin;
	private Transform scannerOrigin { get { return _ScannerOrigin; } set { _ScannerOrigin = value; } }

	[SerializeField] public Transform _playerPos;
	private Transform playerPos { get { return _playerPos; } set { _playerPos = value; } }


	//Material to use
	[SerializeField]
	private Material _material;

	//Shader we will be using
	private Shader shader;

	private Camera _camera;

	[Header("Scanning Settings")]
	//How far you want the scan to go
	[SerializeField, Range(0, 50)] public float scanDistance = 10;
	private float _ScanDistance { get { return scanDistance; } set { scanDistance = value; } }

	//Width of the scan (thickness)
	[SerializeField, Range(0, 50)] public float ScanWidth = 30;
	private float _ScanWidth { get { return ScanWidth; } set { ScanWidth = value; } }

	//Width of the scan (thickness)
	[SerializeField, Range(0, 50)] public float scanningDistanceMax = 20;
	private float _ScanningDistanceMax { get { return scanningDistanceMax; } set { scanningDistanceMax = value; } }
	
	//How fast to scan
	[SerializeField, Range(0, 20)] public float speed = 5;
	private float _Speed { get { return speed; } set { speed = value; } }

	[SerializeField] private bool isScanning;
	#endregion

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
	/// It is set to show as a circle projected from the matrices. 
	/// This OnRenderImage requires us to create our own Graph.Blits in order for it to work
	/// </summary>
	/// 
	#region ImageEffects
	[ImageEffectOpaque]
	void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (_material == null)
		{
			shader = Shader.Find("Kwaku/SimulationFX");
			_material = new Material(shader);
			_material.hideFlags = HideFlags.DontSave;
		}
		
		_material.SetColor("_MidColor", midColor);

		//Where to start the scan in the world
		_material.SetVector("_WorldSpaceScannerPos", _ScannerOrigin.position);

		//Scan Distance Max will be how far we want the scan to go.
		if (_ScanDistance < _ScanningDistanceMax)
		{
			_material.SetFloat("_ScanDistance", _ScanDistance);
		}
		else
		{
			_ScanDistance = 0;
		}
	

		_material.SetFloat("_Distance", _ScanDistance);
		_material.SetFloat("_ScanWidth", _ScanWidth);
		_material.SetFloat("_Intensity", _intensity);

		
		RaycastGraphBlits.RaycastCornerBlit(source, destination, _material);
		
	}
#endregion

}

