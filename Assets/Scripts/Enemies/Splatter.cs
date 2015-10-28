using UnityEngine;
using System.Collections;

public class Splatter : MonoBehaviour {

    private AudioSource audio;
    private ParticleSystem particles;

	// Use this for initialization
	void Start () {
        audio = GetComponent<AudioSource>();
        particles = GetComponent<ParticleSystem>();
        //Start playing immediately
        if (audio)
            audio.Play();
        if (particles)
            particles.Play();
	}
	
	// Update is called once per frame
	void Update () {
        //Kill this if it's not playing something
	    if((!audio || !audio.isPlaying) && (!particles || !particles.isPlaying))
            Destroy(gameObject);
	}
}
