using UnityEngine;
using System.Collections;

public class RepelNade : MonoBehaviour
{
    public bool isActive;
    public float maxSizeTime = 5.0f;
    public float sizeInc = 0.2f;
    public float sizeDec = 2.0f;
    public float maxRadius = 2.7f;
    public float repelTime = 10.0f;

    public float radius = 50.0f;
    public int scrapCost = 200;

    float timer = 0.0f;
    bool hasSpawned;

    GameObject[] medEnemies;
    GameObject[] smallEnemies;
    GameObject[] bigEnemies;

    ScrapManager ScrapMan;

    void Update()
    {
        medEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        smallEnemies = GameObject.FindGameObjectsWithTag("SmallEnemy");
        bigEnemies = GameObject.FindGameObjectsWithTag("BigEnemy");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isActive = !isActive;
            ScrapMan = GameObject.Find("scrapCountLabel").GetComponent<ScrapManager>();

            if (ScrapMan.Scrap < scrapCost)
                return;
            else
                ScrapMan.Scrap -= scrapCost;
        }

        if (isActive)
        {
            hasSpawned = true;
            if (timer <= maxSizeTime)
            {
                radius += sizeInc * Time.deltaTime;

                if (radius >= maxRadius)
                {
                    timer += Time.deltaTime;
                }
            }

            if (timer >= maxSizeTime)
            {
                radius -= sizeDec * Time.deltaTime;

                if (radius < 0)
                {
                    radius = 0;
                    isActive = false;
                    timer = 0.0f;
                }
            }

            foreach (GameObject enemy in medEnemies)
            {
                Vector3 toRepel = transform.position - enemy.transform.position;
                Debug.DrawLine(transform.position, enemy.transform.position);

                if(toRepel.sqrMagnitude <= radius * radius)
                {
                    enemy.GetComponent<MediumEnemy>().SetRepelStateOn();
                }
            }

            foreach (GameObject enemy in smallEnemies)
            {
                Vector3 toRepel = transform.position - enemy.transform.position;
                Debug.DrawLine(transform.position, enemy.transform.position);

                if (toRepel.sqrMagnitude <= radius * radius)
                {
                    enemy.GetComponent<SmallEnemy>().SetRepelStateOn();
                }
            }

            foreach (GameObject enemy in bigEnemies)
            {
                Vector3 toRepel = transform.position - enemy.transform.position;
                Debug.DrawLine(transform.position, enemy.transform.position);

                if (toRepel.sqrMagnitude <= radius * radius)
                {
                    enemy.GetComponent<BigEnemy>().SetRepelStateOn();
                }
            }
        }
        else
        {
            if (hasSpawned && timer == 0)
                Destroy(gameObject);
        }
    }
}
