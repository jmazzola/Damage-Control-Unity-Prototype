using UnityEngine;
using System.Collections;

public class SupportDrone : MonoBehaviour
{
    public int PerShotCost = 2;
    public bool isActive;
    public float Damage;
    public float FireRate;
    public float Range;
    public Transform targetToProtect;
    public Transform targetToAimAt;
    public GameObject bulletPrefab;

    public string[] supportedTags;
    Vector3 posOffset;

    Collider[] collisions;

    RaycastHit hitInfo;
    float lastFireTime = 0.0f;

    ScrapManager ScrapMan;
    void Start()
    {
        posOffset = transform.position - targetToProtect.position;
        Damage = 5f;
        FireRate = 2f;
        Range = 15f;
        ScrapMan = GameObject.Find("scrapCountLabel").GetComponent<ScrapManager>();
    }

    void FixedUpdate()
    {
        // moving the drone to the player
        Vector3 targetPos = targetToProtect.position + posOffset;

        Vector3 temp = transform.position;
        temp.x = Vector3.Lerp(transform.position, targetPos, 5 * Time.deltaTime).x;
        temp.z = Vector3.Lerp(transform.position, targetPos, 5 * Time.deltaTime).z;
        transform.position = temp;

        if (Input.GetMouseButtonDown(1))
            isActive = !isActive;

        //renderer.enabled = isActive;

        GetComponent<ParticleSystem>().enableEmission = false;

        // if active then we need to find the closest enemy then shoot at that enemy
        if (isActive)
        {
            // Finding the closest enemy
            FindClosestEnemy();
            transform.LookAt(targetToAimAt, Vector3.up);
            // shooting towards that enemy if we have enough scrap
            hitInfo = new RaycastHit();
            Physics.Raycast(transform.position, transform.forward, out hitInfo);
            DroneShoot();
        }

    }

    void FindClosestEnemy()
    {
        collisions = Physics.OverlapSphere(transform.position, Range);

        float distance = Range;// float.MaxValue;
        foreach (Collider col in collisions)
        {
            foreach (string tag in supportedTags)
            {
                if (col.gameObject.tag == tag)
                {
                    Vector3 toTarget = col.gameObject.transform.position - transform.position;
                    if (distance > toTarget.sqrMagnitude)
                    {
                        distance = toTarget.sqrMagnitude;
                        targetToAimAt = col.gameObject.transform;
                    }
                    //else
                    //    targetToAimAt = null;
                }
            }
        }
    } // end of function

    void DroneShoot()
    {
        if (targetToAimAt)
        {
            if (Time.time > lastFireTime + 1 / FireRate)
            {
                GetComponent<ParticleSystem>().enableEmission = true;
                GetComponent<AudioSource>().Play();

                if (ScrapMan.Scrap < PerShotCost)
                    return;
                else
                    ScrapMan.Scrap -= PerShotCost;

                var go = Instantiate(bulletPrefab, transform.position, transform.rotation) as GameObject;
                SimpleBullet bullet = go.GetComponent<SimpleBullet>();

                lastFireTime = Time.time;

                Debug.DrawLine(transform.position, hitInfo.point);

                if (hitInfo.transform)
                {
                    EnemyBehaviourScript enemy = hitInfo.collider.GetComponent<EnemyBehaviourScript>();
                    if (enemy)
                        enemy.TakeDamage(Damage);

                    bullet.dist = hitInfo.distance;
                }
                else
                {
                    bullet.dist = 10000.0f;
                }
            }
            else
            {
                GetComponent<ParticleSystem>().enableEmission = false;
            }
        }
    }
}