using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffMeshRenderer : MonoBehaviour {


    private MeshRenderer[] phoneMesh;
    private Canvas[] canvasMesh;


    [SerializeField]
    [Tooltip("Drag the examination from the phone. Set to disable via Animation")]
    private GameObject exitConsole;


    [SerializeField]
    [Tooltip("Drag the Console Here")]
    private Examination examinationScript;
	// Use this for initialization
	void Start () {

        phoneMesh =  GetComponentsInChildren<MeshRenderer>();
        canvasMesh = GetComponentsInChildren<Canvas>();

        foreach (MeshRenderer mesh in phoneMesh)
        {

            mesh.enabled = false;
            
        }
        foreach (Canvas canvas in canvasMesh)
        {

            canvas.enabled = false;

        }

        examinationScript.FinishInspect();
        examinationScript.enabled = false;
        exitConsole.SetActive(true);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
