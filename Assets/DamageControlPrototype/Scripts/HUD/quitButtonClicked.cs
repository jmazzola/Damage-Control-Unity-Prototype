using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class quitButtonClicked : MonoBehaviour 
{
	void Start()
	{
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
                Application.Quit();
        }
        else
            GetComponent<Image>().color = Color.white;
	}
}
