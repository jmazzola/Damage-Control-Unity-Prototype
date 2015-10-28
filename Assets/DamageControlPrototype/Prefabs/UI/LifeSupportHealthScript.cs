using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LifeSupportHealthScript : MonoBehaviour
{
    Text healthLabel
    {
        get { return GameObject.Find("lifeSupporHealthtLabel").GetComponent<Text>(); }
    }
    Text nameLabel
    {
        get { return GameObject.Find("lifeSupportLabel").GetComponent<Text>(); }
    }

    Color startColor;

    void Start()
    {
        startColor = GetComponent<Image>().color;

        UIManager.instance.LifeSupportHealthChanged += instance_LifeSupportHealthChanged;
    }

    void instance_LifeSupportHealthChanged(float delta)
    {
        if (delta < 0.0f)
            transform.localScale = new Vector2(2.0f, 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localScale.magnitude > 1.732f)
        {
            Vector2 scale = transform.localScale;
            scale.x -= 0.1f;
            scale.y -= 0.1f;
            transform.localScale = scale;
        }
        else
        {
            Vector2 scale = transform.localScale;
            scale = Vector2.ClampMagnitude(scale, 1.4142f);
            transform.localScale = scale;
        }

        if (transform.localScale.x < 1.0f)
        {
            Vector2 scale = transform.localScale;
            scale.x = 1.0f;
            scale.y = 1.0f;
            transform.localScale = scale;
        }

        if (UIManager.instance.LifeSupportHealth <= 0)
        {
            Color deadColor = Color.white;
            deadColor.a = 0.5f;

            healthLabel.color = deadColor;
            nameLabel.color = deadColor;
            GetComponent<Image>().color = deadColor;
        }
        else
        {
            healthLabel.color = Color.Lerp(Color.white, Color.red, transform.localScale.x - 1.0f);
            nameLabel.color = Color.Lerp(startColor, Color.red, transform.localScale.x - 1.0f);
            GetComponent<Image>().color = Color.Lerp(startColor, Color.red, transform.localScale.x - 1.0f);
        }

        healthLabel.text = System.Math.Round(UIManager.instance.LifeSupportHealth, 2).ToString();
    }
}
