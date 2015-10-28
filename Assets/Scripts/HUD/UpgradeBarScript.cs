using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class UpgradeBarScript : MonoBehaviour
{
    public GameObject nodePrefab;

    public Color nodeColor;

    public int level;
    public int type;

    List<GameObject> nodes;

    public event UpgradeNodeClickedEventHandler OnClicked;
    UpgradeStation.WeaponUpgrade[] upgrades
    {
        get
        {
            switch (type)
            {
                case 0:
                    return UpgradeStation.pistolUpgrades;
                case 1:
                    return UpgradeStation.assultRifleUpgrades;
                case 2:
                    return UpgradeStation.shotgunUpgrades;
            }

            return null;
        }
    }

    // Use this for initialization
    void Start()
    {
        nodes = new List<GameObject>();


        for (int i = 0; i < 7; i++)
        {
            GameObject node = Instantiate(nodePrefab) as GameObject;
            GameObject child = node.transform.Find("UpgradeNodeFront").gameObject;
            child.SetActive(true);
            child.GetComponent<Image>().color = nodeColor;
            child.GetComponent<upgradeNodeScript>().index = i;

            child.GetComponent<upgradeNodeScript>().OnClicked += UpgradeBarScript_OnClicked;

            Vector2 position = Vector2.zero;

            position.x = i * (nodePrefab.GetComponent<Image>().rectTransform.rect.width + 10);
            position.x += GetComponent<Image>().rectTransform.rect.width / 2.0f;

            position.y = transform.position.y;
            node.transform.position = position;
            node.transform.SetParent(transform);

            nodes.Add(node);
        }
    }

    void UpgradeBarScript_OnClicked(int index)
    {
        if (index != level)
        {

            if (UIManager.instance.Scrap < upgrades[index].Cost)
            {
                GameObject.Find("notEnoughScrap").GetComponent<Text>().color = Color.red;
                return;
            }
               
            return;
        }
        
        level = index + 1;
        if (OnClicked != null)
            OnClicked(index);
    }

    // Update is called once per frame
    void Update()
    {
        if (name == "rifleBar")
            if (GameObject.Find("Upgrade Station").GetComponent<UpgradeStation>().isARUnlocked)
            {
                for (int i = 0; i < nodes.Count; i++)
                    nodes[i].SetActive(true);
            }
        if (name == "shotgunBar")
            if (GameObject.Find("Upgrade Station").GetComponent<UpgradeStation>().isShotgunUnlocked)
            {
                for (int i = 0; i < nodes.Count; i++)
                    nodes[i].SetActive(true);
            }
        for (int i = 0; i < nodes.Count; i++)
        {
            GameObject child = nodes[i].transform.Find("UpgradeNodeFront").gameObject;
            GameObject node= nodes[i];

            Color c = child.GetComponent<Image>().color;
            if (name == "rifleBar" && !GameObject.Find("Upgrade Station").GetComponent<UpgradeStation>().isARUnlocked)
            {
                nodes[i].SetActive(false);
                continue;
            } 
            if (name == "shotgunBar" && !GameObject.Find("Upgrade Station").GetComponent<UpgradeStation>().isShotgunUnlocked)
            {
                nodes[i].SetActive(false);
                continue;
            }
            if (UIManager.instance.Scrap < upgrades[i].Cost)
                continue;
            if (i < level)
                c.a = 1.0f;
            else
                c.a = 0;
            child.GetComponent<Image>().color = c;
        }
    }
}
