using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class notEnoughScrap : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Color c = GetComponent<Text>().color;
        c.a -= 0.01f;
        GetComponent<Text>().color = c;
    }
}
