using UnityEngine;
using System.Collections;

public class MediumEnemy :  EnemyBehaviourScript {
	
	// Use this for initialization
	private Vector3 destination;
	public NavMeshAgent agent;
	public GameObject myPlayer;

	public Animator myAnimator;
	//for animation translations
	public bool hasTarget = false;
	
	void Start () 
	{
//		enemyHealth = 35;
//		movementSpeed = 5.0f;
//		damage = 4.5f;
//		scrapAmount = 10;
//		attackType = 0;
//		
//		attackSpeed = 2.0f;
//		attackRange = 4.0f;
		elapsedTime = attackSpeed;
		
		myPlayer = GameObject.FindGameObjectWithTag ("Player");
		agent = this.GetComponent<NavMeshAgent>();
		agent.updatePosition = true;

		myAnimator = GetComponent<Animator>();
		this.GetComponent<EnemyBehaviourScript>().Start();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (enemyHealth > 0)
		{
			if (myState == EnemyBehaviourScript.StateTypes.eObjective)
				UpdateObjectiveState();
			else //Repel
				UpdateRepelState();
		}
	}

	public void UpdateObjectiveState()
	{
		elapsedTime += Time.deltaTime;

		target = myPlayer;
		if (target != null)
		{
			destination = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
			float distancetoTarget = Vector3.Distance(transform.position, target.transform.position);

			//rotate
			Vector3 toTarget = destination - transform.position;
			float step = 10.0f * Time.deltaTime;
			Vector3 newDir = Vector3.RotateTowards(transform.forward, toTarget, step, 1.0F);
	
			transform.rotation = Quaternion.LookRotation(newDir);
			
			if (target.tag == "Player") 
			{
				if (distancetoTarget <= attackRange) //hardcode the player's size/2 
				{
					hasTarget = true;
					myAnimator.SetBool("hasTarget", hasTarget);
					agent.speed = 0;
					DealDamage();
				}
				else
				{
					hasTarget = false;
					myAnimator.SetBool("hasTarget", hasTarget);
					agent.destination = destination;
					agent.speed = movementSpeed;
				}

				Debug.DrawLine(transform.position, destination);
			}		
		}
	}

	public void UpdateRepelState()
	{
		GameObject repel = GameObject.Find("RepelGrenade");
		if( repel != null && repel.GetComponent<RepelNade>().isActive == true)
		{
			if (target != null)
			{
				destination = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
				float distancetoTarget = Vector3.Distance(target.transform.position, transform.position);
				if (CheckDestination())
					GenerateRandomWayPoint();
				else
				{
					agent.destination = destination;
					agent.speed = movementSpeed;
				}
				
				Debug.DrawLine(transform.position, destination);
			}
		}
		else
		{
			myState = StateTypes.eObjective;
		}
	}
	
	public void DealDamage ()
	{
		if (target != null && elapsedTime > attackSpeed ) 
		{
			float lr = Random.Range(1, 120);
			if(lr <=40)
				myAnimator.Play("monster2Attack1");
			else if(lr > 40 && lr <= 80)
				myAnimator.Play("monster2attack2");
			else
				myAnimator.Play("monster2Attack3");


			elapsedTime = 0.0f;
			AttackPlayer();
		}
	}

	public void GenerateRandomWayPoint()
	{
		GameObject[] myRooms = GameObject.FindGameObjectsWithTag("Room");
		int myNextRoom;
		do
		{
			myNextRoom = Random.Range(0, myRooms.Length - 1);
			
		} while (target == myRooms[myNextRoom]);
		target = myRooms[myNextRoom];
	}

	public bool CheckDestination()
	{
		float distancetoTarget = Vector3.Distance(target.transform.position, transform.position);
		
		if (distancetoTarget <= transform.localScale.x + 5.0f)
			return true;
		return false;
	}
}