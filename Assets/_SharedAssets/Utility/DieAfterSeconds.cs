using UnityEngine;
using System.Collections;

public class DieAfterSeconds : MonoBehaviour {

	public float aliveForSeconds = 1;
	private float counter = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (counter < aliveForSeconds) {
			counter += Time.deltaTime;
		} else
			Destroy (this.gameObject);

	}
}
