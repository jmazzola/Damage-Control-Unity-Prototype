using UnityEngine;
using System.Collections;

public class DebugControls : MonoBehaviour {

    float left;
    float top;
    float right;
    float bot;

    bool isPaused = false;
    Rect pauseRect;

    public Font font;
	// Use this for initialization
	void Start () 
    {
        pauseRect = new Rect(0, 0, Screen.width, Screen.height);
        
        left = Screen.width / 2 - 150;
        top = 100;
        right = Screen.width;
        bot = Screen.height;
	}
	
	// Update is called once per frame
	void Update () {
        //Close the program when you hold shift and press escape
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        if(Application.loadedLevel == 1)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                isPaused = !isPaused;
                if (isPaused == true)
                    Time.timeScale = 0.0f;
                else if (isPaused == false)
                    Time.timeScale = 1.0f;
            }
        }
	}

    void OnGUI()
    {
        if(isPaused)
        {
            GUI.Box(pauseRect, "");
            GUI.skin.label.font = font;
            GUI.skin.label.fontSize = 72;
            GUI.Label(new Rect(left, top, right, bot), "Paused");
            GUI.skin.label.fontSize = 36;
            GUI.Label(new Rect(left - 100, top + 100, right, bot), "Press ESC to unpause.");
        }
    }
}
