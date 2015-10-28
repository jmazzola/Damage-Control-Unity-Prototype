using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerRepairing : MonoBehaviour
{
    public LifeSupportScript lifeSupport;
    public EnginesScript engines;
    public ReactorScript reactor;

    public float engineRepairDist = 50.0f;
    public float lifeSupportRepairDist = 15.0f;
    public float reactorRepairDist = 50.0f;
    public float repairAmt = 20.0f;

    public AudioSource source;

    GameObject engineGO;
    GameObject lifeSupportGO;
    GameObject reactorGO;
    GameObject repairTextGO;

    RepairTrigger engineWorkArea;
    RepairTrigger lifeSupportWorkArea;
    RepairTrigger reactorWorkArea;

    private GameObject weldEffect;

    public void Start()
    {
        source = GetComponent<AudioSource>();
        weldEffect = GameObject.Find("repairParticleEffect");
    }

    void Awake()
    {
        engineGO = GameObject.Find("Engines");
        lifeSupportGO = GameObject.Find("Life Support");
        reactorGO = GameObject.Find("Reactor");
        repairTextGO = GameObject.Find("IsRepairing");

        engineWorkArea = GameObject.Find("EngineWorkArea").GetComponent<RepairTrigger>();
        lifeSupportWorkArea = GameObject.Find("LifeSupportWorkArea").GetComponent<RepairTrigger>();
        reactorWorkArea = GameObject.Find("ReactorWorkArea").GetComponent<RepairTrigger>();

        lifeSupport = lifeSupportGO.GetComponent<LifeSupportScript>();
        engines = engineGO.GetComponent<EnginesScript>();
        reactor = reactorGO.GetComponent<ReactorScript>();

    }

    void Update()
    {
        Vector3 toLife = transform.position - lifeSupportGO.transform.position;
        Vector3 toEngine = transform.position - engineGO.transform.position;
        Vector3 toReactor = transform.position - reactorGO.transform.position;

        if (Input.GetKey(KeyCode.F) && lifeSupportWorkArea.isInTrigger && (lifeSupport.CurrentHealth < lifeSupport.StartingHealth))
        {
            lifeSupport.RepairMe(repairAmt * Time.deltaTime);
            repairTextGO.GetComponent<Text>().text = "Repairing Life Support";


            if (!weldEffect.GetComponent<ParticleSystem>().isPlaying)
            {
                weldEffect.GetComponent<ParticleSystem>().Play();
                weldEffect.GetComponent<weldLightScript>().light.intensity = 3f;
            }

            if (!source.isPlaying)
                source.Play();
        }
        else if (Input.GetKey(KeyCode.F) && engineWorkArea.isInTrigger && (engines.CurrentHealth < engines.StartingHealth))
        {
            engines.RepairMe(repairAmt * Time.deltaTime);
            repairTextGO.GetComponent<Text>().text = "Repairing Engines";


            if (!weldEffect.GetComponent<ParticleSystem>().isPlaying)
            {
                weldEffect.GetComponent<ParticleSystem>().Play();
                weldEffect.GetComponent<weldLightScript>().light.intensity = 3f;
            }

            if (!source.isPlaying)
                source.Play();
        }

        else if (Input.GetKey(KeyCode.F) && reactorWorkArea.isInTrigger && (reactor.CurrentHealth < reactor.StartingHealth))
        {
            reactor.RepairMe(repairAmt * Time.deltaTime);
            repairTextGO.GetComponent<Text>().text = "Repairing Reactor";

            GameObject.Find("Reactor").GetComponent<ReactorScript>().TimeRemaining += Time.deltaTime;

            if (!weldEffect.GetComponent<ParticleSystem>().isPlaying)
            {
                weldEffect.GetComponent<ParticleSystem>().Play();
                weldEffect.GetComponent<weldLightScript>().light.intensity = 3f;
            }

            if (!source.isPlaying)
                source.Play();
        }
        else if (reactorWorkArea.isInTrigger && (reactor.CurrentHealth < reactor.StartingHealth))
        {
            repairTextGO.GetComponent<Text>().text = "Hold F to Repair the Reactor";

            if (weldEffect.GetComponent<ParticleSystem>().isPlaying)
            {
                weldEffect.GetComponent<ParticleSystem>().Stop();
                weldEffect.GetComponent<weldLightScript>().light.intensity = 0f;

            }

            if (source.isPlaying)
                source.Stop();
        }
        else if (engineWorkArea.isInTrigger && (engines.CurrentHealth < engines.StartingHealth))
        {
            repairTextGO.GetComponent<Text>().text = "Hold F to Repair the Engines";
            // weldEffect.SetActive(false);

            if (weldEffect.GetComponent<ParticleSystem>().isPlaying)
            {
                weldEffect.GetComponent<ParticleSystem>().Stop();
                weldEffect.GetComponent<weldLightScript>().light.intensity = 0f;

            }

            if (source.isPlaying)
                source.Stop();
        }
        else if (lifeSupportWorkArea.isInTrigger && (lifeSupport.CurrentHealth < lifeSupport.StartingHealth))
        {
            repairTextGO.GetComponent<Text>().text = "Hold F to Repair the Life Support";
            // weldEffect.SetActive(false);

            if (weldEffect.GetComponent<ParticleSystem>().isPlaying)
            {
                weldEffect.GetComponent<ParticleSystem>().Stop();
                weldEffect.GetComponent<Light>().intensity = 0f;
            }

            if (source.isPlaying)
                source.Stop();
        }
        else
        {
            repairTextGO.GetComponent<Text>().text = string.Empty;
            weldEffect.GetComponent<Light>().intensity = 0f;

            // weldEffect.SetActive(false);

            if (weldEffect.GetComponent<ParticleSystem>().isPlaying)
            {
                weldEffect.GetComponent<ParticleSystem>().Stop();
            }

            if (source.isPlaying)
                source.Stop();
        }

    }
}
