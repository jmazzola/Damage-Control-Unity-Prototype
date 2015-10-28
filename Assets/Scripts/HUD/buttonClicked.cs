using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class buttonClicked : MonoBehaviour 
{
    public int gotoLevelIndex;

	void Start()
	{
		GetComponent<AudioSource>().PlayDelayed(1.5f);
	}

    void Update()
    {
        Rect bounds = GetComponent<RectTransform>().rect;
        bounds.center = gameObject.transform.position;


        Vector2 mousePos = Input.mousePosition;



        if (bounds.Contains(mousePos))
        {
            GetComponent<Image>().color = Color.gray;
            if (Input.GetMouseButtonDown(0))
			{
				GetComponent<AudioSource>().Stop();
                Application.LoadLevel(gotoLevelIndex);
			}
        }
        else
            GetComponent<Image>().color = Color.white;
    }
}
