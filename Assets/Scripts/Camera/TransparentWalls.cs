using UnityEngine;
using System.Collections;

public class TransparentWalls : MonoBehaviour {

    public GameObject player;
    public ArrayList transparencies;

	// Use this for initialization
	void Start () 
    {
        //Save ourselves the trouble
        if (player == null)
        {
            Debug.Log("Target not set: Terminating Script");
            Destroy(this);
        }
        else
            transparencies = new ArrayList();
	}
	
	// Update is called once per frame
	void Update () 
    {
        //Decloak what we were looking at last frame
        foreach (GameObject wall in transparencies)
        {
            Color temp = wall.renderer.material.color;
            temp.a = 1;
            wall.renderer.material.color = temp;
        }
        transparencies.Clear();

        //Find everything between the camera and the player
        Vector3 start = transform.position;
        Ray toPlayer = new Ray(start, player.transform.position - start);
        float distance = (player.transform.position - start).magnitude;
        RaycastHit hit;
        //Debug.Log("Distance: " + distance);
        //Cycle through everything in front of the player
        do
        {
            Physics.Raycast(toPlayer, out hit, distance);        

            //Look through dat wall!
            if(hit.collider != null )//&& hit.collider.gameObject.tag == "Wall")
            {
                transparencies.Add(hit.collider.gameObject);
                Color temp = hit.collider.gameObject.renderer.material.color;
                temp.a = 0.2f;
                hit.collider.gameObject.renderer.material.color = temp;
            }

            //Make a new ray from the last collision towards the player
            start = hit.point + toPlayer.direction * 0.1f;
            distance -= hit.distance;
            toPlayer = new Ray(start, toPlayer.direction);
        } while (hit.collider != null && hit.collider.gameObject != player && distance > 0);

        
	}
}
