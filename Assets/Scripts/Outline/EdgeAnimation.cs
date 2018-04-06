using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeAnimation : MonoBehaviour {

	bool pingPong = false;
    EdgeEffect edgeEffect;
    EdgeEffectOff edgeEffectOff;

    private void Awake()
    {
        edgeEffect = GetComponent<EdgeEffect>();
        edgeEffectOff = GetComponent<EdgeEffectOff>();
    }

    // Update is called once per frame
    void Update()
	{
		float alpha = edgeEffect.lineColor.a;

		if (pingPong) //if pingPong is false then activate.
		{
			alpha += Time.deltaTime;

			if (alpha >= 1)
				pingPong = false;
		}
		else //if pingpong is true then activate is false
		{
            alpha -= Time.deltaTime;

			if (alpha <= 0)
				pingPong = true;

		}

        alpha = Mathf.Clamp01(alpha);
        edgeEffect.lineColor.a = alpha;
        edgeEffect.UpdateMaterialsPublicProperties();
        edgeEffectOff.lineColor.a = alpha;
        edgeEffectOff.UpdateMaterialsPublicProperties();
    }

}
