using UnityEngine;
using System.Collections;

public class ConstantSpawner : MonoBehaviour
{
	
	//The object being spawned
	// public GameObject monsterToSpawn;
	
	//How many spawn at a time
	public int minSpawnCount = 1;
	//Max spawned per batch
	public int maxSpawnCount = 5;
	
	//The frequency in which they spawn
	public float minSpawnFrequency = 1.0f;
	//Adjusts the spawn rate by a max of this
	public float maxSpawnFrequency = 10.0f;
	//Time remaining until the next spawn (Shouldn't have to touch this)
	public float timeToSpawn = 0.0f;
	
	//How far away (x & z) object will spawn
	public float spawnRadius = 2.0f;
	//The color of the debug grid
	public Color debugColor = Color.magenta;
	
	public bool isActive = true;
	public float coolDown = 1.0f;
	public float waitedTime = 0.0f; // to cooldown

	public GameObject smallEnemyPrefab;
	public GameObject controllerPrefab;

	public bool controllerCreated = false;
	public int maxPackNumber = 6; // needs to be tested
	public int currentPack = 0;

	public GameObject defCon;
	public int smallEnemyCost = 10;

	//increment small enemy amount by 1 every 20s
	public float incrementTime = 20.0f;
	public float currentIncrement = 0.0f;


	void Start()
	{
		
		if (minSpawnCount > maxSpawnCount)
			maxSpawnCount = minSpawnCount;
		if (minSpawnFrequency > maxSpawnFrequency)
			maxSpawnFrequency = minSpawnFrequency;
		
		timeToSpawn = Random.Range(minSpawnFrequency, maxSpawnFrequency);
		//particleSystem.enableEmission = false;
		GetComponent<ParticleRenderer>().enabled = false;
		

		defCon = GameObject.FindGameObjectWithTag("DefCon");
	}
	
	// Update is called once per frame
	void Update()
	{
		if (isActive)
		{
			currentIncrement += Time.deltaTime;
			if(currentIncrement > incrementTime)
			{
				AddMinMax(1);
				currentIncrement = 0.0f;
			}
			//particleSystem.enableEmission = true;
			GetComponent<ParticleRenderer>().enabled = true;
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
		else
		{
			waitedTime += Time.deltaTime;
			if (waitedTime >= coolDown)
			{
				waitedTime = 0.0f;
				isActive = true;
			}
		}
	}
	
	void SpawnMonster()
	{
		GameObject myController = null;
		int spawnedAmount = 0;
		int amountLimit = Random.Range(minSpawnCount, maxSpawnCount);
		while (spawnedAmount < amountLimit)
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
			
			currentPack++;
			spawnedAmount++;

			if(defCon != null)
				defCon.GetComponent<DefConManagerScript>().ChangeCurrentEnemyCost(smallEnemyCost);
		}
		if (myController != null)
			myController.GetComponent<ControllerScript>().GenerateRandomWayPoint();
		controllerCreated = false;
		currentPack = 0;
		isActive = false;
		//particleSystem.enableEmission = false;
		GetComponent<ParticleRenderer>().enabled = false;
	}

	public void AddMinMax(int amount)
	{
		minSpawnCount += amount;
		maxSpawnCount += amount;
	}
}