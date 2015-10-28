using UnityEngine;
using System.Collections;

public class DefConManagerScript : MonoBehaviour {


	public float currentEnemyCost = 0;
	public int currentDefConLevel = 5;

	GameObject[] CentralLights;
	bool changed = true;
	bool lightsOff = false;
	int number = 1;

	//THIS NEEDS TWEAKING
	//1 - green   -> less than COST 50
	//2 - blue    -> more than 50 less than 150
	//3 - yellow  -> more than 150 less than 350
	//4 - orange  -> more than 350 less than 600
	//5 - red     -> more than 600
	public Color color1 = Color.blue;
	public Color color2 = Color.black;

	GameObject myReactor;

	void Start () 
	{
		CentralLights = GameObject.FindGameObjectsWithTag("Light");
		myReactor = GameObject.Find("Reactor");

		Color meine;

		meine.r = 0.12f;
		meine.g = 0.56f;
		meine.b = 1.0f;
		meine.a = 0.75f;


		for (int i = 0; i < CentralLights.Length; i++ )
			CentralLights[i].light.color = meine;

	}

	void Update () 
	{
		if (changed == false)
			ChangeValues();

		if(myReactor != null && myReactor.GetComponent<ReactorScript>().isDead == false)
		{
			//turn lights on
			if(lightsOff == false)
			{
				TurnLightsOnOff();
				lightsOff = true;
			}
			
			if (currentDefConLevel <= 3)
			{
				for (int i = 0; i < CentralLights.Length; i++ )
				{
					//CentralLights[i].light.color = Color.Lerp(CentralLights[i].light.color, deadColor, 0.05f);
					if (number == 1)
						//light.intensity = Random.Range(0.5f, maxIntensity);
						StartCoroutine(waitforColor1(CentralLights[i]));
					else
						//light.intensity = Random.Range(0.5f, maxIntensity);
						StartCoroutine(waitforColor2(CentralLights[i]));
				}
			}
		}
		else
		{
			//turn lights off
			if(lightsOff == true)
			{
				TurnLightsOnOff();
				lightsOff = false;
			}
		}

	}

	void TurnLightsOnOff()
	{
		for (int i = 0; i < CentralLights.Length; i++ )
			CentralLights[i].light.enabled = !lightsOff;
	}

	void ChangeValues()
	{
		//check the total cost and calculate the DefCon Level
		if (currentEnemyCost <= 50)
			currentDefConLevel = 5;
		else if (currentEnemyCost > 50 && currentEnemyCost <= 150)
			currentDefConLevel = 4;
		else if (currentEnemyCost > 150 && currentEnemyCost <= 350)
			currentDefConLevel = 3;
		else if (currentEnemyCost > 350 && currentEnemyCost <= 600)
			currentDefConLevel = 2;
		else if (currentEnemyCost > 600)
			currentDefConLevel = 1;

		Color myCol = Color.white;

		switch(currentDefConLevel)
		{
			case(1):
			{
				myCol = Color.white;
				break;
			}
			case(2):
			{
				myCol = Color.red;
				break;
			}
			case(3):
			{
				myCol = Color.yellow;
				break;
			}
			case(4):
			{
				myCol = Color.green;
				break;
			}
			case(5):
			{
				myCol.r = 0.12f;
				myCol.g = 0.56f;
				myCol.b = 1.0f;
				myCol.a = 0.75f;

				break;
			}
		}
		color1 = myCol;
		changed = true;

		for (int i = 0; i < CentralLights.Length; i++ )
			CentralLights[i].light.color = color1;


	}

	public void ChangeCurrentEnemyCost(float cost)
	{
		currentEnemyCost += cost;
		changed = false;
	}

	IEnumerator waitforColor1(GameObject myLight)
	{
		yield return new WaitForSeconds (0.4f);
		myLight.light.color = color1;
		number = 2;
	}
	IEnumerator waitforColor2(GameObject myLight)
	{
		yield return new WaitForSeconds ( 0.4f);
		myLight.light.color = color2;
		number = 1;
	}

}
