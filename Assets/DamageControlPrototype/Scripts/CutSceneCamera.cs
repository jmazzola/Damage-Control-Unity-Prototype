using UnityEngine;
using System.Collections;

public class CutSceneCamera : MonoBehaviour {

	GameObject[] myExplosions;
	GameObject myReactor;
	GameObject mainGui, second;

	GameObject mainCamera;
	

	// Use this for initialization
	void Start () 
	{
		myExplosions = GameObject.FindGameObjectsWithTag("Explosion");
		myReactor = GameObject.Find("Reactor");
		mainCamera = GameObject.FindGameObjectWithTag("MainCamera");

		mainGui = GameObject.Find("MainFaderPanel");
		second = GameObject.Find("SecondGui");

		GetComponent<Animator>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (myReactor.GetComponent<ReactorScript>().blown == true)
		{
			if(mainGui.guiTexture.enabled == true)
				FadeToBlack();

			if (mainGui.guiTexture.color == Color.black || mainGui.guiTexture.color.a > 200.0f)
				ChangeCamera();
		}
	}

	public void ActivateExplosions()
	{
		//myExplosions = GameObject.FindGameObjectsWithTag("Explosion");
		
		for (int i = 0; i < myExplosions.Length; i++)
		{
			myExplosions[i].GetComponent<ParticleEmitter>().emit = true;
		}
	}

	public void EndScene()
	{
		Application.LoadLevel(2);
	}

	void FadeToBlack()
	{
		mainGui.guiTexture.color = Color.Lerp(mainGui.guiTexture.color, Color.black, 0.2f);
	}

	void ChangeCamera()
	{
		mainCamera.transform.position = new Vector3(48.95f, 33.67f, -19.73f);
		mainGui.guiTexture.enabled = false;
		GetComponent<Animator>().enabled = true;
		//GetComponent<AudioSource>().Play();
	}
}
