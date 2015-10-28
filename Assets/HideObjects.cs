using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HideObjects : MonoBehaviour
{
    //What to watch
    public GameObject player;
    //What layer will we ignore
    public LayerMask occludeLayer;
    //Everyone currently transparent
    private List<MeshRenderer> clientele;

    void Start()
    {
        clientele = new List<MeshRenderer>();
        if (player == null)
            Destroy(this);
    }

    void Update()
    {
        //Turn on the old clients' renderers
        if (clientele.Count > 0)
        {
            foreach (MeshRenderer item in clientele)
                item.enabled = true;
            clientele.Clear();
        }

        Vector3 toPlayer = player.transform.position - transform.position;
        //Cast a ray from this object's transform the the watch target's transform.
        RaycastHit[] hits = Physics.RaycastAll(transform.position, toPlayer, toPlayer.magnitude, occludeLayer);

        //Loop through all overlapping objects and disable their mesh renderer
        if (hits.Length > 0)
        {
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.gameObject.transform != player && hit.collider.transform.root != player)
                {
                    hit.collider.gameObject.GetComponent<MeshRenderer>().enabled = false;
                    clientele.Add(hit.collider.gameObject.GetComponent<MeshRenderer>());
                }
            }
        }
    }
}