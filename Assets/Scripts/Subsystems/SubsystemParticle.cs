using UnityEngine;
using System.Collections;

public class SubsystemParticle : MonoBehaviour {

	public GameObject mySubsystem;

	public GameObject theEngine;
	public GameObject theLifeSupport;
	public GameObject theReactor;


	// Use this for initialization
	void Start () 
	{
		//particleSystem.enableEmission = false;

		theEngine = GameObject.Find("Engines");
		theLifeSupport = GameObject.Find("Life Support");
		theReactor = GameObject.Find("Reactor");

	}
	
	// Update is called once per frame
	void Update () 
	{
		if (mySubsystem != null)
		{
			float percentage = 0;

            if (mySubsystem == theLifeSupport)
            {
                percentage = mySubsystem.GetComponent<LifeSupportScript>().CurrentHealth / mySubsystem.GetComponent<LifeSupportScript>().StartingHealth;
            }
            else if (mySubsystem == theEngine)
			{
				percentage = mySubsystem.GetComponent<EnginesScript>().CurrentHealth / mySubsystem.GetComponent<EnginesScript>().StartingHealth; 
			}
			else if (mySubsystem == theReactor)
			{
				percentage = mySubsystem.GetComponent<ReactorScript>().CurrentHealth / mySubsystem.GetComponent<ReactorScript>().StartingHealth; 
			}

			float size;
			if (percentage > 0.05f)
			{
				if (percentage == 1)
					size = 0;
				else
					size = 1.0f / percentage;
				particleSystem.startSize =  size;
			}
		}
	}
}
