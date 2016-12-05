using UnityEngine;
using System.Collections;

namespace ONGLParticles{
	public class PointPositionToTexture : MonoBehaviour {

		public int width;
		Texture2D posTexture;
		public string filename;
		public bool generateSphere;
		public bool generateSphereShell;
		// Use this for initialization
		void Start () {
			posTexture = new Texture2D (width, width, TextureFormat.ARGB32, false);
			if (generateSphere) {
				GenerateRandomSphere ();
			} else if (generateSphereShell) {
				GenerateSphereShell ();
			}
		}

		public void GenerateRandomSphere(){

			int detail = width;//edges.Count;
			Vector3 vp;

			for (int i = 0; i < (int)detail; i++) {
				for (int j = 0; j < (int)detail; j++) {
					Vector3 rCol = (Random.insideUnitSphere*.5f)+new Vector3(.5f,.5f,.5f);
					posTexture.SetPixel(i, j, new Color(Mathf.Pow(rCol.x,.454f),Mathf.Pow(rCol.y,.454f),Mathf.Pow(rCol.z,.454f), 1));
				}
			}


			posTexture.Apply ();
			Snapshot.filename = filename;
			Snapshot.TakeSnapshot (posTexture);

		}
		public void GenerateSphereShell(){

			int detail = width;//edges.Count;
			Vector3 vp;

			for (int i = 0; i < (int)detail; i++) {
				for (int j = 0; j < (int)detail; j++) {
					Vector3 rCol = (Random.onUnitSphere*.5f)+new Vector3(.5f,.5f,.5f);
					posTexture.SetPixel(i, j, new Color(Mathf.Pow(rCol.x,.454f),Mathf.Pow(rCol.y,.454f),Mathf.Pow(rCol.z,.454f), 1));
				}
			}

			posTexture.Apply ();
			Snapshot.filename = filename;
			Snapshot.TakeSnapshot (posTexture);

		}
	
	}
}