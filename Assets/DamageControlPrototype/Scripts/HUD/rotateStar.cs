using UnityEngine;
using System.Collections;

public class rotateStar : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		Vector3 euler = new Vector3 (0, 0.1f, 0);
		gameObject.transform.Rotate(euler);
	}
}
