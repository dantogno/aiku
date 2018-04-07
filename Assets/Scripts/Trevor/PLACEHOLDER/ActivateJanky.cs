using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateJanky : MonoBehaviour 
{
	[SerializeField] private JankyVO jankyVO;
    [SerializeField] private GameObject returnTrigger;

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player") 
		{
			jankyVO.enabled = true;
            returnTrigger.GetComponent<BoxCollider>().enabled = true;
		}
	}
}
