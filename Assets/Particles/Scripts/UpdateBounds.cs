using UnityEngine;
using System.Collections;

namespace ONGLParticles{
	public class updateBounds : MonoBehaviour {


		// Update is called once per frame
		void Awake () {
			Transform camTransform = Camera.main.transform;
			float distToCenter = (Camera.main.farClipPlane - Camera.main.nearClipPlane) / 2.0f;
			Vector3 center = camTransform.position + camTransform.forward * distToCenter;
			float extremeBound = 5000.0f;
			MeshFilter filter = GetComponent<MeshFilter> ();
			filter.sharedMesh.bounds = new Bounds (center, Vector3.one * extremeBound);
		}

	}
}
