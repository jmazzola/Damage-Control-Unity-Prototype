using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public delegate void UpgradeNodeClickedEventHandler(int index);

public class upgradeNodeScript : MonoBehaviour
{
    public int index;

    public event UpgradeNodeClickedEventHandler OnClicked;

    public UpgradeStation.WeaponUpgrade upgrade
    {
        get { return UpgradeStation.pistolUpgrades[index]; }
    }


    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePosition = Input.mousePosition;

        Rect r = GetComponent<RectTransform>().rect;
        r.center = gameObject.transform.position;
        if (r.Contains(mousePosition))
        {
            GameObject.Find("itemDesc").GetComponent<Text>().text = upgrade.description + " Cost:" + upgrade.Cost.ToString() + " scrap";
            GameObject.Find("itemDesc").GetComponent<Text>().color = Color.white;
            transform.parent.gameObject.GetComponent<Image>().color = Color.gray;

            // dispatch clicked event
            if (OnClicked != null && Input.GetMouseButton(0))
                OnClicked(index);
        }
        else
            transform.parent.gameObject.GetComponent<Image>().color = Color.white;
    }
}
