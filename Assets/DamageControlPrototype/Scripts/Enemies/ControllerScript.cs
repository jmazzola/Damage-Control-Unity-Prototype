using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ControllerScript : MonoBehaviour {

	public List<GameObject> mySmalls = new List<GameObject>();
	public int myLength = 0;

	public EnemyBehaviourScript.StateTypes groupState = 0;
	public GameObject currentDestination = null;
	// Use this for initialization
	void Start () 
	{
//		enemyRadius = GetComponent<SmallEnemy>().transform.localScale.x/2;
//		maxSpeed = GetComponent<SmallEnemy>().movementSpeed;
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.position = mySmalls[0].transform.position;
	}

	public void AddSmall(GameObject small)
	{
		mySmalls.Add(small);
		myLength++;
	}

	
	public List<GameObject> GetMyArray()
	{
		return mySmalls;
	}

	public void RemoveSmall(GameObject small)
	{
		mySmalls.Remove(small);
		myLength--;
		if (myLength == 0)
			Destroy(gameObject);
	}

	public void NotifyStateChange(EnemyBehaviourScript.StateTypes state, GameObject defendTarget)
	{
		if (groupState != state)
		{
			switch(groupState)
			{
				case(EnemyBehaviourScript.StateTypes.eObjective):
				{
					GenerateRandomWayPoint();
					for (int i = 0; i < myLength; i++)
					{
						mySmalls[i].GetComponent<SmallEnemy>().myState = state;
					}
					break;		
				}
				case(EnemyBehaviourScript.StateTypes.ePatrol):
				{
					for (int i = 0; i < myLength; i++)
					{
						mySmalls[i].GetComponent<SmallEnemy>().myState = state;
						mySmalls[i].GetComponent<SmallEnemy>().currentBigAlly = defendTarget;
					}
					break;	
				}
				case(EnemyBehaviourScript.StateTypes.eRepel):
				{
					GenerateRandomWayPoint();
					for (int i = 0; i < myLength; i++)
					{
						mySmalls[i].GetComponent<SmallEnemy>().myState = state;
					}
					break;	
				}
			}
			groupState = state;
		}

	}

	public void GenerateRandomWayPoint()
	{
		GameObject[] myRooms = GameObject.FindGameObjectsWithTag("Room");
		int myNextRoom;
		do
		{
			myNextRoom = Random.Range(0, myRooms.Length - 1);
			
		} while (currentDestination == myRooms[myNextRoom]);
		currentDestination = myRooms[myNextRoom];

		for (int i = 0; i < myLength; i++)
		{
			mySmalls[i].GetComponent<SmallEnemy>().currentDestination = currentDestination;
		}
	}
}