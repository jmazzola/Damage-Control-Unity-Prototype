using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Rifle : Weapon
{
    void Start()
    {
        magCurrent = magSize = 30;
        fireRate = 10.0f;
        damage = 5.0f;
        spread = 3.0f;
        reloadSpeed = 2.0f;
    }

    new void OnEnable()
    {
        //ammo, name, image
        HUDElement1.GetComponent<Text>().text = magSize.ToString();
        HUDElement2.GetComponent<Text>().text = "Rifle";
        HUDElement3.GetComponent<Image>().sprite = GameObject.Find("assultRifle").GetComponent<SpriteRenderer>().sprite;

        base.OnEnable();
    }
}
