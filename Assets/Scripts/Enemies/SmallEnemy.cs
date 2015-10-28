using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SmallEnemy : EnemyBehaviourScript
{

    // Use this for initialization
    private Vector3 destination;
    public NavMeshAgent agent;
    public ControllerScript myController;
	
    private GameObject myPlayer;
    public float agroRange = 10.0f;
    public float defendRange = 15.0f;
	public float distanceToDefend = 5.0f;

	public float baseArgoRange = 10.0f;

    public GameObject currentDestination = null;//random movement
    public GameObject currentBigAlly; //defend buddy

	public Animator myAnimator;
	//for animation translations
	public bool hasTarget = false;

    void Start()
    {
//        enemyHealth = 10;
//        movementSpeed = 6.5f;
//        damage = 2.0f;
//        scrapAmount = 2;
//        attackType = 0;
//
//        attackSpeed = 1.5f;
//        attackRange = 1.0f;
		baseArgoRange = agroRange;
        elapsedTime = attackSpeed;


        myPlayer = GameObject.FindGameObjectWithTag("Player");
        agent = this.GetComponent<NavMeshAgent>();
        agent.updatePosition = true;
        
		myAnimator = GetComponent<Animator>();
		this.GetComponent<EnemyBehaviourScript>().Start();
    }

    void Update()
    {
        EvaluateState();
		CheckDefConState();
        switch (myState)
        {
            case (StateTypes.eObjective): //random movement
            {
                UpdateObjectiveState();
                break;
            }
            case (StateTypes.ePatrol): //defend
            {
                UpdatePatrolState();
                break;
            }
            case (StateTypes.eAgro): //attack
            {
                UpdateAgroState();
                break;
            }
			case (StateTypes.eRepel): //attack
			{
				UpdateRepelState();
				break;
			}
        }
    }

    public void UpdateObjectiveState() //random movement
    {
		if (currentDestination != null)
		{
       		destination = new Vector3(currentDestination.transform.position.x, transform.position.y, currentDestination.transform.position.z);
       		float distancetoTarget = Vector3.Distance(currentDestination.transform.position, transform.position);

			//turn
			Vector3 toTarget = destination - transform.position;
			float step = 10.0f * Time.deltaTime;
			Vector3 newDir = Vector3.RotateTowards(transform.forward, toTarget, step, 1.0F);
			
			transform.rotation = Quaternion.LookRotation(newDir);

            if (CheckDestination())
            {
                if (myController != null)
                    myController.GetComponent<ControllerScript>().GenerateRandomWayPoint();
            }
            else
            {
                agent.destination = destination;
                agent.speed = movementSpeed;
            }
	   		
       		Debug.DrawLine(transform.position, destination);
		}
    }

    public void UpdatePatrolState() //defend
    {
        destination = new Vector3(currentBigAlly.transform.position.x, transform.position.y, currentBigAlly.transform.position.z);
        float distancetoAlly = Vector3.Distance(transform.position, currentBigAlly.transform.position);

		Vector3 toTarget = destination - transform.position;
		float step = 10.0f * Time.deltaTime;
		Vector3 newDir = Vector3.RotateTowards(transform.forward, toTarget, step, 1.0F);
		
		transform.rotation = Quaternion.LookRotation(newDir);


        if (currentBigAlly != null && currentBigAlly.tag == "BigEnemy")
        {
			if (distancetoAlly <= distanceToDefend)
                agent.speed = 0;
            else
            {
                agent.destination = destination;
                agent.speed = movementSpeed;
            }
        }

        Debug.DrawLine(transform.position, destination);
    }

    public void UpdateAgroState()//attack
    {
        destination = new Vector3(myPlayer.transform.position.x, transform.position.y, myPlayer.transform.position.z);
        float distancetoTarget = Vector3.Distance(transform.position, myPlayer.transform.position);

		Vector3 toTarget = destination - transform.position;
		float step = 10.0f * Time.deltaTime;
		Vector3 newDir = Vector3.RotateTowards(transform.forward, toTarget, step, 1.0F);
		
		transform.rotation = Quaternion.LookRotation(newDir);

        if (distancetoTarget <= attackRange)
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

	public void UpdateRepelState()
	{
		GameObject repel = GameObject.Find("RepelGrenade");
		if( repel != null && repel.GetComponent<RepelNade>().isActive == true)
		{
			UpdateObjectiveState();
		}
		else
		{
			myState = StateTypes.eObjective;
			if(myController != null)
				myController.NotifyStateChange(myState, null);
		}
	}

	public void EvaluateState()
	{
		if(myState != EnemyBehaviourScript.StateTypes.eRepel)
		{
			float defendDistance = float.MaxValue;
			float distanceToPlayer = Vector3.Distance(myPlayer.transform.position, transform.position);
			FindClosestsBigAlly();
			if(currentBigAlly != null)
				defendDistance = Vector3.Distance(currentBigAlly.transform.position, transform.position);
			
			if (distanceToPlayer <= agroRange) //check the agrorange first
			{
				if (myState != StateTypes.eAgro)
				{
					myState = StateTypes.eAgro;

                    if(myController != null)
					    myController.GetComponent<ControllerScript>().NotifyStateChange(myState, null);
				}
			}
			else if (defendDistance <= defendRange) //check the defending second
			{
				if (myState != StateTypes.ePatrol)//defend
				{
					myState = StateTypes.eObjective;

                    if (myController != null)
					    myController.GetComponent<ControllerScript>().NotifyStateChange(myState, currentBigAlly);
				}
			}
			else //random movement
			{
				switch(myState)
				{
				case (StateTypes.eObjective): //random point
				{
					if (currentBigAlly != null)
					{
						if (defendDistance <= defendRange)
							myState = StateTypes.ePatrol;

                        if (myController != null)
						    myController.GetComponent<ControllerScript>().NotifyStateChange(myState, currentBigAlly);
						//agroRange = 15.0f; //increase/decrease agroRange 
					}
					break;
				}
				case (StateTypes.ePatrol): //defend
				{
					if (currentBigAlly == null)
					{
						myState = StateTypes.eObjective;

                        if (myController != null)
						    myController.GetComponent<ControllerScript>().NotifyStateChange(myState, null);
					}
					//agroRange = 15.0f; //increase/decrease agroRange
					break;
				}
				case (StateTypes.eAgro):
				{
					if (currentBigAlly != null &&  Vector3.Distance(currentBigAlly.transform.position, transform.position) > defendRange)
						myState = StateTypes.ePatrol;
					else
						myState = StateTypes.eObjective;

                    if (myController != null)
					    myController.GetComponent<ControllerScript>().NotifyStateChange(myState, null);
					break;
				}
				}
			}
		}
	}

    public void DealDamage()
    {
        if (myPlayer != null && elapsedTime > attackSpeed)
        {
			myAnimator.Play("Attack");
            elapsedTime = 0.0f;
            AttackPlayer();
        }
        else
            elapsedTime += Time.deltaTime;
    }


    public void FindClosestsBigAlly()
    {
        GameObject[] myBigAllies = GameObject.FindGameObjectsWithTag("BigEnemy");
        //check the closests
        float closests = 10000000;
        int index = -1;

        for (int i = 0; i < myBigAllies.Length; i++)
        {
            float distancetoTarget = Vector3.Distance(transform.position, myBigAllies[i].transform.position);

            if (distancetoTarget < closests)
            {
                index = i;
                closests = distancetoTarget;
            }
        }

        if (index != -1 && closests <= defendRange)
            currentBigAlly = myBigAllies[index];
        else
        {
            myState = StateTypes.eObjective; //random
            currentBigAlly = null;
        }
    }

    public bool CheckDestination()
    {
        float distancetoTarget = Vector3.Distance(currentDestination.transform.position, transform.position);

        if (distancetoTarget <= transform.localScale.x + 3.0f)
            return true;
        return false;
    }

	public void CheckDefConState()
	{
		int level = defCon.GetComponent<DefConManagerScript>().currentDefConLevel;

		switch(level)
		{
			case(1):
			{
				agroRange = baseArgoRange + (baseArgoRange * 4);
				break;
			}
			case(2):
			{
				agroRange = baseArgoRange + (baseArgoRange * 3);
				break;
			}
			case(3):
			{
				agroRange = baseArgoRange + (baseArgoRange * 2);
				break;
			}
			case(4):
			{
				agroRange = baseArgoRange + baseArgoRange;
				break;
			}
			case(5):
			{
				agroRange = baseArgoRange;
				break;
			}
		}
	}
}