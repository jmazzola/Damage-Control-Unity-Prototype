using UnityEngine;
using System.Collections;

public class ReactorHealthDisplay : MonoBehaviour
{
    float Health;
    float MaxHealth;
    public float HealthBarHeight = 5f;
    public float HealthBarLeft = 100f;
    public float HealthBarTop = 1f;
    public float Adjustment = 1.25f;

    private Vector3 WorldPos = new Vector3();
    private Vector3 ViewPos = new Vector3();
    private Transform myTransform;
    private Camera myCamera;

    //assign the camera so we can raycast from it
    //private GameObject myCam;
    private GameObject PlayerRef;

    private ReactorScript ScriptRef;

    // Use this for initialization
    void Awake()
    {
        //myCam = GameObject.Find("Main Camera");
        myTransform = transform;
        myCamera = Camera.main;

        ScriptRef = gameObject.GetComponent<ReactorScript>();
        PlayerRef = GameObject.FindGameObjectWithTag("Player");

        MaxHealth = ScriptRef.StartingHealth;
    }

    // Update is called once per frame
    void OnGUI()
    {
        Health = ScriptRef.getCurrentHealth();
        WorldPos = new Vector3(myTransform.position.x, myTransform.position.y + Adjustment, myTransform.position.z);
        ViewPos = myCamera.WorldToScreenPoint(WorldPos);

        Vector3 ToPlayer = myTransform.position - PlayerRef.transform.position;

        if (renderer.isVisible && ToPlayer.sqrMagnitude < 100)
        {
            if (ScriptRef.GetIfDead() == false)
                GUI.color = Color.green;
            else
                GUI.color = Color.red;

            Rect position = new Rect(ViewPos.x - HealthBarLeft / 2, Screen.height - ViewPos.y - HealthBarTop, 100, 0);
            GUI.HorizontalScrollbar(position, 0, Health, 0, MaxHealth);

            //Debug.DrawRay(myCam.transform.position, myCam.transform.forward * 10, Color.red);

        }
    }
}