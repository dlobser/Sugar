using UnityEngine;
using System.Collections;

public class Gizmo : MonoBehaviour {

	public float size = 1 ;

	void OnDrawGizmos() {
		Gizmos.color = Color.white;
		Gizmos.DrawSphere(transform.position, transform.localScale.x*.1f * size);
		Gizmos.DrawWireSphere(transform.position, transform.localScale.x * size);
	}

}
