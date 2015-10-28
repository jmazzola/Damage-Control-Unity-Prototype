using UnityEngine;
using System.Collections;

public class ArrowScroll : MonoBehaviour {

    public float scrollSpeed = 1;
    public float blinkSpeed = 1;
    public float standbyTime = 0.4f;
    private float currentStandby = 0;
    private Color naturalColor;
    private bool isBrightening = false;
    private float blinkProgress = 1;
	// Use this for initialization
	void Start () {
        naturalColor = renderer.material.color;
	}
	
	// Update is called once per frame
	void Update () {
        //Let the arrow stay at its pole for a moment
        if(currentStandby > 0)
        {
            currentStandby -= Time.deltaTime;
            return;
        }
        //Scroll the arrows
        Vector2 pos = renderer.material.mainTextureOffset;
        pos.y += scrollSpeed * Time.deltaTime;
        renderer.material.mainTextureOffset = pos;

        //Blink the texture
        if (isBrightening)
        {
            //Bring it closer to its brightest point
            renderer.material.color = Color.Lerp(renderer.material.color, naturalColor, Mathf.Min(blinkProgress + Time.deltaTime * blinkSpeed, 1));
            //Bright as can be? Let's wait a bit
            if (renderer.material.color == naturalColor)
            {
                isBrightening = false;
                currentStandby = standbyTime;
            }
        }
        else
        {
            //Bring it closer to the darkest it can be
            renderer.material.color = Color.Lerp(renderer.material.color, Color.black, Mathf.Max(blinkProgress - Time.deltaTime * blinkSpeed, 0));
            //Dark as can be? Let's wait a moment
            if (renderer.material.color == Color.black)
            {
                isBrightening = true;
                currentStandby = standbyTime;
            }

        }

	}
}
