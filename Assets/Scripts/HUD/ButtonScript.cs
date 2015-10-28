using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public delegate void UpgradeButtonClickedEventHandler();

public class ButtonScript : MonoBehaviour
{
    public event UpgradeButtonClickedEventHandler OnClicked;
    public event UpgradeButtonClickedEventHandler MouseHover;


    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePosition = Input.mousePosition;

        Rect r = GetComponent<RectTransform>().rect;
        r.center = gameObject.transform.position + new Vector3(r.width / 2, 0);
        if (r.Contains(mousePosition))
        {
            Color c = Color.gray;
            c.a = GetComponent<Image>().color.a;
            GetComponent<Image>().color = c;
            if (MouseHover != null)
                MouseHover();
            // dispatch clicked event
            if (OnClicked != null && Input.GetMouseButton(0))
                OnClicked();
        }
        else
            GetComponent<Image>().color = Color.white;
    }
}
