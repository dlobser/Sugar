using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveToCam : MonoBehaviour {
    float original;
	// Use this for initialization
	void Start () {
        original = this.transform.position.z;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 lerped = Vector3.Lerp(this.transform.position, Camera.main.transform.position,.01f);
        this.transform.position = new Vector3(lerped.x, lerped.y, original);
	}
}
