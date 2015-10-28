using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Pistol : Weapon
{
    void Start()
    {
        magCurrent = magSize = 7;
        fireRate = 1.5f;
        damage = 8.0f;
        spread = 1.5f;
    }

    new void OnEnable()
    {
        //ammo, name, image
        HUDElement1.GetComponent<Text>().text = "INF";
        HUDElement2.GetComponent<Text>().text = "Pistol";
        HUDElement3.GetComponent<Image>().sprite = GameObject.Find("pistol").GetComponent<SpriteRenderer>().sprite;

        base.OnEnable();
    }
}
