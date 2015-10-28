using UnityEngine;
using System.Collections;

public class PlayerEquipment : MonoBehaviour 
{
    public int repelGrenadeCount;
    public GameObject repelGrenadeGO;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(repelGrenadeCount > 0)
            {
                --repelGrenadeCount;
                Instantiate(repelGrenadeGO, new Vector3(transform.position.x, transform.position.y, transform.position.z + 1), Quaternion.identity);
                repelGrenadeGO.GetComponent<RepelNade>().isActive = true;
            }
        }
	}
}
