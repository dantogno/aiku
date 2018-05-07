using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayHologenVO_DW : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The puzzle which, upon completion, affects this object.")]
    private RingPuzzle puzzle;

    private void OnEnable()
    {
        puzzle.PuzzleUnlocked += PlayAudio;
    }
    private void OnDisable()
    {
        puzzle.PuzzleUnlocked -= PlayAudio;
    }

    public void PlayAudio()
    {
        RingVO ringVO = GetComponent<RingVO>();

        if (ringVO != null) ringVO.TriggerVOAudio();
    }
}
