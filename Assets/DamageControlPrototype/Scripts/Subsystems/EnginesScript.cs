using UnityEngine;
//using System.Collections;

public class EnginesScript : MonoBehaviour
{

    public float StartingHealth = 100f;
    public float MaxSpeed = 2.0f;
    public float SpeedDelta = 1.0f;
    public float GoalDistance = 600f;

    public float CurrentHealth;
    float CurrentSpeed;
    public float DistanceTraveled;
    public float[] WaveStartsAtPercent;

    public bool isDead = false;
    AudioSource ambience;
    GameObject[] Spawners;
    MonsterSpawner[] SpawnScriptRef;
    bool[] didWeBlowIt;

    public bool downPlayed = false;
    public bool upPlayed = true;

    // Rects for the Heathbars / text
    //    Rect DBarPosition;
    //    Rect InfoPosition;
    //    Rect WinnerPosition;

    // Use this for initialization
    void Start()
    {
        CurrentHealth = StartingHealth;
        CurrentSpeed = 0;
        DistanceTraveled = 0;

        ambience = GetComponent<AudioSource>();

        Spawners = GameObject.FindGameObjectsWithTag("WaveSpawners");
        SpawnScriptRef = new MonsterSpawner[Spawners.Length];
        didWeBlowIt = new bool[WaveStartsAtPercent.Length];
        for (int i = 0; i < Spawners.Length; i++)
        {
            SpawnScriptRef[i] = Spawners[i].GetComponent<MonsterSpawner>();
        }
        for (int i = 0; i < didWeBlowIt.Length; i++)
        {
            didWeBlowIt[i] = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

        float Percentage = 0;
        float TraveledPercent = DistanceTraveled / GoalDistance;

        for (int i = 0; i < WaveStartsAtPercent.Length; i++)
        {
            Percentage = WaveStartsAtPercent[i] / 100;
            if (TraveledPercent >= Percentage)
            {
                if (didWeBlowIt[i] == false)
                {
                    for (int k = 0; k < Spawners.Length; k++)
                    {
                        didWeBlowIt[i] = true;
                        SpawnScriptRef[k].ActivateMe();
                        //SpawnScriptRef[k].DeactivateMe();
                    }
                }
            }
        }

        // if the ship manages to reach the goal
        if (DistanceTraveled >= GoalDistance)
        {
            DistanceTraveled = GoalDistance;
            CurrentSpeed = 0;
            // We've won do so stuff.
            Application.LoadLevel(3);
        }

        if (isDead)
        {
            if (downPlayed == false)
            {
                downPlayed = true;
                upPlayed = false;
                GameObject.Find("System Down").GetComponent<AudioSource>().Play();
            }
            if (CurrentSpeed > 0f)
                CurrentSpeed -= SpeedDelta * Time.deltaTime;

            if (CurrentSpeed < 0f)
                CurrentSpeed = 0;

            //Halt ambience
            if (ambience && ambience.isPlaying)
                ambience.Pause();
        }
        else
        {

            if (upPlayed == false)
            {
                upPlayed = true;
                downPlayed = false;
                GameObject.Find("System Up").GetComponent<AudioSource>().Play();
            }

            if (CurrentSpeed < MaxSpeed)
                CurrentSpeed += SpeedDelta * Time.deltaTime;

            if (CurrentSpeed > MaxSpeed)
                CurrentSpeed = MaxSpeed;

            //Play ambience
            if (ambience && !ambience.isPlaying)
                ambience.Play();
        }

        DistanceTraveled += CurrentSpeed * Time.deltaTime;
    }

    //    void OnGUI()
    //    {
    //        //GUI.color = Color.white;
    //        //GUI.HorizontalScrollbar(DBarPosition, 0, DistanceTraveled, 0, GoalDistance);
    //
    //        //GUI.contentColor = Color.white;
    //        //GUI.skin.label.fontSize = 16;
    //        //GUI.Label(InfoPosition, "Distance: " + (int)DistanceTraveled + "/" + (int)GoalDistance);
    //
    //        if (Winner)
    //        {
    //            GUI.skin.label.fontSize = 200;
    //            GUI.contentColor = Color.white;
    //            GUI.Label(WinnerPosition, "You Win"); //displays health in text format
    //
    //        }
    //    }

    // Pass in a positive number
    public void TakeDamage(float amount)
    {
        CurrentHealth -= amount;

        if (CurrentHealth <= 0.0f)
        {
            // The system has gone down so it needs to stop working
            isDead = true;
            CurrentHealth = 0;
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