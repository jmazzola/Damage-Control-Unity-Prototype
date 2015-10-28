using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    //////////////////////////////////////////////////
    //
    //PUBLICS for modification/use elsewhere
    //
    //weapon specific crap
    public bool isUnlocked;
    public int magSize, magCurrent;
    public float fireRate;
    public float damage;
    public float spread;
    public float reloadSpeed;

    //what the gun shoots
    public GameObject bullet_Tier_0;
    public GameObject bullet_Tier_1;
    public GameObject bullet_Tier_2;
    public GameObject bullet_Tier_3;
    public GameObject bullet_Tier_4;
    public GameObject bullet_Tier_5;
    public GameObject bullet_Tier_6;
    public GameObject bullet_Tier_7;


    //upgrade material
    public int tier;
    public int maxTier = 7;
    public int costToUpgrade;

    //weapon reload timer
    public float reloadTimer = 0.0f;

    //////////////////////////////////////////////////
    //
    //PROTECTED for inherited objects
    //
    //what the constant raycast hit
    protected RaycastHit hitInfo;

    //the last time fired
    protected float lastFireTime = 0.0f;

    //gun effects
    protected GameObject gun_barrel;
    protected ParticleSystem barrel_effect;
    protected AudioSource barrel_sound;

    //for player weapons
    protected PlayerMovement player;

    //hud stuff
    protected GameObject HUDElement1;   //ammo
    protected GameObject HUDElement2;   //name
    protected GameObject HUDElement3;   //image
    protected GameObject HUDElement4;   //weapon bg

    //////////////////////////////////////////////////
    //
    //PRIVATE for this class only
    //
    private bool firing = false;
    private bool reloading = false;
    private Color reloadColor;

    //private Color red;
    //private Color orange;
    //private Color yellow;
    //private Color green;
    //private Color blue;
    //private Color indigo;
    //private Color violet;
    //
    //private Color tierColor;

    void Awake()
    {
        reloadColor = Color.white;

        isUnlocked = false;

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();

        HUDElement1 = GameObject.Find("WeaponAmmo");
        HUDElement2 = GameObject.Find("WeaponName");
        HUDElement3 = GameObject.Find("WeaponImage");
        HUDElement4 = GameObject.Find("currentWeaponBack");

        //awake, onenable, start << function order

        //catch the player && HUD in awake
        //gun-specific HUD, gun effects
        //then initialize the gun itself.

        //the update function takes care of updating ammo

        //upgrade tier colors
        //red = CreateRGBColor(255f, 0f, 0f, 255f);
        //orange = CreateRGBColor(255f, 127f, 0f, 255f);
        //yellow = CreateRGBColor(255f, 255f, 0f, 255f);
        //green = CreateRGBColor(0f, 255f, 0f, 255f);
        //blue = CreateRGBColor(0f, 0f, 255f, 255f);
        //indigo = CreateRGBColor(75f, 0f, 130f, 255f);
        //violet = CreateRGBColor(143f, 0f, 255f, 255f);
        //
        //tierColor = CreateRGBColor(255f, 255f, 255f, 255f);
    }

    public void OnEnable()
    {
        gun_barrel = GameObject.FindGameObjectWithTag("Barrel");
        barrel_effect = gun_barrel.GetComponent<ParticleSystem>();
        barrel_sound = gun_barrel.GetComponent<AudioSource>();
    }

    void OnDisable()
    {
        if (barrel_effect.isPlaying == true || barrel_effect.IsAlive() == true)
            barrel_effect.Stop();

        if (barrel_sound.isPlaying == true)
            barrel_sound.Stop();

        barrel_sound = null;
        barrel_effect = null;
        gun_barrel = null;
    }

    void FixedUpdate()
    {
        var coneRandomRotation = Quaternion.Euler(Random.Range(-spread, spread), Random.Range(-spread, spread), 0);
        hitInfo = new RaycastHit();
        Physics.Raycast(transform.position, new Vector3(coneRandomRotation.x + transform.forward.x, coneRandomRotation.y + transform.forward.y, transform.forward.z), out hitInfo);
    }

    void Update()
    {
        if (firing == false)
        {
            barrel_effect.Stop();
            WeaponSwitching();
        }

        if (firing == false && Input.GetMouseButton(0))
            firing = true;
        if (firing == true && Input.GetMouseButtonUp(0))
            firing = false;

        if (firing)
        {
            if (!GameObject.Find("UpgradeMenu"))
                Shoot();
        }
        else
        {
            if (gun_barrel)
            {
                if (barrel_effect.isPlaying == true || barrel_effect.IsAlive() == true)
                    barrel_effect.Stop();//.enableEmission = false;
            }
        }
    }

    void LateUpdate()
    {
        if (player.pistol.activeInHierarchy == false)
        {
            HUDElement1.GetComponent<Text>().text = magCurrent.ToString();

            if (reloading)
            {
                Reload();
                return; //may move
            }

            if (magCurrent <= 0 && UIManager.instance.Scrap > magSize)
            {
                reloading = true;
            }
            else if (magCurrent <= 0)
            {
                magCurrent = UIManager.instance.Scrap;
                UIManager.instance.Scrap = 0;
            }
        }

        Vector3 toPlayer = player.transform.position - player.targetPoint;
        if (toPlayer.sqrMagnitude > 3)
        {
            transform.LookAt(player.targetPoint);
        }
    }

    public virtual void Shoot()
    {
        if (magCurrent <= 0 && player.pistol.activeInHierarchy == false)
            return;

        if (gun_barrel == null)
            return;

        if (Time.time > lastFireTime + 1 / fireRate)
        {
            barrel_effect.Play();
            gun_barrel.GetComponent<AudioSource>().PlayOneShot(gun_barrel.GetComponent<AudioSource>().clip);

            GameObject bullet_tier_x = null;
            switch (tier)
            {
                case 1:
                    bullet_tier_x = bullet_Tier_1;
                    break;
                case 2:
                    bullet_tier_x = bullet_Tier_2;
                    break;
                case 3:
                    bullet_tier_x = bullet_Tier_3;
                    break;
                case 4:
                    bullet_tier_x = bullet_Tier_4;
                    break;
                case 5:
                    bullet_tier_x = bullet_Tier_5;
                    break;
                case 6:
                    bullet_tier_x = bullet_Tier_6;
                    break;
                case 7:
                    bullet_tier_x = bullet_Tier_7;
                    break;

                default:
                    bullet_tier_x = bullet_Tier_0;
                    break;
            }

            var coneRandomRotation = Quaternion.Euler(Random.Range(-spread, spread), Random.Range(-spread, spread), 0);
            var go = Instantiate(bullet_tier_x, gun_barrel.transform.position, transform.rotation * coneRandomRotation) as GameObject;
            SimpleBullet bullet = go.GetComponent<SimpleBullet>();

            lastFireTime = Time.time;

            if (hitInfo.transform)
            {
                EnemyBehaviourScript enemy = hitInfo.collider.GetComponent<EnemyBehaviourScript>();
                if (enemy)
                    enemy.TakeDamage(damage);

                bullet.dist = hitInfo.distance;
            }
            else
            {
                bullet.dist = 10000.0f;
            }

            magCurrent--;
        }
        else
        {
            barrel_effect.Stop();//.enableEmission = false;
        }
    }

    void WeaponSwitching()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (!player.pistol.activeInHierarchy)
            {
                player.rifle.SetActive(false);
                player.shotgun.SetActive(false);
                player.pistol.SetActive(true);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (!player.rifle.activeInHierarchy && GameObject.Find("Upgrade Station").GetComponent<UpgradeStation>().isARUnlocked)
            {
                player.pistol.SetActive(false);
                player.shotgun.SetActive(false);
                player.rifle.SetActive(true);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (!player.shotgun.activeInHierarchy && GameObject.Find("Upgrade Station").GetComponent<UpgradeStation>().isShotgunUnlocked)
            {
                player.pistol.SetActive(false);
                player.rifle.SetActive(false);
                player.shotgun.SetActive(true);
            }
        }
    }

    void Reload()
    {
        firing = false;

        if (barrel_effect)
            barrel_effect.Stop();// gun_barrel.GetComponent<ParticleSystem>().enableEmission = false;

        reloadTimer += Time.deltaTime;

        HUDElement1.GetComponent<Text>().text = "Reloading";// magCurrent.ToString("Reloading");
        HUDElement1.GetComponent<Text>().fontStyle = FontStyle.Bold;

        reloadColor.a -= Time.deltaTime;

        if (reloadColor.a < 0.2f)
            reloadColor.a = 1.0f;

        HUDElement1.GetComponent<Text>().color = reloadColor;
        HUDElement1.GetComponent<Text>().fontSize = 64;
        HUDElement2.GetComponent<Text>().color = new Color(1f, 1f, 1f, reloadColor.a);
        HUDElement3.GetComponent<Image>().color = new Color(1f, 1f, 1f, reloadColor.a);
        HUDElement4.GetComponent<Image>().color = new Color(1f, 1f, 1f, reloadColor.a);

        if (reloadTimer >= reloadSpeed)
        {
            magCurrent = magSize;
            UIManager.instance.Scrap -= magSize;
            reloading = false;
            reloadTimer = 0.0f;

            HUDElement1.GetComponent<Text>().text = magCurrent.ToString("0");
            HUDElement1.GetComponent<Text>().fontSize = 84;
            HUDElement1.GetComponent<Text>().color = Color.white;
            HUDElement2.GetComponent<Text>().color = new Color(1f, 1f, 1f, 1f);// = 1.0f;
            HUDElement3.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);// = 1.0f;
            HUDElement4.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);// = 1.0f;
        }
    }

}