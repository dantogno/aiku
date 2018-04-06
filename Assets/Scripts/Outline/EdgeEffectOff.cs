using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class EdgeEffectOff : EdgeEffect {

    protected override void CreateMaterialsIfNeeded()
    {
        if (outlineShader == null)
            outlineShader = Resources.Load<Shader>("EdgeShaderOff");
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
}
