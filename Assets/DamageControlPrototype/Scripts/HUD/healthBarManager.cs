using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class healthBarManager : MonoBehaviour
{

    public float Health;

    PlayerHealth ph;
    // Use this for initialization
    void Start()
    {
        ph = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        Health = ph.currentHealth;

        //float clampHealth = Mathf.Clamp (Health / 100.0f, 0.0f, 1.0f);
        //Image image = GetComponent<Image> ();
        //Sprite sprite = image.sprite;
        //Vector4 border = sprite.border;
        //border.y = Mathf.Lerp (100.0f, 0.0f, clampHealth);
        //image.sprite = Sprite.Create(sprite.texture, new Rect(border.x, border.y, border.z, border.w), new Vector2(0.5f, 0.5f));
        Vector3 scale = gameObject.transform.localScale;
        scale.y = Mathf.Clamp(Health / ph.maxHealth, 0.0f, 1.0f);
        gameObject.transform.localScale = scale;


    }
}
