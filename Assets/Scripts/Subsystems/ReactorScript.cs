using UnityEngine;
using System.Collections;

public class ReactorScript : MonoBehaviour {

    public float BoomTimer = 30f;
    public float StartingHealth = 100f;

    public Color deadColor;

    public float CurrentHealth = 100f;
    public float TimeRemaining;

    public bool isDead = false;
    public Font font;
	public bool blown = false;

    Rect InfoPosition;

    public AudioClip reactorDown;
    public AudioClip reactorMeltdown;
	// Use this for initialization
	void Start () {

        CurrentHealth = StartingHealth;
        TimeRemaining = BoomTimer;
        InfoPosition = new Rect((Screen.width / 2) - 300, Screen.height - 50, 400, 50);
	}
	
	// Update is called once per frame
	void Update () {
	
        AudioSource source = UIManager.instance.GetComponent<AudioSource>();
        if(isDead)
        {
			if (GetComponent<AudioSource>() != null && GetComponent<AudioSource>().isPlaying == false)
            {
                GetComponent<AudioSource>().Play();

                if (!blown)
                {
                    source.clip = reactorDown;
                    source.Play();
                }
                else
                    source.Stop();
            }
            if (TimeRemaining <= 9.0f && !source.isPlaying && !blown)
            {
                source.clip = reactorMeltdown;
                source.Play();
            }

            TimeRemaining -= Time.deltaTime;
        }
        else
        {
            TimeRemaining = BoomTimer;
            if (source.isPlaying && source.clip == reactorMeltdown)
            {
                source.Stop();
                source.clip = null;
            }
			if (GetComponent<AudioSource>() != null && GetComponent<AudioSource>().isPlaying == true)
				GetComponent<AudioSource>().Stop();
        }

        if(TimeRemaining <= 0)
        {
            // You lose so do stuff
			TimeRemaining = 0.0f;
			GetComponent<AudioSource>().Stop();
			blown = true;
        }

        //TakeDamage(10 * Time.deltaTime);
	}

    void OnGUI()
    {
        if (isDead)
        {
            GUIDrawRect(InfoPosition, Color.black);
            GUI.contentColor = Color.red;
            GUI.skin.font = font;
            GUI.skin.label.fontSize = 30;
            GUI.Label(InfoPosition, "Self-Destruct: " + TimeRemaining.ToString("0.00"));
        }
    }

    private static Texture2D _staticRectTexture;
    private static GUIStyle _staticRectStyle;

    // Note that this function is only meant to be called from OnGUI() functions.
    public static void GUIDrawRect(Rect position, Color color)
    {
        if (_staticRectTexture == null)
        {
            _staticRectTexture = new Texture2D(1, 1);
        }

        if (_staticRectStyle == null)
        {
            _staticRectStyle = new GUIStyle();
        }

        _staticRectTexture.SetPixel(0, 0, color);
        _staticRectTexture.Apply();

        _staticRectStyle.normal.background = _staticRectTexture;

        GUI.Box(position, GUIContent.none, _staticRectStyle);
    }

    public void TakeDamage(float amount)
    {
        CurrentHealth -= amount;

        if (CurrentHealth <= 0.0f)
        {
            // The system has gone down so it needs to stop working
            isDead = true;
            CurrentHealth = 0.0f;
			GetComponent<AudioSource>().Stop ();
        }
    }

    // Pass in a positive number
    public void RepairMe(float amount)
    {
        CurrentHealth += amount;

        if (CurrentHealth >= StartingHealth)
        {
            isDead = false;
            CurrentHealth = StartingHealth;
            TimeRemaining = BoomTimer;
        }
    }

    public float getCurrentHealth()
    {
        return CurrentHealth;
    }

    public bool GetIfDead()
    {
        return isDead;
    }

}
