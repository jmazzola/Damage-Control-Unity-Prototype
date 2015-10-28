using UnityEngine;
using System.Collections;

public class EnemyBehaviourScript : MonoBehaviour 
{
	public enum AttackTypes {eMELEE = 0, eRANGE};
	public enum StateTypes {eObjective = 0, eAgro, ePatrol, eRepel};
	//objective = main duty
	// Agro = when it's in range
	//Patrol = after completing the objective

	//public int Level = 1;

	public float enemyHealth = 2.0f;
	public float movementSpeed = 2.0f;
	public float damage = 1.0f;
	public int scrapAmount = 1;
	public AttackTypes attackType = 0;
	public GameObject target;
	public float attackSpeed = 2.0f;
	public float attackRange = 1.0f;
	public float elapsedTime = 0.0f;
    public GameObject scrapPickup;
	public GameObject deathEffect;

	public GameObject defCon;
	public bool defIsSet = false;

	private float smallCost = 10;
	private float mediumCost = 25;
	private float bigCost = 50;
	
	    private bool isDead;
	
	public StateTypes myState = 0; // starts in objective state
	
    static Random rand = new Random();

	public void Start ()
	{
		defCon = GameObject.FindGameObjectWithTag("DefCon");
		 isDead = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void ChangeTarget(GameObject newTarget)
	{
		target = newTarget;
	}

    public void AttackPlayer()
    {
        PlayerHealth ph = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
		ph.TakeDamage(damage, PlayerDamageType.Injure);
    }

    public void TakeDamage(float amount)
    {
	     if (isDead)
            return;
			
		float lr = Random.Range(1, 100);

		if (this.tag == "BigEnemy")
		{
			BigEnemy myBig = this.GetComponent<BigEnemy>();
			//stop the movement when he is getting hit
			myBig.agent.speed = 0;

			if (lr <= 50)
				myBig.GetComponent<Animator>().Play("get a hit (L)");
			else
				myBig.GetComponent<Animator>().Play("get a hit (R)");

		}
		else if (this.tag == "Enemy")
		{
			MediumEnemy myMed = this.GetComponent<MediumEnemy>();

			//stop the movement when he is getting hit
			myMed.agent.speed = 0;
			
			if (lr <= 50)
				myMed.GetComponent<Animator>().Play("monster2Hit1");
			else
				myMed.GetComponent<Animator>().Play("monster2Hit2");
			
		}
		else
		{
			SmallEnemy mySmall = this.GetComponent<SmallEnemy>();
			
			//stop the movement when he is getting hit
			mySmall.agent.speed = 0;

			if (lr <= 50)
				mySmall.GetComponent<Animator>().Play("Attack_Right");
			else
				mySmall.GetComponent<Animator>().Play("Attack_Left");
		}

        enemyHealth -= amount;

        //Debug.Log(gameObject.tag + " Took " + amount + " Damage");

        //Check death
        if (enemyHealth <= 0.0f)
        {
			isDead = true;
		
            if (this.tag == "SmallEnemy")
            {
				SmallEnemy mySmall = this.GetComponent<SmallEnemy>();
                if (mySmall && mySmall.myController)
                    mySmall.myController.RemoveSmall(this.gameObject);

				if(defCon != null && defIsSet == false)
				{
					defCon.GetComponent<DefConManagerScript>().ChangeCurrentEnemyCost(-smallCost);
					defIsSet = true;
				}

				mySmall.GetComponent<Animator>().Play("Death");
				
				StartCoroutine(WaitForAnimation(1.20f));
            }
			else if (this.tag == "Enemy")
			{
				if(defCon != null && defIsSet == false)
				{
					defCon.GetComponent<DefConManagerScript>().ChangeCurrentEnemyCost(-mediumCost);
					defIsSet = true;
				}
				MediumEnemy myMed = this.GetComponent<MediumEnemy>();
				myMed.GetComponent<Animator>().Play("monster2Die");
				
				StartCoroutine(WaitForAnimation(1.20f));
			}
			else if (this.tag == "BigEnemy")
			{
				if(defCon != null && defIsSet == false)
				{
					defCon.GetComponent<DefConManagerScript>().ChangeCurrentEnemyCost(-bigCost);
					defIsSet = true;
				}

				BigEnemy myBig = this.GetComponent<BigEnemy>();
				myBig.GetComponent<Animator>().Play("dead");

				StartCoroutine(WaitForAnimation(3.9f));
			}
			
			 if (deathEffect)
            {
                Instantiate(deathEffect, transform.position, Quaternion.identity);
            }

			//Drop our scrap
			GameObject scrap = (GameObject)Instantiate(scrapPickup, gameObject.transform.position, Quaternion.identity);
			scrap.GetComponent<ScrapPickup>().ChangeQuantity(scrapAmount);
			
			Destroy(gameObject);
			

            //Debug.Log(gameObject.tag + " Killed");
        }
    }

	public void SetRepelStateOn()
	{
		myState = StateTypes.eRepel;
		if(tag == "SmallEnemy")
		{
			SmallEnemy help = (SmallEnemy)this;
			help.myController.NotifyStateChange(myState, null);
		}
	}

	private IEnumerator WaitForAnimation (float myTime )
	{
		yield return new WaitForSeconds(myTime);

	}
	
}


