using UnityEngine;
using System.Collections;

public class LogosScript : MonoBehaviour {

	public float fadeSpeed = 1.5f; 
	float timer = 3.0f;
	bool end = false;
	
	// Use this for initialization
	void Start () 
	{
		//audio.PlayDelayed(0.75f);
		//Screen.lockCursor = true;
		//Screen.showCursor = false;
		
	}
	
	// Update is called once per frame
	void Update () {
		// If the scene is starting...
		
		if (timer > 0) 
		{
			timer -= Time.deltaTime;	
		}
		
		if (timer < 0) 
		{
			EndScene();
		}

		if (end == true)
			LoadLevel ();
	}
	
	void FadeToClear ()
	{
		// Lerp the colour of the texture between itself and transparent.
		guiTexture.color = Color.Lerp(guiTexture.color, Color.clear, fadeSpeed * Time.deltaTime);
		
	}
	
	
	
	void EndScene ()
	{
		FadeToClear ();
		//almost visible
		if(guiTexture.color.a <=0.05f)
		{
			// ... set the colour to clear and disable the GUITexture.
			guiTexture.color = Color.clear;
			guiTexture.enabled = false;
			end = true;
		}
	}

	void LoadLevel()
	{
		Application.LoadLevel (1);
	}
	
}