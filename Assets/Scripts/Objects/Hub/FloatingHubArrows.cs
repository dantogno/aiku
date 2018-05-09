using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingHubArrows : MonoBehaviour
{
    [SerializeField, Tooltip("Arrow to toggle")]
    private GameObject arrow;

    [SerializeField, Tooltip("What is the arrow for?")]
    private ArrowType arrowType;

    [SerializeField, Tooltip("If this is for the hologen, this is the hologen's shutdown button.")]
    private RingPuzzleButton button;

    private enum ArrowType { hologen, mineralBox }

    private void Awake()
    {
        arrow.SetActive(false);
    }

    private void OnEnable()
    {
        HubSceneChanger.FinishedLevel += ActivateArrow;
        if (arrowType == ArrowType.hologen) button.ButtonPressed += DeactivateArrow;
    }
    private void OnDisable()
    {
        HubSceneChanger.FinishedLevel -= ActivateArrow;
        if (arrowType == ArrowType.hologen) button.ButtonPressed -= DeactivateArrow;
    }

    private void ActivateArrow(HubSceneChanger.CrewmemberName crewmember)
    {
        if (arrow != null)
        {
            switch (crewmember)
            {
                case HubSceneChanger.CrewmemberName.Norma:
                    break;
                case HubSceneChanger.CrewmemberName.Trevor:
                    if (arrowType == ArrowType.mineralBox) arrow.SetActive(true);
                    break;
                case HubSceneChanger.CrewmemberName.Ray:
                    if (arrowType == ArrowType.hologen) arrow.SetActive(true);
                    break;
                default:
                    break;
            }
        }
    }

    private void DeactivateArrow()
    {
        if (arrow != null)
        {
            arrow.SetActive(false);
        }
    }
}
