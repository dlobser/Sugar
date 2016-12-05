using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class DLUtils  {

	public static void makeChildList(GameObject container, List<GameObject> checkList){
		for (int i = 0; i < container.transform.childCount; i++) {
			Transform go = container.transform.GetChild (i);
			checkList.Add (go.gameObject);
			if (go.transform.childCount > 0)
				makeChildList (go.gameObject, checkList);
		}
	}

	public static void copyTransforms(GameObject b, GameObject a){
		a.transform.localPosition = b.transform.localPosition;
		a.transform.localScale = b.transform.localScale;
		a.transform.localEulerAngles = b.transform.localEulerAngles;
	}

	public static void copyWorldTransforms(GameObject b, GameObject a){
		a.transform.position = b.transform.position;
		a.transform.localScale = b.transform.localScale;
		a.transform.eulerAngles = b.transform.eulerAngles;
	}

	public static void childrenToList(GameObject a, List<GameObject> l){
		for (int i = 0; i < a.transform.childCount; i++) {
			l.Add(a.transform.GetChild(i).gameObject);
		}
	}

	public static Vector2 constrainVec2(Vector2 min, Vector2 max, Vector2 b){
		return new Vector2 (Mathf.Min (max.x, Mathf.Max (min.x, b.x)), Mathf.Min (max.y, Mathf.Max (min.y, b.y)));
	}

	public static Vector3 constrainVec3(Vector3 min, Vector3 max, Vector3 b){
		return new Vector3 (
			Mathf.Min (max.x, Mathf.Max (min.x, b.x)),
			Mathf.Min (max.y, Mathf.Max (min.y, b.y)),
			Mathf.Min (max.y, Mathf.Max (min.y, b.z))
		);
	}

	public static Vector3 constrainVec3Proper(Vector3 min, Vector3 max, Vector3 b){
		return new Vector3 (
			Mathf.Min (max.x, Mathf.Max (min.x, b.x)),
			Mathf.Min (max.y, Mathf.Max (min.y, b.y)),
			Mathf.Min (max.z, Mathf.Max (min.z, b.z))
		);
	}

	public static int whichChild(Transform a){
		int which = -1;
		Transform b = a.transform.parent;
		for (int i = 0; i < b.childCount; i++) {
			if(b.GetChild(i).Equals(a))
				which = i;
		}
		return which;
	}

	public static Vector2 vec2(float a){
		return new Vector2 (a, a);
	}
	public static Vector3 vec3(float a){
		return new Vector3 (a, a, a);
	}

	public static Vector3[] listToArray(List<Vector3> l, int k, int j){
		Vector3[] returner;
		returner = new Vector3[j-k];
		for(int i = k ; i < j ; i++){
			returner [i] = l [i];
		}
		return returner;
	}
}
