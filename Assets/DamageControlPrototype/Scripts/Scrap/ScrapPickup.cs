using UnityEngine;
using System.Collections;

public class ScrapPickup : MonoBehaviour
{
    //What plays when picked up
    public AudioClip pickupSound;
    //How much scrap is earned from picking this up
    public int quantity = 5;
    //How long this piece will live for
    public float lifeTime = 10.0f;
    //How long this piece has lived
    private float elapsedLife = 0.0f;
    //How long it's blinked
    private float blinkTime = 0.0f;
    //How much time before it blinks
    private float blinkThreshold = 2.5f;
    //Minimum needed for silver
    public int silverThreshold = 10;
    //Minimum needed for gold
    public int goldThreshold = 20;

    public int distanceForPickup = 10;
    public float smoot = 2.0f;

    // Use this for initialization
    void Start()
    {
        blinkThreshold = lifeTime * 0.33f;
        ChangeAppearance();
    }

    //Changes the appearance based on the amount of scrap
    void ChangeAppearance()
    {
        //Apply color
        Color newColor;
        //Bronze
        if (quantity < silverThreshold)
        {
            newColor = new Color(0.80391f, 0.498039f, 0.196078f);
        }
        //Silver
        else if (quantity < goldThreshold)
        {
            newColor = new Color(0.75294f, 0.75294f, 0.75294f);
        }
        //Gold
        else
        {
            newColor = new Color(1.0f, 0.8f, 0.0f);
        }
        gameObject.renderer.material.color = newColor;
        gameObject.particleSystem.startColor = newColor;
        gameObject.light.color = newColor;
    }

    //Updates the quantity and appearance of the scrap
    public void ChangeQuantity(int _quantity)
    {
        quantity = _quantity;
        ChangeAppearance();
    }

    // Update is called once per frame
    void Update()
    {
        //Die of old age
        elapsedLife += Time.deltaTime;
        if (elapsedLife >= lifeTime)
            Destroy(gameObject);

        //Dying soon...
        if (lifeTime - elapsedLife < blinkThreshold)
        {
            blinkTime -= Time.deltaTime;
            //Blink
            if (blinkTime <= 0)
            {
                gameObject.renderer.enabled = !gameObject.renderer.enabled;
                blinkTime = (lifeTime - elapsedLife) * 0.08f;
                if (gameObject.renderer.enabled)
                    blinkTime *= 1.5f;
            }
        }
    }

    void FixedUpdate()
    {
        //scrap flying, bitches!! :D's

        PlayerMovement player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();

        //vector to player [destination - source]
        Vector3 toPlayer = player.transform.position - transform.position;

        if (toPlayer.magnitude < distanceForPickup)
        {
            transform.position = Vector3.Slerp(transform.position, player.transform.position, Time.deltaTime * smoot);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        //print("collision with scrap");
        if (other.gameObject.tag == "Player")
        {
            //Make the player play this
            other.gameObject.SendMessage("PickUpChime", pickupSound);
            //Give them our scrap
            GameObject.Find("scrapCountLabel").GetComponent<ScrapManager>().Scrap += quantity;
            //We're done here
            Destroy(gameObject);
        }
    }
}