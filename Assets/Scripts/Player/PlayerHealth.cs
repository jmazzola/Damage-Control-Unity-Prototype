using UnityEngine;
using System.Collections;

public enum PlayerDamageType
{
    Injure,
    Suffocate,
}

public class PlayerHealth : MonoBehaviour 
{
    public float currentHealth = 100.0f;
    public float maxHealth = 100.0f;
    public bool alive;

    public AudioClip[] damageSounds;
    private float soundTimer = 0;

    PlayerMovement playerMovement;

    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    public void Update()
    {
        soundTimer -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        alive = (currentHealth > 0.0f);

        if (currentHealth < 0.0f)
            currentHealth = 0.0f;
    }

    public void TakeDamage(float dmg, PlayerDamageType type)
    {
        if (alive)
        {
            currentHealth -= dmg;
            if (type == PlayerDamageType.Injure)
                UIManager.instance.overlay.Show(Color.red);
            else if (type == PlayerDamageType.Suffocate)
                UIManager.instance.overlay.Show(Color.blue);

            //Play damage sound
            if(soundTimer <= 0)
            {
                soundTimer = 0.8f;
                GetComponent<AudioSource>().PlayOneShot(damageSounds[Random.Range(0, damageSounds.Length - 1)]);
            }
        }
        else
            Die();        
    }


    void Die()
    {
       // print("DEAD.");

        //Go to the game over screen
        Application.LoadLevel(2);
    }
}
