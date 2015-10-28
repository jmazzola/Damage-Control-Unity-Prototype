using UnityEngine;
using System.Collections;

public class lightPulse : MonoBehaviour {

	float time;
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		time += Time.deltaTime;
		GetComponent<Light> ().range = Mathf.Abs(Mathf.Sin (time)) * 100.0f;
	}
}
