using UnityEngine;
using System.Collections;

public class SpinLights : MonoBehaviour {

	GameObject myReactor;
	// Use this for initialization
	void Start () 
	{
		myReactor = GameObject.Find("Reactor");

	}
	
	// Update is called once per frame
	void Update () 
	{
		if (myReactor.GetComponent<ReactorScript>().isDead == true)
			transform.Rotate(transform.forward.normalized * 1.5f, Space.World);
	
	}
}
