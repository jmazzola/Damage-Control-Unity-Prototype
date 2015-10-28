using UnityEngine;
using System.Collections;

public class LifeSupportScript : MonoBehaviour
{

    public float StartingHealth = 100f;
    public float OxygenDelta = 1f;
    public float HealthDelta = 1f;
    public float CurrentHealth;

    private GameObject PlayerRef;
    private PlayerOxygen OxyScriptRef;
    private PlayerHealth HealthScriptRef;

    public bool isDead = false;
    public bool isPlayed = false;

    // Use this for initialization
    void Start()
    {
        CurrentHealth = StartingHealth;
        PlayerRef = GameObject.FindGameObjectWithTag("Player");
        OxyScriptRef = PlayerRef.GetComponent<PlayerOxygen>();
        HealthScriptRef = PlayerRef.GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        if (OxyScriptRef.oxygen > OxyScriptRef.maxOxygen)
            OxyScriptRef.oxygen = OxyScriptRef.maxOxygen;

        if (!OxyScriptRef.IsFull() && isDead == false)
            OxyScriptRef.oxygen += OxygenDelta * Time.deltaTime;

        if (isDead)
        {
            if (isPlayed == false)
            {
                isPlayed = true;
                GetComponent<AudioSource>().Play();
            }

            if (OxyScriptRef.oxygen > 0)
                OxyScriptRef.oxygen -= OxygenDelta * Time.deltaTime;

            else if (OxyScriptRef.oxygen < 0)
                OxyScriptRef.oxygen = 0;
        }
        else
        {

            if (isPlayed == true)
            {
                isPlayed = false;
                GetComponent<AudioSource>().Stop();
            }

            if (HealthScriptRef.currentHealth < HealthScriptRef.maxHealth)
                HealthScriptRef.currentHealth += HealthDelta * Time.deltaTime;
            else if (HealthScriptRef.currentHealth > HealthScriptRef.maxHealth)
                HealthScriptRef.currentHealth = HealthScriptRef.maxHealth;
        }
    }

    public void TakeDamage(float amount)
    {
        CurrentHealth -= amount;

        if (CurrentHealth <= 0.0f)
        {
            // The system has gone down so it needs to stop working
            isDead = true;
            CurrentHealth = 0;
        }
    }

    // Pass in a positive number
    public void RepairMe(float amount)
    {
        CurrentHealth += amount;

        if (CurrentHealth >= StartingHealth)
        {
            isDead = false;
            CurrentHealth = StartingHealth;
        }
    }

    // Get the current health
    public float getCurrentHealth()
    {
        return CurrentHealth;
    }

    public bool GetIfDead()
    {
        return isDead;
    }
}