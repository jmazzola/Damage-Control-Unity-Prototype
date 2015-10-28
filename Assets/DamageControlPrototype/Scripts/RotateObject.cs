using UnityEngine;
using System.Collections;

public class RotateObject : MonoBehaviour {

    public Vector3 axis;
    public float speed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        transform.Rotate(axis, speed * Time.deltaTime);
	}
}
