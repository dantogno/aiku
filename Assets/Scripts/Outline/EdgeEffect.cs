using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.XR;


[RequireComponent(typeof(Camera))]

public class EdgeEffect : MonoBehaviour
{

    private static EdgeEffect m_instance;
    public static EdgeEffect Instance
    {
        get
        {
            if (Equals(m_instance, null))
            {
                return m_instance = FindObjectOfType(typeof(EdgeEffect)) as EdgeEffect;
            }

            return m_instance;
        }
    }


    protected EdgeEffect() { }
    //	private LinkedSet<EdgeAttach> outlines = new LinkedSet<EdgeAttach>();
    //private EdgeAttach _outlines = new EdgeAttach();
    protected List<EdgeAttach> _outlines = new List<EdgeAttach>();

    [Range(1.0f, 6.0f)]
    public float lineThickness = 1.25f;
    [Range(0, 10)]
    public float lineIntensity = .5f;
    [Range(0, 1)]
    public float fillAmount = 0.2f;
    public Color lineColor = Color.red;


    [Header("Advanced settings")]
    public bool scaleWithScreenSize = true;
    [Range(0.1f, .9f)]
    public float alphaCutoff = .5f;
    public Camera sourceCamera;

    [Header("These settings can affect performance!")]
    public bool cornerOutlines = false;

    [HideInInspector]
    public Camera outlineCamera;
    protected Material outline1Material;
    protected Material outlineEraseMaterial;

    protected Shader outlineShader;
    protected Shader outlineBufferShader;
    [HideInInspector]
    public Material outlineShaderMaterial;
    [HideInInspector]
    public RenderTexture renderTexture;
    [HideInInspector]
    public RenderTexture extraRenderTexture;

    CommandBuffer commandBuffer;


    List<Material> materialBuffer = new List<Material>();

    protected Material CreateMaterial(Color emissionColor)
    {
        //to make sure the material is visible
        Material m = new Material(outlineBufferShader);
        m.SetColor("_Color", emissionColor);
        m.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        m.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        m.SetInt("_ZWrite", 0);
        m.DisableKeyword("_ALPHATEST_ON");
        m.EnableKeyword("_ALPHABLEND_ON");
        m.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        m.renderQueue = 3000; //transparent
        return m;
    }
    private void Awake()
    {
        Cursor.visible = false;
        m_instance = this;
    }
    //public static GameObject manipulate;
    // Use this for initialization
    void Start()
    {
        CreateMaterialsIfNeeded();
        UpdateMaterialsPublicProperties();

        if (sourceCamera == null)
        {
            sourceCamera = GetComponent<Camera>();

            if (sourceCamera == null)
                sourceCamera = Camera.main;
        }

        if (outlineCamera == null)
        {
            GameObject cameraGameObject = new GameObject("Outline Camera");
            cameraGameObject.transform.parent = sourceCamera.transform;
            outlineCamera = cameraGameObject.AddComponent<Camera>();
            outlineCamera.enabled = false;
        }

        renderTexture = new RenderTexture(sourceCamera.pixelWidth, sourceCamera.pixelHeight, 16, RenderTextureFormat.Default);
        extraRenderTexture = new RenderTexture(sourceCamera.pixelWidth, sourceCamera.pixelHeight, 16, RenderTextureFormat.Default);
        UpdateOutlineCameraFromSource();

        commandBuffer = new CommandBuffer();
        outlineCamera.AddCommandBuffer(CameraEvent.BeforeImageEffects, commandBuffer);
    }
    public void OnPreRender()
    {
        if (commandBuffer == null)
            return;

        CreateMaterialsIfNeeded();

        //checking to see if there is a renderTexture and fixing the camera's height and width
        if (renderTexture == null || renderTexture.width != sourceCamera.pixelWidth || renderTexture.height != sourceCamera.pixelHeight)
        {
            renderTexture = new RenderTexture(sourceCamera.pixelWidth, sourceCamera.pixelHeight, 16, RenderTextureFormat.Default);
            extraRenderTexture = new RenderTexture(sourceCamera.pixelWidth, sourceCamera.pixelHeight, 16, RenderTextureFormat.Default);
            outlineCamera.targetTexture = renderTexture;
        }
        UpdateMaterialsPublicProperties();
        UpdateOutlineCameraFromSource();
        outlineCamera.targetTexture = renderTexture;
        commandBuffer.SetRenderTarget(renderTexture);

        commandBuffer.Clear();
        //~~~~~~~~~~~~~~~~~~~~~~~~~~we resized we checked cameras and we set the comandbuffer to target the outlinecameras renderer text now to to add on

        if (_outlines != null)
        {
            foreach (EdgeAttach outline in _outlines)
            {
                LayerMask l = sourceCamera.cullingMask;

                if (outline != null && l == (l | (1 << outline.gameObject.layer)))
                {
                    for (int v = 0; v < outline.Renderer.sharedMaterials.Length; v++)
                    {
                        Material m = null;

                        if (outline.Renderer.sharedMaterials[v].mainTexture != null && outline.Renderer.sharedMaterials[v])
                        {
                            foreach (Material g in materialBuffer)
                            {
                                if (g.mainTexture == outline.Renderer.sharedMaterials[v].mainTexture) // material matches get that material?
                                {//attach script here before we turn on the erase tool?
                                    if (outline.eraseRenderer && g.color == outlineEraseMaterial.color) //same result?
                                        m = g;
                                    else if (g.color == outline1Material.color) //same result?
                                        m = g;
                                }
                            }

                            if (m == null) //there was no same material in the list
                            {
                                if (outline.eraseRenderer)
                                    m = new Material(outlineEraseMaterial);
                                else
                                    m = new Material(outline1Material);
                                m.mainTexture = outline.Renderer.sharedMaterials[v].mainTexture;
                                materialBuffer.Add(m);
                            }
                        }
                        else
                        {
                            if (outline.eraseRenderer)
                                m = outlineEraseMaterial;
                            else
                                m = outline1Material;

                        }
                         //can these be above one forloop?
                        commandBuffer.DrawRenderer(outline.GetComponent<Renderer>(), m, 0, 0);
                        MeshFilter mL = outline.GetComponent<MeshFilter>();
                        if (mL)
                        {
                            if (mL.sharedMesh != null)
                            {
                                for (int i = 1; i < mL.sharedMesh.subMeshCount; i++)
                                    commandBuffer.DrawRenderer(outline.GetComponent<Renderer>(), m, i, 0);
                            }
                        }
                        SkinnedMeshRenderer sMR = outline.GetComponent<SkinnedMeshRenderer>();
                        if (sMR)
                        {
                            if (sMR.sharedMesh != null)
                            {
                                for (int i = 1; i < sMR.sharedMesh.subMeshCount; i++)
                                    commandBuffer.DrawRenderer(outline.GetComponent<Renderer>(), m, i, 0);
                            }
                        }
                    }
                }
            }
        }
        outlineCamera.Render();
    }
    private void OnEnable()
    {
        EdgeAttach[] o = FindObjectsOfType<EdgeAttach>();
        //??????
        foreach (EdgeAttach oL in o)
        {
            oL.enabled = false;
            oL.enabled = true;
        }
    }

    void OnDestroy()
    {
        if (renderTexture != null)
            renderTexture.Release();
        if (extraRenderTexture != null)
            extraRenderTexture.Release();
        DestroyMaterials();
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        outlineShaderMaterial.SetTexture("_OutlineSource", renderTexture);
        Graphics.Blit(source, destination, outlineShaderMaterial, 1);
    }
    protected virtual void CreateMaterialsIfNeeded()
    {
        if (outlineShader == null)
            outlineShader = Resources.Load<Shader>("EdgeShader");
        if (outlineBufferShader == null)
        {
            outlineBufferShader = Resources.Load<Shader>("EdgeBuffer");
        }
        if (outlineShaderMaterial == null)
        {
            outlineShaderMaterial = new Material(outlineShader);
            outlineShaderMaterial.hideFlags = HideFlags.HideAndDontSave;
            UpdateMaterialsPublicProperties();
        }
        if (outlineEraseMaterial == null)
            outlineEraseMaterial = CreateMaterial(new Color(0, 0, 0, 0));
        if (outline1Material == null)
            outline1Material = CreateMaterial(new Color(1, 0, 0, 0));
    }
    private void DestroyMaterials()
    {
        foreach (Material m in materialBuffer)
            DestroyImmediate(m);
        materialBuffer.Clear();
        DestroyImmediate(outlineShaderMaterial);
        DestroyImmediate(outlineEraseMaterial);
        DestroyImmediate(outline1Material);
        outlineShader = null;
        outlineBufferShader = null;
        outlineShaderMaterial = null;
        outlineEraseMaterial = null;
        outline1Material = null;
    }
    public virtual void UpdateMaterialsPublicProperties()
    {
        if (outlineShaderMaterial)
        {
            float scalingFactor = 1;
            if (scaleWithScreenSize)
            {
                // If Screen.height gets bigger, outlines gets thicker
                scalingFactor = Screen.height / 360.0f;
            }

            // If scaling is too small (height less than 360 pixels), make sure you still render the outlines, but render them with 1 thickness
            if (scaleWithScreenSize && scalingFactor < 1)
            {
                if (XRSettings.isDeviceActive && sourceCamera.stereoTargetEye != StereoTargetEyeMask.None)
                {
                    outlineShaderMaterial.SetFloat("_LineThicknessX", (1 / 1000.0f) * (1.0f / XRSettings.eyeTextureWidth) * 1000.0f);
                    outlineShaderMaterial.SetFloat("_LineThicknessY", (1 / 1000.0f) * (1.0f / XRSettings.eyeTextureHeight) * 1000.0f);
                }
                else
                {
                    outlineShaderMaterial.SetFloat("_LineThicknessX", (1 / 1000.0f) * (1.0f / Screen.width) * 1000.0f);
                    outlineShaderMaterial.SetFloat("_LineThicknessY", (1 / 1000.0f) * (1.0f / Screen.height) * 1000.0f);
                }
            }
            else
            {
                if (XRSettings.isDeviceActive && sourceCamera.stereoTargetEye != StereoTargetEyeMask.None)
                {
                    outlineShaderMaterial.SetFloat("_LineThicknessX", scalingFactor * (lineThickness / 1000.0f) * (1.0f / XRSettings.eyeTextureWidth) * 1000.0f);
                    outlineShaderMaterial.SetFloat("_LineThicknessY", scalingFactor * (lineThickness / 1000.0f) * (1.0f / XRSettings.eyeTextureHeight) * 1000.0f);
                }
                else
                {
                    outlineShaderMaterial.SetFloat("_LineThicknessX", scalingFactor * (lineThickness / 1000.0f) * (1.0f / Screen.width) * 1000.0f);
                    outlineShaderMaterial.SetFloat("_LineThicknessY", scalingFactor * (lineThickness / 1000.0f) * (1.0f / Screen.height) * 1000.0f);
                }
            }
            outlineShaderMaterial.SetFloat("_LineIntensity", lineIntensity);
            outlineShaderMaterial.SetFloat("_FillAmount", fillAmount);
            outlineShaderMaterial.SetColor("_LineColor1", lineColor * lineColor);
            if (cornerOutlines)
                outlineShaderMaterial.SetInt("_CornerOutlines", 1);
            else
                outlineShaderMaterial.SetInt("_CornerOutlines", 0);
            Shader.SetGlobalFloat("_EdgeAlphaCutoff", alphaCutoff);
        }
    }


    // Update the camera everyframe 
    void UpdateOutlineCameraFromSource()
    {
        outlineCamera.CopyFrom(sourceCamera);
        outlineCamera.renderingPath = RenderingPath.Forward;
        outlineCamera.backgroundColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        outlineCamera.clearFlags = CameraClearFlags.SolidColor;
        outlineCamera.rect = new Rect(0, 0, 1, 1);
        outlineCamera.cullingMask = 0;
        outlineCamera.targetTexture = renderTexture;
        outlineCamera.enabled = false;
#if UNITY_5_6_OR_NEWER
        outlineCamera.allowHDR = false;
#else
							outlineCamera.hdr = false;
#endif
    }

    public void AddOutline(EdgeAttach outline)
    {
        if (!_outlines.Contains(outline))
            _outlines.Add(outline);
    }

    public void RemoveOutline(EdgeAttach outline)
    {
        if (_outlines.Contains(outline))
            _outlines.Remove(outline);
    }
}
