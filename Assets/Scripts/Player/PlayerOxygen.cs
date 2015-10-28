using UnityEngine;
using System.Collections;

public class PlayerOxygen : MonoBehaviour
{
    public float oxygen = 100.0f;
    public float maxOxygen = 100.0f;

    public float suffocationTimer;
    public float suffocatePerSec = 5.0f;

    PlayerHealth playerHealth;

    void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    public bool IsFull()
    {
        return (oxygen == maxOxygen);
    }

    void Update()
    {
        if (oxygen <= 0.0f)
        {
            suffocationTimer += Time.deltaTime;

            if (playerHealth != null)
            {
                if (suffocationTimer >= 1.0f)
                {
                    playerHealth.TakeDamage(suffocatePerSec, PlayerDamageType.Suffocate);
                    suffocationTimer = 0.0f;
                }
            }
            else
            {
                Debug.LogError("PlayerHealth is null. Unable to suffocate player.");
            }
        }
    }
}
