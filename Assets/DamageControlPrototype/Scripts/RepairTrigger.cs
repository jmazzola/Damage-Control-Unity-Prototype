using UnityEngine;
using System.Collections;

public class RepairTrigger : MonoBehaviour 
{

    public bool isInTrigger;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            isInTrigger = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
            isInTrigger = false;
    }
	
}
