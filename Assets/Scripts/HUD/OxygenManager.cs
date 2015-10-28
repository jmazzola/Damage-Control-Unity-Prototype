using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OxygenManager : MonoBehaviour
{
    GameObject oxyLight;
    bool lightVisible
    {
        set { oxyLight.SetActive(value); }
    }

    public float Oxy;
    private float oxyPrev;

    private int oxyLowCounter;

    PlayerOxygen o2;

    // Use this for initialization
    void Start()
    {
        oxyLight = GameObject.Find("OxygenLight");
        o2 = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerOxygen>();
    }

    // Update is called once per frame
    void Update()
    {
        lightVisible = oxyPrev != Oxy;

        oxyPrev = Oxy;
        if (Oxy < 25)
        {
            oxyLowCounter++;
            lightVisible = Mathf.Sin(oxyLowCounter) < 0;
        }
       
        Oxy = o2.oxygen;

        float oxy_inv = Mathf.Lerp(100.0f, 0.0f, Oxy / 100.0f);
        float angle = 20.0f + 33.0f * Mathf.Clamp(oxy_inv / 100.0f, 0.0f, 1.0f);

        Quaternion rot = gameObject.transform.rotation;
        rot.z = angle * Mathf.Deg2Rad;
        gameObject.transform.rotation = rot;
    }
}
