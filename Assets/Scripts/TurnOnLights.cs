using UnityEngine;
using System.Collections;

public class TurnOnLights : MonoBehaviour {

	GameObject myReactor;
	// Use this for initialization
	void Start () 
	{
		light.enabled = false;
		myReactor = GameObject.Find("Reactor");
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (myReactor.GetComponent<ReactorScript>().isDead == true)
			light.enabled = true;
		else
			light.enabled = false;
	}
}
