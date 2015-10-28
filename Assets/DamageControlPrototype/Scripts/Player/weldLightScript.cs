using UnityEngine;
using System.Collections;

using Random = System.Random;

public class weldLightScript : MonoBehaviour
{
    public new Light light
    {
        get { return GetComponent<Light>(); }
    }

    static Random rand = new Random();

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        light.range = (rand.Next(100) / 100.0f) * 5.0f;
        if (light.range < 2.5f)
            light.range = 2.5f;
    }
}
