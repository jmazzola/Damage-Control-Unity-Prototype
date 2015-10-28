using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ProgressMapScript : MonoBehaviour
{
    private EnginesScript engineScript
    {
        get { return GameObject.Find("Engines").GetComponent<EnginesScript>(); }
    }

    public float progress;

    float menuWidth
    {
        get
        {
            float width = GameObject.Find("progressMap").GetComponent<RectTransform>().rect.width;
            width -= (width * 0.1f);

            return width;
        }
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        progress = menuWidth * (engineScript.DistanceTraveled / engineScript.GoalDistance) - (menuWidth / 2.0f);
        Vector2 pos = GameObject.Find("progressMap").transform.position;
        pos.y -= 20.0f;
        pos.x += progress;
        gameObject.transform.position = pos;
    }
}
