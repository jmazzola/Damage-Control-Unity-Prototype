using UnityEngine;
using System.Collections;

public class mainMenuButtonScript : MonoBehaviour
{
    public Camera camera;

    public int gotoLevel;

    Vector3 startScale;

    // Use this for initialization
    void Start()
    {
        startScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (GetComponent<BoxCollider>().Raycast(ray, out hit, 100.0f))
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (gotoLevel == -1)
                    Application.Quit();

                Application.LoadLevel(gotoLevel);
            }
            transform.localScale = startScale * 1.02f;
        }
        else
            transform.localScale = startScale;
    }
}
