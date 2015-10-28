using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScrapManager : MonoBehaviour {

    public int Scrap;

	// Use this for initialization
	void Start () 
	{
		//UIManager.instance.Health = 50.0f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		Text t = GetComponent<Text> ();
		t.text = Scrap.ToString ();
	}
}
