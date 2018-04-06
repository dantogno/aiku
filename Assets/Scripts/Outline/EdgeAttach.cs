using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Renderer))]

public class EdgeAttach : MonoBehaviour
{

    // Use this for initialization
    public Renderer Renderer { get; private set; }
    public EdgeEffect effect { get; private set; }
    public EdgeEffect effectOff { get; private set; }
    public int color;
    public bool eraseRenderer;
    PowerableObject powerAttached;

    private void Awake()
    {
        Renderer = GetComponent<Renderer>();
        effect = Camera.main.GetComponent<EdgeEffect>();
        effectOff = Camera.main.GetComponent<EdgeEffectOff>();
        powerAttached = this.GetComponent<PowerExchanger>().ConnectedPowerable;
        powerAttached.OnPoweredOn += PowerOn;
        powerAttached.OnPoweredOff += PowerOff;
    }

    private void PowerOn()
    {
        effect.AddOutline(this);
        effectOff.RemoveOutline(this);
    }
    private void PowerOff()
    {
        effectOff.AddOutline(this);
        effect.RemoveOutline(this);
    }


    void OnEnable()
    {
        if (powerAttached.IsFullyPowered)
            effect.AddOutline(this);
        else
            effectOff.AddOutline(this);
    }
    void OnDisable()
    {
        effect.RemoveOutline(this);
        effectOff.RemoveOutline(this);
    }
}
