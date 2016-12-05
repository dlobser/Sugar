using UnityEngine;
using System.Collections;

public class lookAtTarget : MonoBehaviour {

	public GameObject target;
	public float scalar;
	public float maxScale = 1;
	public Vector3 offset = Vector3.zero;
	public Vector3 scaleOffset = Vector3.one;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 s = scaleOffset;
		this.transform.LookAt (target.transform.position);
//		float d = Mathf.Min(maxScale, Vector3.Distance (this.transform.position, target.transform.position));
//		this.transform.localScale = new Vector3 (Mathf.Min(d,s.x), Mathf.Min(d,s.y), Mathf.Min(d,s.z));
		this.transform.Rotate (offset);
	
	}
}
