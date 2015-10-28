using UnityEngine;
using System.Collections;

public class spaceScript : MonoBehaviour
{
    public float speed;
    private float progressPrev;

    // Use this for initialization
    void Start()
    {
        GameObject ship = GameObject.Find("shipIcon");
        progressPrev = ship.GetComponent<ProgressMapScript>().progress;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject ship = GameObject.Find("shipIcon");
        float progress = ship.GetComponent<ProgressMapScript>().progress;
        GetComponent<MeshRenderer>().material.mainTextureOffset += Vector2.up * speed * Time.deltaTime * (progress - progressPrev);
        progressPrev = progress;
    }
}
