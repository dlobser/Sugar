using UnityEngine;
using System.Collections;

public class moveForward1 : MonoBehaviour {

	public float maxSpeed;
	float speed;
	public float acceleration = .02f;
	public float deceleration = 1;
	Vector3 aimer;

	void Start(){
		aimer = Vector3.zero;
	}

	// Use this for initialization
	void reset(){
		speed = 0;
	}
	// Update is called once per frame
	void Update () {
		if(Input.GetButton("Fire1")|| Input.GetMouseButtonUp(0)||Input.GetMouseButtonDown(0)) {  
			print (speed);
			aimer = Vector3.Lerp (aimer, Camera.main.transform.forward, .1f);
			if(speed<maxSpeed)
				speed+=acceleration*Time.deltaTime;
		}
		else if (speed > 0)
			speed -= deceleration*Time.deltaTime;
		if (speed > 0) {
			Vector3 F = Camera.main.transform.forward;

			transform.Translate(aimer*speed*Time.deltaTime);
		}
//		transform.localPosition = Vector3.Scale (transform.localPosition,new Vector3(transform.localPosition.x, transform.localPosition.y*.96f, transform.localPosition.z));

	}
}
