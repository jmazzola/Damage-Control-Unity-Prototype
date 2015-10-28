using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Multitexture support has made... I'm sorry...
public class OcclusionZone : MonoBehaviour {

    //The gameobjects that will be made transparent
    public GameObject[] objectsToOcclude;
    //The shader used by each object
    private Shader[][] nativeShader;
    //The transparency at which the original material will be restored
    public float[] minTransparency;
    //The transparency at which we stop fading
    public float[] maxTransparency;
    //The rate per second at which the transparency will change
    public float fadeSpeed = 0.8f;
    //Whether or not we are actively making things transparent
    public bool occluding = false;
    //Whether or not we are transitioning
    private bool transitioning = false;

    private int entries = 0;

    public Shader transparentShader;

	// Use this for initialization
	void Start () 
    {
        //How many objects can we do
        entries = Mathf.Min(objectsToOcclude.Length, minTransparency.Length, maxTransparency.Length);
        //If we don't have what we need, end it all
        if (entries < 1)
        {
            Debug.Log("Occlusion Zone: No valid entries, killing self... Now");
            Destroy(this);
        }
        else if (transparentShader == null)
        {
            Debug.Log("Occlusion Zone: Invalid transparent shader, killing self... Now");
            Destroy(this);
        }
        //Populate the native shader list
        nativeShader = new Shader[entries][];
        for (int i = 0; i < entries; i++)
        {
            //Support multitexturing
            Material[] temp = objectsToOcclude[i].renderer.materials;
            nativeShader[i] = new Shader[temp.Length];
            for (int j = 0; j < temp.Length; j++)
            {
                nativeShader[i][j] = temp[j].shader;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        //The transparency is changing
        if(transitioning)
        {
            //See if it lasts
            transitioning = false;
            //Things are becoming transparent
	        if(occluding)
            {
                for (int i = 0; i < entries; i++)
                {
                    if (objectsToOcclude[i] && objectsToOcclude[i].renderer.material.color.a != minTransparency[i])
                    {
                        for (int j = 0; j < nativeShader[i].Length; j++)
                        {
                            //Fade the alpha
                            Color temp = objectsToOcclude[i].renderer.materials[j].color;
                            temp.a -= fadeSpeed * Time.deltaTime;
                            //We're done fading this object
                            if (temp.a <= minTransparency[i])
                            {
                                temp.a = minTransparency[i];
                            }
                            //Gotta keep fading
                            else
                            {
                                transitioning = true;
                            }
                            objectsToOcclude[i].renderer.materials[j].color = temp;
                        }
                    }
                }
            }
            //Things are becoming opaque
            else
            {
                for (int i = 0; i < entries; i++)
                {
                    if (objectsToOcclude[i] && objectsToOcclude[i].renderer.material.color.a != maxTransparency[i])
                    {
                        //Fade the alpha
                        Color temp = objectsToOcclude[i].renderer.material.color;
                        temp.a += fadeSpeed * Time.deltaTime;
                        //We're done fading this object
                        if (temp.a >= maxTransparency[i])
                        {
                            temp.a = maxTransparency[i];
                            for (int j = 0; j < nativeShader[i].Length; j++)
                            {
                                //Switch shaders
                                objectsToOcclude[i].renderer.materials[j].shader = nativeShader[i][j];
                            }
                        }
                        //Gotta keep fading
                        else
                        {
                            transitioning = true;
                        }
                        objectsToOcclude[i].renderer.material.color = temp;
                    }
                }
            }
        }
	}

    //When the player enters
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            //Debug.Log("Player entering " + gameObject.name);
            //Switch to the transparent texture
            for (int i = 0; i < entries; i++)
            {
                //Make every shader transparent
                for (int j = 0; j < nativeShader[i].Length; j++)
                {
                    //Begin each object's journey towards transparency
                    objectsToOcclude[i].renderer.materials[j].shader = transparentShader;
                }
                if (!transitioning)
                {
                    Color temp = objectsToOcclude[i].renderer.material.color;
                    temp.a = maxTransparency[i];
                    objectsToOcclude[i].renderer.material.color = temp;
                }
            }

            //Let them know we're gonna be making things transparent
            occluding = true;
            transitioning = true;
        }
    }

    //When the player leaves
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            //Debug.Log("Player entering " + gameObject.name);

            //Let them know we're making things opque
            occluding = false;
            transitioning = true;
        }
    }
}
