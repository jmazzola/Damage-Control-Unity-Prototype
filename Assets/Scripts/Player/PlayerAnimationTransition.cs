using UnityEngine;
using System.Collections;

public class PlayerAnimationTransition : MonoBehaviour {

	void Start()
	{
		animation["run_forward"].speed = 0.8f;
		animation["run_backward"].speed = 0.9f;
		animation["run_left"].speed = 0.8f;
		animation["run_right"].speed = 0.8f;
	}

	void Update () 
	{
		float myZ = Vector3.Dot(transform.forward, Vector3.forward);

		if (myZ >= 0)
		{
			if(Input.GetKey(KeyCode.W))		
				animation.CrossFade("run_forward", 0.1f);
			else if(Input.GetKey(KeyCode.S))
				animation.CrossFade("run_backward", 0.1f);
			else if(Input.GetKey(KeyCode.A))
				animation.CrossFade("run_left", 0.1f);
			else if(Input.GetKey(KeyCode.D))
				animation.CrossFade("run_right", 0.1f);
			else
				animation.CrossFade("idle", 0.1f);
		}
		else
		{
			if(Input.GetKey(KeyCode.W))	
				animation.CrossFade("run_backward", 0.1f);
			else if(Input.GetKey(KeyCode.S))
				animation.CrossFade("run_forward", 0.1f);
			else if(Input.GetKey(KeyCode.A))
				animation.CrossFade("run_right", 0.1f);
			else if(Input.GetKey(KeyCode.D))
				animation.CrossFade("run_left", 0.1f);
			else
				animation.CrossFade("idle", 0.2f);
		}

	}
}
