using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class screenOverlayEffect : MonoBehaviour
{
    Image image;

    // the ammout that the overlay while dissapear per frame
    public float fade = 0.1f;

    // Use this for initialization
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        Color color = image.color;
        color.a -= fade;
        image.color = color;
    }

    public void Show(Color color)
    {
        color.a = 0.55f;
        image.color = color;
    }
}