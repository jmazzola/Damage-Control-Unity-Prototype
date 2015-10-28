using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using TimeSpan = System.TimeSpan;

public delegate void HealthChangedEventHandler(float delta);

public class UIManager : MonoBehaviour
{
    public int Scrap
    {
        get { return gameObject.GetComponentInChildren<ScrapManager>().Scrap; }

        set { gameObject.GetComponentInChildren<ScrapManager>().Scrap = value; }
    }

    public float Health
    {
        get { return gameObject.GetComponentInChildren<healthBarManager>().Health; }

        set { gameObject.GetComponentInChildren<healthBarManager>().Health = value; }
    }

    public float Oxygen
    {
        get { return gameObject.GetComponentInChildren<OxygenManager>().Oxy; }

        set { gameObject.GetComponentInChildren<OxygenManager>().Oxy = value; }
    }

    public float EngineHealth
    {
        get { return GameObject.Find("Engines").GetComponent<EnginesScript>().CurrentHealth; }

        set { GameObject.Find("Engines").GetComponent<EnginesScript>().CurrentHealth = value; }
    }

    public float LifeSupportHealth
    {
        get { return GameObject.Find("Life Support").GetComponent<LifeSupportScript>().CurrentHealth; }

        set { GameObject.Find("Life Support").GetComponent<LifeSupportScript>().CurrentHealth = value; }
    }

    public float ReactorHealth
    {
        get { return GameObject.Find("Reactor").GetComponent<ReactorScript>().CurrentHealth; }

        set { GameObject.Find("Reactor").GetComponent<ReactorScript>().CurrentHealth = value; }
    }

    public event HealthChangedEventHandler ReactorHealthChanged;
    public event HealthChangedEventHandler LifeSupportHealthChanged;
    public event HealthChangedEventHandler EnginesHealthChanged;

    float reactorHealth_prev;
    float lifeSupportHealth_prev;
    float enginesHealth_prev;

    public float playWarningAfter;

    public screenOverlayEffect overlay
    {
        get { return gameObject.GetComponentInChildren<screenOverlayEffect>(); }
    }

    float lifeSupportWarningCounter;
    float enginesWarningCounter;
    float reactorWarningCounter;
    
    public static UIManager instance;

    public AudioClip lifeSupportWarning;
    public AudioClip engineWarning;
    public AudioClip reactorWarning;
    public AudioClip reactorDown;
    public AudioClip reactorMeltdown;

    AudioSource source
    {
        get { return GetComponent<AudioSource>(); }
    }

    public GameObject upgradeMenu;

    void Awake()
    {
        if (instance != null)
            throw new System.InvalidOperationException("There can be ONLY ONE! instance of UI manager...");
        instance = this;
    }    

    void Start()
    {
        lifeSupportHealth_prev = LifeSupportHealth;
        enginesHealth_prev = EngineHealth;
        reactorHealth_prev = ReactorHealth;

        upgradeMenu.transform.Find("CloseButton").gameObject.GetComponent<ButtonScript>().OnClicked += UIManager_OnClicked;
    }

    void UIManager_OnClicked()
    {
        upgradeMenu.gameObject.SetActive(false);
    }

    void Update()
    {
        if (lifeSupportWarningCounter > float.MinValue)
            lifeSupportWarningCounter -= Time.deltaTime;
        if (enginesWarningCounter > float.MinValue)
            enginesWarningCounter -= Time.deltaTime;
        if (reactorWarningCounter > float.MinValue)
            reactorWarningCounter -= Time.deltaTime;

        // if the life support takes damage, play the warning sound
        if (LifeSupportHealth < lifeSupportHealth_prev)
        {
            LifeSupportHealthChanged(LifeSupportHealth - lifeSupportHealth_prev);

            if (lifeSupportWarningCounter <= 0.0f)
            {
                lifeSupportWarningCounter = playWarningAfter;

                if (!source.isPlaying)
                {
                    source.clip = lifeSupportWarning;
                    source.Play();
                }
            }
        }

        // if the engines take damage, play the warning sound
        if (EngineHealth < enginesHealth_prev)
        {
            EnginesHealthChanged(EngineHealth - enginesHealth_prev);

            if (enginesWarningCounter <= 0.0f)
            {
                enginesWarningCounter = playWarningAfter;

                if (!source.isPlaying)
                {
                    source.clip = engineWarning;
                    source.Play();
                }
            }
        }

        // if the reactor takes damage, play the warning sound
        if (ReactorHealth < reactorHealth_prev)
        {
            ReactorHealthChanged(ReactorHealth - reactorHealth_prev);

            if (reactorWarningCounter <= 0.0f)
            {
                reactorWarningCounter = playWarningAfter;

                if (!source.isPlaying)
                {
                    source.clip = reactorWarning;
                    source.Play();
                }
            }
        }
        
        lifeSupportHealth_prev = LifeSupportHealth;
        enginesHealth_prev = EngineHealth;
        reactorHealth_prev = ReactorHealth;
    }
}