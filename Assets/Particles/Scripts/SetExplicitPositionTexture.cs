using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ONGLParticles{
	public class SetExplicitPositionTexture : MonoBehaviour {

		public Material mat;
		public string explicitTextureFieldName;
		Texture2D tex;
		int size = 1024;
		int count = 0;
		// Use this for initialization
		void Awake () {
			tex = new Texture2D (size, size, TextureFormat.RGBAFloat, false);
			for (int i = 0; i < size; i++) {
				for (int j = 0; j < size; j++) {
					tex.SetPixel (i, i, new Color(0,0,0,0));
				}
			}
			tex.Apply ();
			mat.SetTexture (explicitTextureFieldName, tex);

		}
		
		// Update is called once per frame
		void Update () {
			
		}
	}
}
