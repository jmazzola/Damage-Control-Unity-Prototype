using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float movementSpeed = 6.0f;
    public float turnSpeed;
    public float OxygenUse = 10.0f;

    public bool isSprinting = false;
    public bool recharge = false;

    public AudioClip[] footSteps;

    float stepTimer = 0;
    float chimeTimer = 0;

    Vector3 movement;

    public Vector3 targetPoint;

    public GameObject pistol;
    public GameObject rifle;
    public GameObject shotgun;
	GameObject mainCamera;

    private PlayerOxygen OxygenRef;


    void Awake()
    {
        OxygenRef = GetComponent<PlayerOxygen>();
		mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    void FixedUpdate()
    {
        //Time to the next step
        stepTimer -= Time.deltaTime;
        chimeTimer -= Time.deltaTime;
        if (isSprinting)
            stepTimer -= Time.deltaTime * 0.5f;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Move(h, v);
        Turn();

    }

  void Move(float h, float v)
    {
      if (GameObject.Find("UpgradeMenu"))
            return;

        if (Input.GetKey(KeyCode.LeftShift) && OxygenRef.oxygen > 50)
        {
            if (recharge == false)
            {
                isSprinting = true;
                GetComponent<TrailRenderer>().enabled = true;
                if (h != 0 || v != 0)
                    OxygenRef.oxygen -= OxygenUse * Time.deltaTime;
            }
        }
        else
        {
            if (OxygenRef.oxygen < 60)
                recharge = true;
            isSprinting = false;
            GetComponent<TrailRenderer>().enabled = false;
        }


        if (OxygenRef.oxygen > 60)
            recharge = false;

        movement.Set(h, 0, v);

        //Make footstep sounds
        if (movement.sqrMagnitude > 0 && stepTimer <= 0)
        {
            stepTimer = 0.3f;
            GetComponent<AudioSource>().PlayOneShot(footSteps[Random.Range(0, footSteps.Length - 1)]);
        }

        movement = movement.normalized * movementSpeed * Time.deltaTime;

        if (isSprinting)
            movement *= 2.0f;


        gameObject.rigidbody.MovePosition(movement + transform.position);
    }
    void Turn()
    {
        if (GameObject.Find("UpgradeMenu"))
            return;

        Plane playerPlane = new Plane(Vector3.up, transform.position);
		//if (Camera.main != null && Camera.main.enabled == true)
		Ray ray = mainCamera.camera.ScreenPointToRay(Input.mousePosition);

        float hitdist = 0.0f;
        if (playerPlane.Raycast(ray, out hitdist))
        {
            targetPoint = ray.GetPoint(hitdist);

            Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }
    }

    public void PickUpChime(AudioClip chime)
    {
        if (chimeTimer <= 0)
        {
            chimeTimer = 0.08f;
            GetComponent<AudioSource>().PlayOneShot(chime);
        }
    }

}
