using UnityEngine;
using System.Collections;

public class SimpleBullet : MonoBehaviour
{

    public float speed = 10;
    public float lifeTime = 5.0f;
    public float dist = 10000;

    float spawnTime = 0.0f;
    Transform tr;

    // Use this for initialization
    void Start()
    {
        tr = transform;
        spawnTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        tr.position += tr.forward * speed * Time.deltaTime;
        dist -= speed * Time.deltaTime;
        if ((Time.time > spawnTime + lifeTime) || dist < 0)
        {
            GameObject impactFx = GameObject.Find("impactEffect");
            impactFx.transform.position = tr.position;
            impactFx.GetComponent<ParticleSystem>().Play();

            Destroy(gameObject);
        }
    }
}