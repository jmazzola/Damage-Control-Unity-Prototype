using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

    public enum DoorStatus { CLOSED = 0, OPENING, OPEN, CLOSING, SEALED, BROKEN };

    //The part that opens
    public GameObject leftSlider;
    //The other part that opens
    public GameObject rightSlider;
    //Make door sounds
    public AudioClip openSound;
    public AudioClip closeSound;
    //Door's operation status in the game
    public DoorStatus condition = DoorStatus.CLOSED;
    //Positive and negative x position to move the sliders
    //Fine tuned, must adjust if door is scaled
    private float closePosition = 0.205f;
    private float openPosition = 0.555f;
    //The speed at which the door opens/closes
    public float doorSpeed = 10.0f;
    //Health parameters
    public float currHealth = 100;
    public float maxHealth = 100;
    //Radius of occupant detection
    public float detectRadius = 3;

    //Supported tags
    public string[] supportedTags;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        GetOccupants();

        //Slide the doors closed
        if (condition == DoorStatus.CLOSING)
        {
            //Move the doors
            rightSlider.transform.Translate(doorSpeed * Time.deltaTime, 0, 0);
            leftSlider.transform.Translate(-doorSpeed * Time.deltaTime, 0, 0);

            //Are we on target
            if (leftSlider.transform.localPosition.x <= closePosition)
            {
                //Correct them
                rightSlider.transform.localPosition =
                    new Vector3(-closePosition,
                    rightSlider.transform.localPosition.y,
                    rightSlider.transform.localPosition.z);
                leftSlider.transform.localPosition =
                    new Vector3(closePosition,
                    leftSlider.transform.localPosition.y,
                    leftSlider.transform.localPosition.z);

                condition = DoorStatus.CLOSED;
            }
        }
        //Slide the doors open
        else if(condition == DoorStatus.OPENING)
        {
            //Debug.Log("Right SLider X Position: " + rightSlider.transform.localPosition.x);
            //Move the doors
            rightSlider.transform.Translate(-doorSpeed * Time.deltaTime, 0, 0);
            leftSlider.transform.Translate(doorSpeed * Time.deltaTime, 0, 0);

            //Are we on target
            if (leftSlider.transform.localPosition.x >= openPosition)
            {
                //Correct them
                rightSlider.transform.localPosition = 
                    new Vector3(-openPosition, 
                    rightSlider.transform.localPosition.y,
                    rightSlider.transform.localPosition.z);
                leftSlider.transform.localPosition = 
                    new Vector3(openPosition, 
                    leftSlider.transform.localPosition.y,
                    leftSlider.transform.localPosition.z);

                condition = DoorStatus.OPEN;
            }
        }
	}

    public void GetOccupants()
    {
        Collider[] collisions = Physics.OverlapSphere(transform.position, detectRadius);
        bool viable = false;
        //Find which collisions are viable
        foreach (Collider col in collisions)
        {
            foreach (string tag in supportedTags)
            {
                if (col.gameObject.tag == tag)
                {
                    viable = true;
                    break;
                }
            }
            if (viable)
                break;
        }
        //Empty? close the door
        if (!viable && (condition == DoorStatus.OPENING || condition == DoorStatus.OPEN))
        {
            if (closeSound)
                GetComponent<AudioSource>().PlayOneShot(closeSound);
            condition = DoorStatus.CLOSING;
            //Debug.Log("Door Closed");
        }
        //Someone, any one is here, open the door
        else if (viable && (condition == DoorStatus.CLOSED || condition == DoorStatus.CLOSING))
        {
            if (openSound)
                GetComponent<AudioSource>().PlayOneShot(openSound);
            condition = DoorStatus.OPENING;
            //Debug.Log("Door Opened");
        }
    }

    public void Repair(float heal)
    {
        //Let's unify these bad boys!
        TakeDamage(-heal);
    }

    public void TakeDamage(float damage)
    {
        //Apply the change
        currHealth -= damage;

        //Repair this
        if (damage < 0)
        {
            if (currHealth >= maxHealth)
            {
                currHealth = maxHealth;
                //Repairing from a broken state
                if (condition == DoorStatus.BROKEN)
                    condition = DoorStatus.OPEN;
            }
        }
        //The door is broken
        else if (currHealth <= 0)
        {
            condition = DoorStatus.BROKEN;
            currHealth = 0;
        }
    }


}
