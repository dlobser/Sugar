using UnityEngine;
using System.Collections;

namespace ONGLParticles{
	public class BlitParticles : MonoBehaviour {

		public Texture2D initialPosition;
		public Material[] toBlit;
		RenderTexture[] rTextures;
		public RenderTexture outTexture;

		bool init = false;

		void Start () {
			rTextures = new RenderTexture[toBlit.Length];
			for (int i = 0; i < rTextures.Length; i++) {
				rTextures [i] = new RenderTexture (initialPosition.width, initialPosition.height, 24, RenderTextureFormat.ARGBFloat);
				rTextures [i].filterMode = FilterMode.Point;
			}
		}
		
		void Update () {
			for (int i = 0; i < toBlit.Length; i++) {
				if (i == 0 && !init) {
					Graphics.Blit (initialPosition, rTextures [i], toBlit [i]);
					init = true;
				} else if (i == 0) {
					Graphics.Blit (outTexture, rTextures [i], toBlit [i]);
				}
				else
					Graphics.Blit (rTextures[i-1], rTextures [i], toBlit [i]);
			}
			Graphics.Blit (rTextures [rTextures.Length - 1], outTexture);
		}
	}
}
