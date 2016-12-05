using UnityEngine;
using System.Collections;

namespace ONGLParticles{
	public static class Snapshot : object {
		
		public static int horizontalRes;
		public static string filename = "Assets/testScreenShot.png";

		public static void TakeSnapshot(RenderTexture tex){
			
			int width = horizontalRes;
			int height = horizontalRes;
			RenderTexture renderTex = new RenderTexture(width,height,24);
			Texture2D screenShot = new Texture2D(width,height, TextureFormat.RGB24, false);
			RenderTexture.active = tex;
			screenShot.ReadPixels(new Rect(0, 0, width,height), 0, 0);
			RenderTexture.active = null;
			byte[] bytes = screenShot.EncodeToPNG();
			System.IO.File.WriteAllBytes("Assets/"+filename+".png", bytes);
			Debug.Log(string.Format("Took screenshot to: {0}", filename));


		}


		public static void TakeSnapshot(Texture2D tex){

			byte[] bytes = tex.EncodeToPNG();
			System.IO.File.WriteAllBytes("Assets/"+filename+".png", bytes);
			Debug.Log(string.Format("Took screenshot to: {0}", filename));


		}
	}
}