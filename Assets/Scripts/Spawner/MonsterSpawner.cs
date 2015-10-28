using UnityEngine;
using System.Collections;

public class MonsterSpawner : MonoBehaviour
{

	//The frequency in which they spawn
	public float minSpawnFrequency = 2.0f;
	//Adjusts the spawn rate by a max of this
	public float maxSpawnFrequency = 8.0f;
	//Time remaining until the next spawn (Shouldn't have to touch this)
	public float timeToSpawn = 0.0f;
	
    //How far away (x & z) object will spawn
    public float spawnRadius = 2.5f;
    //The color of the debug grid
    public Color debugColor = Color.magenta;

    public bool isActive = true;
    public float basePoints = 60.0f;
    public float currentPoints = 60.0f;
    public float coolDown = 1.0f;
    public float waitedTime = 0.0f; // to cooldown

    public float smallEnemyCost = 10.0f;
    public float mediumEnemyCost = 30.0f;
    public float bigEnemyCost = 50.0f;
    public float mediumChance = 5.0f;
    public float maxMediumEnemyChance = 70.0f;
    public float bigChance = 0.0f;
    public float maxBigEnemyChance = 45.0f;

    public float chanceModifierBigEnemy = 5.0f;
    public float chanceModifierMediumEnemy = 7.0f;
    public GameObject bigEnemyPrefab;
    public GameObject smallEnemyPrefab;
    public GameObject mediumEnemyPrefab;

    public GameObject controllerPrefab;
    public bool controllerCreated = false;
    public int maxPackNumber = 6; // needs to be tested
    public int currentPack = 0;

	public bool firstTime = true;

	public int basePointIncrement = 50;

	public GameObject defCon;

    //public float chanceLimiterBigEnemy = 10.0f;

    // Use this for initialization
    void Start()
    {
		//particleSystem.enableEmission = false;
		GetComponent<ParticleRenderer>().enabled = false;
		defCon = GameObject.FindGameObjectWithTag("DefCon");

		timeToSpawn = Random.Range(minSpawnFrequency, maxSpawnFrequency);
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
			timeToSpawn -= Time.deltaTime;
			if (timeToSpawn <= 0)
			{
				//Will activate somewhere between min/max frequency
				timeToSpawn = Random.Range(minSpawnFrequency, maxSpawnFrequency);
				//if(monsterToSpawn != null)
				SpawnMonster();
			}
            //Draws in which objects will spawn
            if (Debug.isDebugBuild)
            {
                Debug.DrawLine(new Vector3(transform.position.x - spawnRadius, transform.position.y, transform.position.z - spawnRadius),
                    new Vector3(transform.position.x - spawnRadius, transform.position.y, transform.position.z + spawnRadius), debugColor);
                Debug.DrawLine(new Vector3(transform.position.x - spawnRadius, transform.position.y, transform.position.z - spawnRadius),
                   new Vector3(transform.position.x + spawnRadius, transform.position.y, transform.position.z - spawnRadius), debugColor);
                Debug.DrawLine(new Vector3(transform.position.x + spawnRadius, transform.position.y, transform.position.z + spawnRadius),
                   new Vector3(transform.position.x - spawnRadius, transform.position.y, transform.position.z + spawnRadius), debugColor);
                Debug.DrawLine(new Vector3(transform.position.x + spawnRadius, transform.position.y, transform.position.z + spawnRadius),
                   new Vector3(transform.position.x + spawnRadius, transform.position.y, transform.position.z - spawnRadius), debugColor);
            }
        }
    }

    void SpawnMonster()
    {
        //Will spawn something between the min/max spawn count
        GameObject myController = null;
        while (currentPoints >= smallEnemyCost)
        {
            float myChance = Random.Range(0.0f, 100.0f);

            if (currentPoints >= bigEnemyCost && bigChance > 0 && myChance <= bigChance)
            {
                Vector3 spawnPos = transform.position;
                spawnPos.x += Random.Range(-spawnRadius, spawnRadius);
                spawnPos.z += Random.Range(-spawnRadius, spawnRadius);
                GameObject monster = (GameObject)Instantiate(bigEnemyPrefab, spawnPos, Quaternion.identity);
                currentPoints -= bigEnemyCost;

				if(defCon != null)
					defCon.GetComponent<DefConManagerScript>().ChangeCurrentEnemyCost(bigEnemyCost);
            }
            else if (currentPoints >= mediumEnemyCost && mediumChance > 0 && myChance <= mediumChance)
            {
                Vector3 spawnPos = transform.position;
                spawnPos.x += Random.Range(-spawnRadius, spawnRadius);
                spawnPos.z += Random.Range(-spawnRadius, spawnRadius);
                GameObject monster = (GameObject)Instantiate(mediumEnemyPrefab, spawnPos, Quaternion.identity);
                currentPoints -= mediumEnemyCost;

				if(defCon != null)
					defCon.GetComponent<DefConManagerScript>().ChangeCurrentEnemyCost(mediumEnemyCost);
            }
            else //spawn small
            {
                if (controllerCreated == false || currentPack == maxPackNumber)
                {
					if(currentPack == maxPackNumber)
						myController.GetComponent<ControllerScript>().GenerateRandomWayPoint();
                    currentPack = 0;
                    Vector3 spawnPose = transform.position;
                    myController = (GameObject)Instantiate(controllerPrefab, spawnPose, Quaternion.identity);
                    controllerCreated = true;
                }
                Vector3 spawnPos = transform.position;
                spawnPos.x += Random.Range(-spawnRadius, spawnRadius);
                spawnPos.z += Random.Range(-spawnRadius, spawnRadius);
                GameObject monster = (GameObject)Instantiate(smallEnemyPrefab, spawnPos, Quaternion.identity);
                myController.GetComponent<ControllerScript>().AddSmall(monster);
                monster.GetComponent<SmallEnemy>().myController = myController.GetComponent<ControllerScript>();

                currentPoints -= smallEnemyCost;
                currentPack++;

				if(defCon != null)
					defCon.GetComponent<DefConManagerScript>().ChangeCurrentEnemyCost(smallEnemyCost);
            }
        }
		if (myController != null)
			myController.GetComponent<ControllerScript>().GenerateRandomWayPoint();
        controllerCreated = false;
        currentPack = 0;
        isActive = false;
        //particleSystem.enableEmission = false;
		GetComponent<ParticleRenderer>().enabled = false;
    }

	public void ActivateMe()
	{
		isActive = true;
		float distance = GameObject.Find("Engines").GetComponent<EnginesScript>().DistanceTraveled;
		
		basePoints += Random.Range(0, basePointIncrement);
		currentPoints += basePoints;
				
		if (firstTime == false && GameObject.Find("Engines").GetComponent<EnginesScript>().GetIfDead() == false)
		{
			float initChance = distance / GameObject.Find("Engines").GetComponent<EnginesScript>().GoalDistance;
			
			if (initChance > 0.1f && mediumChance <= initChance)
				bigChance += chanceModifierBigEnemy;
			
			if (mediumChance > maxMediumEnemyChance)
				mediumChance = maxMediumEnemyChance;
			
			if (initChance > 0.1f && bigChance <= initChance)
				bigChance += chanceModifierBigEnemy;
			
			if (bigChance > maxBigEnemyChance)
				bigChance = maxBigEnemyChance;
		}
		//particleSystem.enableEmission = true;
		GetComponent<ParticleRenderer>().enabled = true;
		firstTime = false;
	}

	public void DeactivateMe()
	{
		isActive = false;
	}

	public void SetMaxBigChance(float chance)
	{
		maxBigEnemyChance = chance;
	}

	public void SetMaxMediumChance(float chance)
	{
		maxMediumEnemyChance = chance;
	}
}