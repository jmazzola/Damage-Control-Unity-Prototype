using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using Random = System.Random;

public class mainMenuHoverScript : MonoBehaviour
{
    float startY;
    public float hoverAmmount;

    static Random random = new Random();

    // Use this for initialization
    void Start()
    {
        startY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.realtimeSinceStartup;
        float y_offset = Mathf.Sin(dt);
        Vector3 position = transform.position;

        position.y = startY + y_offset * hoverAmmount;

        transform.position = position;

        Color c = GameObject.Find("titleLogo").GetComponent<Image>().color;
        c.a = random.Next(100) / 100.0f;
    }
}
