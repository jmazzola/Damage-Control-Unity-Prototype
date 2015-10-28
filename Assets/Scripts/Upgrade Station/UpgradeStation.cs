using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class UpgradeStation : MonoBehaviour
{
    public enum UpgradeType
    {
        FireRate = 0,
        Damage = 1,
        Accuracy = 2,
    }

    public class WeaponUpgrade
    {
        public UpgradeType type;

        public string description;

        public float ammount;

        public int Cost;

        public WeaponUpgrade(UpgradeType type, string description, float ammount, int cost)
        {
            this.type = type;
            this.description = description;
            this.ammount = ammount;
            this.Cost = cost;
        }
    }

    public static WeaponUpgrade[] pistolUpgrades;
    public static WeaponUpgrade[] assultRifleUpgrades;
    public static WeaponUpgrade[] shotgunUpgrades;

    GameObject player;
    public GameObject goText;
    public Text upgradeText;
    public Text descText;

    public GameObject upgradeMenu;
    public GameObject pistol;
    public GameObject shotgun;
    public GameObject rifle;

    public AudioClip upgradeSound;

    public bool isARUnlocked;
    public bool isShotgunUnlocked;

    public int shotgunCost = 100;
    public int assaultRifleCost = 100;

    GameObject itemDesc
    {
        get { return upgradeMenu.transform.Find("itemDesc").gameObject; }
    }

    void Awake()
    {
        pistolUpgrades = new WeaponUpgrade[]
        {
            new WeaponUpgrade(UpgradeType.FireRate, "Fire Rate 1", 1.0f, 15),
            new WeaponUpgrade(UpgradeType.FireRate, "Fire Rate 2", 1.0f, 30),
            new WeaponUpgrade(UpgradeType.FireRate, "Fire Rate 3", 1.0f, 60),
            new WeaponUpgrade(UpgradeType.Damage, "Damage 1", 5.0f, 100),
            new WeaponUpgrade(UpgradeType.Damage, "Damage 2", 10.0f, 120),
            new WeaponUpgrade(UpgradeType.Accuracy, "Fire Rate 4", 1.0f, 130),
            new WeaponUpgrade(UpgradeType.Accuracy, "Accuracy 1" , 4.0f, 150),
        };

        //THESE DON'T DO DICK
        shotgunUpgrades = new WeaponUpgrade[]
        {
            new WeaponUpgrade(UpgradeType.FireRate, "Fire Rate 1", 1.0f, 30),
            new WeaponUpgrade(UpgradeType.FireRate, "Fire Rate 2", 1.0f, 60),
            new WeaponUpgrade(UpgradeType.FireRate, "Fire Rate 3", 1.0f, 100),
            new WeaponUpgrade(UpgradeType.Damage, "Damage 1", 5.0f, 120),
            new WeaponUpgrade(UpgradeType.Damage, "Damage 2", 10.0f, 130),
            new WeaponUpgrade(UpgradeType.Accuracy, "Fire Rate 4", 1.0f, 150),
            new WeaponUpgrade(UpgradeType.Accuracy, "Accuracy 1" , 4.0f, 170),
        };

        //THESE DON'T DO DICK
        assultRifleUpgrades = new WeaponUpgrade[]
        {
            new WeaponUpgrade(UpgradeType.FireRate, "Fire Rate 1", 1.0f, 30),
            new WeaponUpgrade(UpgradeType.FireRate, "Fire Rate 2", 1.0f, 60),
            new WeaponUpgrade(UpgradeType.FireRate, "Fire Rate 3", 1.0f, 100),
            new WeaponUpgrade(UpgradeType.Damage, "Damage 1", 5.0f, 120),
            new WeaponUpgrade(UpgradeType.Damage, "Damage 2", 10.0f, 130),
            new WeaponUpgrade(UpgradeType.Accuracy, "Fire Rate 4", 1.0f, 150),
            new WeaponUpgrade(UpgradeType.Accuracy, "Accuracy 1" , 4.0f, 170),
        };

        player = GameObject.FindGameObjectWithTag("Player");
        goText = GameObject.Find("UpgradeText");
        upgradeText = GameObject.Find("Instruct").GetComponent<Text>();
        descText = GameObject.Find("Desc").GetComponent<Text>();

        upgradeMenu.SetActive(true);

        GameObject.Find("pistolBar").gameObject.GetComponent<UpgradeBarScript>().OnClicked += UpgradeStation_OnClicked;
        GameObject.Find("rifleBar").gameObject.GetComponent<UpgradeBarScript>().OnClicked += rifleBar_onClicked;
        GameObject.Find("shotgunBar").gameObject.GetComponent<UpgradeBarScript>().OnClicked += shotgunBar_onClicked;

        GameObject.Find("assultRifleButton").gameObject.GetComponent<ButtonScript>().OnClicked += rifleButton_onClicked;
        GameObject.Find("shotgunButton").gameObject.GetComponent<ButtonScript>().OnClicked += shotgunButton_onClicked;

        GameObject.Find("shotgunButton").gameObject.GetComponent<ButtonScript>().MouseHover += shotgunButton_MouseHover;
        GameObject.Find("assultRifleButton").gameObject.GetComponent<ButtonScript>().MouseHover += assaultRifleButton_MouseHover;
        upgradeMenu.SetActive(false);
    }

    void Start()
    {

    }

    void shotgunButton_MouseHover()
    {
        itemDesc.GetComponent<Text>().text = shotgunCost.ToString() + " scrap to buy Shotgun";
    }

    void assaultRifleButton_MouseHover()
    {

        itemDesc.GetComponent<Text>().text = assaultRifleCost.ToString() + " scrap to buy Assault Rifle";
    }

    private void shotgunButton_onClicked()
    {
        if (UIManager.instance.Scrap < shotgunCost || isShotgunUnlocked == true)
            return;

        isShotgunUnlocked = true;
        UIManager.instance.Scrap -= shotgunCost;
    }

    private void rifleButton_onClicked()
    {
		if (UIManager.instance.Scrap < assaultRifleCost || isARUnlocked == true)
            return;

        isARUnlocked = true;
        UIManager.instance.Scrap -= assaultRifleCost;
    }

    void shotgunBar_onClicked(int index)
    {
        if (!isShotgunUnlocked)
            return;

        Upgrade(GameObject.Find("Player").GetComponent<PlayerMovement>().shotgun.GetComponent<Shotgun>(), (UpgradeType)index, shotgunUpgrades[index].ammount);
    }

    void rifleBar_onClicked(int index)
    {
        if (!isARUnlocked)
            return;

        Upgrade(GameObject.Find("Player").GetComponent<PlayerMovement>().rifle.GetComponent<Rifle>(), (UpgradeType)index, assultRifleUpgrades[index].ammount);
    }

    void UpgradeStation_OnClicked(int index)
    {
        Upgrade(GameObject.Find("Player").GetComponent<PlayerMovement>().pistol.GetComponent<Pistol>(), (UpgradeType)index, pistolUpgrades[index].ammount);
    }

    void Update()
    {
        float toUs = Vector3.Magnitude(player.transform.position - transform.position);

        Weapon weapEquipped = null;
        // Figure out what weap is equipped
        if (player.GetComponent<PlayerMovement>().pistol.activeInHierarchy)
            weapEquipped = player.GetComponent<PlayerMovement>().pistol.GetComponent<Pistol>() as Weapon;
        else if (player.GetComponent<PlayerMovement>().rifle.activeInHierarchy)
            weapEquipped = player.GetComponent<PlayerMovement>().rifle.GetComponent<Rifle>() as Weapon;
        else if (player.GetComponent<PlayerMovement>().shotgun.activeInHierarchy)
            weapEquipped = player.GetComponent<PlayerMovement>().shotgun.GetComponent<Shotgun>() as Weapon;

        if (toUs <= 4.5f)
        {
            // upgradeText.text = "Tier: " + weapEquipped.tier + " / " + weapEquipped.maxTier + "\nPress F to upgrade your weapon | Cost: " + weapEquipped.costToUpgrade + " Scrap";
            if (!upgradeMenu.activeInHierarchy)
                descText.text = "Press \'F\' to upgrade weapons";
            else
                descText.text = string.Empty;
            if (Input.GetKeyDown(KeyCode.F))
                upgradeMenu.SetActive(true);

        }
        else if (toUs >= 4.5f)
        {
            upgradeText.text = string.Empty;
            descText.text = string.Empty;
            upgradeMenu.SetActive(false);
        }
    }

    void Upgrade(Weapon weapon, UpgradeType type, float amtToAdd)
    {
        ScrapManager s = GameObject.Find("scrapCountLabel").GetComponent<ScrapManager>();

        if (s.Scrap >= weapon.costToUpgrade)
        {
            if (weapon.tier < weapon.maxTier)
            {
                s.Scrap -= weapon.costToUpgrade;
                ++weapon.tier;
                //weapon.costToUpgrade += 20;
                //print(weapon.tier);
                string typeOfUpgrade = string.Empty;

                switch (type)
                {
                    case UpgradeType.FireRate:
                        {
                            weapon.fireRate += amtToAdd;
                            typeOfUpgrade = "Firerate";
                            break;
                        }
                    case UpgradeType.Damage:
                        {
                            weapon.damage += amtToAdd;
                            typeOfUpgrade = "Damage";
                            break;
                        }
                    case UpgradeType.Accuracy:
                        {
                            if (weapon.spread > 1.0f)
                            {
                                weapon.spread -= amtToAdd; //subtract
                            }

                            typeOfUpgrade = "Accuracy";
                            break;
                        }
                    default:
                        {
                            typeOfUpgrade = "Unknown UpgradeType";
                            break;
                        }
                }

                upgradeMenu.GetComponent<AudioSource>().clip = upgradeSound;
                upgradeMenu.GetComponent<AudioSource>().Play();

                //  descText.text = typeOfUpgrade + " + " + amtToAdd.ToString().Replace("-", "");
            }

            else
            {
                //descText.text = "Weapon has been fully upgraded!";
            }

        }
        else
        {
            //descText.text = "Insufficient funds";
        }
    }
}