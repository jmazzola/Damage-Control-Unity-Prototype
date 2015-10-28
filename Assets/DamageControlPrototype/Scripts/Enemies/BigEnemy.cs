using UnityEngine;
using System.Collections;

public class BigEnemy : EnemyBehaviourScript
{

    // Use this for initialization
    private Vector3 destination;
    public NavMeshAgent agent;
    private GameObject myPlayer;
    public float agroRange = 6.5f;

    public GameObject theEngine;
    public GameObject theLifeSupport;
    public GameObject theReactor;

    public GameObject[] mySystems;
    public int myCurrentSystem;

    public Animator myAnimator;
    //for animation translations
    public bool hasTarget = false;

    void Start()
    {
        //		enemyHealth = 150;
        //		movementSpeed = 4.5f;
        //		damage = 20.0f;
        //		scrapAmount = 50;
        //		attackType = 0;
        //		
        //		attackSpeed = 3.5f;
        //		attackRange = 6.5f;
        elapsedTime = attackSpeed;
        myPlayer = GameObject.Find("Player");

        FindClosestsTarget();

        agent = this.GetComponent<NavMeshAgent>();
        agent.updatePosition = true;
        //agent.updateRotation = true;

        theEngine = GameObject.Find("Engines");
        theLifeSupport = GameObject.Find("Life Support");
        theReactor = GameObject.Find("Reactor");
        myCurrentSystem = Random.Range(0, 2);

        mySystems = new GameObject[3];
        mySystems[0] = theEngine;
        mySystems[1] = theLifeSupport;
        mySystems[2] = theReactor;


        myAnimator = GetComponent<Animator>();
        this.GetComponent<EnemyBehaviourScript>().Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyHealth > 0)
        {
            EvaluateState();
            switch (myState)
            {
                case (StateTypes.eObjective):
                    {
                        UpdateObjectiveState();
                        break;
                    }
                case (StateTypes.ePatrol):
                    {
                        UpdatePatrolState();
                        break;
                    }
                case (StateTypes.eAgro):
                    {
                        UpdateAgroState();
                        break;
                    }
                case (StateTypes.eRepel):
                    {
                        UpdateRepelState();
                        break;
                    }
            }
        }
    }

    public void UpdateObjectiveState()
    {
        if (target != null)
        {
            if (IsTargetAlive(target) == false)
                FindClosestsTarget();
            if (target != null)
            {
                destination = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
                float distancetoTarget = Vector3.Distance(transform.position, target.transform.position);

                //turn
                Vector3 toTarget = destination - transform.position;
                float step = 10.0f * Time.deltaTime;
                Vector3 newDir = Vector3.RotateTowards(transform.forward, toTarget, step, 1.0F);

                transform.rotation = Quaternion.LookRotation(newDir);

                if (target.tag == "SubSystem")
                {
                    if (distancetoTarget <= attackRange)
                    { //hardcode the player's size/2
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
                }
            }
        }
        else
            myState = StateTypes.ePatrol;  //when all the subsystems are destroyed patrol between the subsystems

        //if (target.tag == "SubSystem") 

        Debug.DrawLine(transform.position, destination);
    }

    public void UpdatePatrolState()
    {
        if (CheckDestination())
            FindNextTarget();
        if (target != null)
        {
            destination = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
            float distancetoTarget = Vector3.Distance(transform.position, target.transform.position);

            //turn
            Vector3 toTarget = destination - transform.position;
            float step = 10.0f * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, toTarget, step, 1.0F);

            transform.rotation = Quaternion.LookRotation(newDir);

            if (distancetoTarget <= attackRange)
            { //hardcode the player's size/2
                agent.speed = 0;
                DealDamage();
            }
            else
            {
                agent.destination = destination;
                agent.speed = movementSpeed;
            }
        }
        else
            //agent.destination = spawner.transform.position; //go home
            myState = StateTypes.eObjective;  //when all the subsystems are destroyed patrol between the subsystems

        //if (target.tag == "SubSystem") 

        Debug.DrawLine(transform.position, destination);
    }

    public void UpdateAgroState()
    {
        destination = new Vector3(target.transform.position.x, transform.position.y, myPlayer.transform.position.z);
        float distancetoTarget = Vector3.Distance(transform.position, myPlayer.transform.position);

        //turn
        Vector3 toTarget = destination - transform.position;
        float step = 10.0f * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, toTarget, step, 1.0F);

        transform.rotation = Quaternion.LookRotation(newDir);

        if (target.tag == "Player")
        {
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
        }
        else
            myState = StateTypes.eObjective;

        Debug.DrawLine(transform.position, destination);
    }

    public void UpdateRepelState()
    {
        GameObject repel = GameObject.Find("RepelGrenade");
        if (repel != null && repel.GetComponent<RepelNade>().isActive == true)
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

    public void EvaluateState()
    {
        float distanceToPlayer = Vector3.Distance(myPlayer.transform.position, transform.position);

        if (myState != EnemyBehaviourScript.StateTypes.eRepel)
        {
            if (distanceToPlayer <= agroRange) //check the agrorange first
            {
                myState = StateTypes.eAgro;
                target = myPlayer;
            }
            else
            {
                switch (myState)
                {
                    case (StateTypes.eObjective):
                        {
                            if (IsTargetAlive(theEngine) == false && IsTargetAlive(theLifeSupport) == false
                            && IsTargetAlive(theReactor) == false)
                            {
                                FindNextTarget();
                                myState = StateTypes.ePatrol;
                                agroRange = 15.0f; //increase agroRange 
                            }
                            break;
                        }
                    case (StateTypes.ePatrol):
                        {
                            if (IsTargetAlive(theEngine) == true || IsTargetAlive(theLifeSupport) == true
                            || IsTargetAlive(theReactor) == true)
                            {
                                myState = StateTypes.eObjective;
                                FindClosestsTarget();
                                agroRange = attackRange; //decrease agroRange 	
                            }
                            break;
                        }
                    case (StateTypes.eAgro):
                        {
                            myState = StateTypes.eObjective;
                            break;
                        }
                }
            }
        }
    }

    public void DealDamage()
    {
        myAnimator.SetFloat("shotTime", elapsedTime);
        if (target != null && elapsedTime > attackSpeed)
        {
            elapsedTime = 0.0f;
            myAnimator.SetFloat("shotTime", elapsedTime);
            if (target == myPlayer)
                AttackPlayer();
            else if (target == theEngine)
                target.gameObject.GetComponent<EnginesScript>().TakeDamage(damage);
            else if (target == theLifeSupport)
                target.gameObject.GetComponent<LifeSupportScript>().TakeDamage(damage);
            else if (target == theReactor)
                target.gameObject.GetComponent<ReactorScript>().TakeDamage(damage);
        }
        else
            elapsedTime += Time.deltaTime;
    }

    public bool IsTargetAlive(GameObject target)
    {
        if (target == theEngine && target.GetComponent<EnginesScript>().GetIfDead() == false)
            return true;
        else if (target == theLifeSupport && target.GetComponent<LifeSupportScript>().GetIfDead() == false)
            return true;
        else if (target == theReactor && target.GetComponent<ReactorScript>().GetIfDead() == false)
            return true;
        else
            return false;
    }

    public void FindClosestsTarget()
    {
        GameObject[] mySystems = GameObject.FindGameObjectsWithTag("SubSystem");
        //check the closests
        float closests = 10000000;
        int index = -1;

        for (int i = 0; i < mySystems.Length; i++)
        {
            float distancetoTarget = Vector3.Distance(transform.position, mySystems[i].transform.position);

            if (IsTargetAlive(mySystems[i]) && distancetoTarget < closests)
            {
                index = i;
                closests = distancetoTarget;
            }
        }

        if (index != -1)
            target = mySystems[index];
        else
        {
            FindNextTarget();
            myState = StateTypes.ePatrol;
        }
    }

    public void FindNextTarget()
    {
        myCurrentSystem++;
        if (myCurrentSystem > 2)
            myCurrentSystem = 0;

        target = mySystems[myCurrentSystem];
    }

    public bool CheckDestination()
    {
        float distancetoTarget = Vector3.Distance(target.transform.position, transform.position);

        if (distancetoTarget <= attackRange + 5.0f)
            return true;
        return false;
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
}
