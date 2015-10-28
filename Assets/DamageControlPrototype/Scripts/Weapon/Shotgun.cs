using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Shotgun : Weapon
{
    public int pellets;

    void Start()
    {
        pellets = 12;

        magCurrent = magSize = 5;
        fireRate = .85f;
        damage = 15.0f;
        spread = 10.0f;
        reloadSpeed = 3.0f;
    }

    new void OnEnable()
    {
        //ammo, name, image
        HUDElement1.GetComponent<Text>().text = magSize.ToString();
        HUDElement2.GetComponent<Text>().text = "Shotgun";
        HUDElement3.GetComponent<Image>().sprite = GameObject.Find("shotgun").GetComponent<SpriteRenderer>().sprite;

        base.OnEnable();
    }

    override public void Shoot()
    {
        if (magCurrent <= 0)
            return;

        if (Time.time > lastFireTime + 1 / fireRate)
        {
            barrel_effect.Play();//.enableEmmision = true;
            barrel_sound.PlayOneShot(barrel_sound.clip);

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

            for (int i = 0; i < pellets; i++)
            {

                var coneRandomRotation = Quaternion.Euler(Random.Range(-spread, spread), Random.Range(-spread, spread), 0);
                var go = Instantiate(bullet_tier_x, gun_barrel.transform.position, transform.rotation * coneRandomRotation) as GameObject;
                SimpleBullet bullet = go.GetComponent<SimpleBullet>();

                RaycastHit shot = new RaycastHit();
                Physics.Raycast(transform.position, new Vector3(coneRandomRotation.x + transform.forward.x, coneRandomRotation.y + transform.forward.y, transform.forward.z), out shot);

                if (shot.transform)
                {
                    EnemyBehaviourScript enemy = shot.collider.GetComponent<EnemyBehaviourScript>();
                    if (enemy)
                        enemy.TakeDamage(damage);

                    bullet.dist = shot.distance;
                }
                else
                {
                    bullet.dist = 10000.0f;
                }
            }

            magCurrent--;
            lastFireTime = Time.time;
        }
        else
        {
            barrel_effect.Stop();//.enableEmission = false;
        }
    }
}