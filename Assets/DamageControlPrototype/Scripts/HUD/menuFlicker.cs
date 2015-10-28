using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using Random = System.Random;

public class menuFlicker : MonoBehaviour
{
    /// <summary>
    /// A value from 0 to 100 for how much the menu should flicker
    /// </summary>
    public int Intensity;

    Image image
    {
        get { return GetComponent<Image>(); }
    }

    static Random random = new Random();

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Color c = image.color;
        c.a = 0.5f + random.Next((int)Mathf.Clamp(Intensity, 0, 100)) / 100.0f;
        image.color = c;

        if (Input.GetKeyDown(KeyCode.F))
        {
            gameObject.SetActive(false);
        }
    }
}
