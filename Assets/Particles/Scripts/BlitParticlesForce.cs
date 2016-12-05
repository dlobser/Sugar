using UnityEngine;
using System.Collections;

namespace ONGLParticles{
	public class BlitParticlesForce : MonoBehaviour {

		public Texture2D initialPosition;

		public Material ReformatInitialImage;
		public Material ApplyForceToPosition;
		public Material AddVelocityAcceleration;
		public Material VelocityToPosition;
		public Material ParticlePosition;

		 RenderTexture Acceleration;
		 RenderTexture Velocity;
		 RenderTexture OldVelocity;
		 RenderTexture OldPosition;
		 RenderTexture Position;

		Texture2D blackTexture;

//		public RenderTexture Position;	
		public RenderTexture Test;

		bool init = false;

		bool ping = false;

		float count = 0;

		void Start () {

			GenerateBlackTexture ();

            Acceleration = new RenderTexture(1, 1, 24, RenderTextureFormat.ARGBFloat);
            Acceleration.filterMode = FilterMode.Point;
            Velocity = new RenderTexture(1, 1, 24, RenderTextureFormat.ARGBFloat);
            Velocity.filterMode = FilterMode.Point;
            OldVelocity = new RenderTexture(1, 1, 24, RenderTextureFormat.ARGBFloat);
            OldVelocity.filterMode = FilterMode.Point;
            OldPosition = new RenderTexture(1, 1, 24, RenderTextureFormat.ARGBFloat);
            OldPosition.filterMode = FilterMode.Point;
            Position = new RenderTexture(1, 1, 24, RenderTextureFormat.ARGBFloat);
            Position.filterMode = FilterMode.Point;
            //		Position = new RenderTexture (initialPosition.width, initialPosition.height, 24, RenderTextureFormat.ARGBFloat);
            //		Position.filterMode = FilterMode.Point;

            Acceleration.width = initialPosition.width;
            Acceleration.height = initialPosition.width;

            Velocity.width = initialPosition.width;
            Velocity.height = initialPosition.width;

            OldVelocity.width = initialPosition.width;
            OldVelocity.height = initialPosition.width;

            OldPosition.width = initialPosition.width;
            OldPosition.height = initialPosition.width;

            Position.width = initialPosition.width;
            Position.height = initialPosition.width;

            //			Graphics.Blit (blackTexture, Acceleration);
            //			Graphics.Blit (blackTexture, Velocity);
            //			Graphics.Blit (blackTexture, OldVelocity);
            //			Graphics.Blit (blackTexture, OldPosition);
            //			Graphics.Blit (blackTexture, Position);
            clearTextures();
//
//			Acceleration.DiscardContents ();
//			Velocity.DiscardContents ();
//			OldVelocity.DiscardContents ();
//			OldPosition.DiscardContents ();
//			Position.DiscardContents ();

			VelocityToPosition.SetTexture ("_Position", OldPosition);
			AddVelocityAcceleration.SetTexture ("_OldVelocity", OldVelocity);
			ParticlePosition.SetTexture ("_Offset", Position);

//			DoBlits ();
//			Snapshot.horizontalRes = initialPosition.width;
//			Snapshot.TakeSnapshot (Acceleration);


		}

		public void GenerateBlackTexture(){

			blackTexture = new Texture2D (initialPosition.width, initialPosition.width);
			int detail = initialPosition.width;//edges.Count;
			Vector3 vp;

			for (int i = 0; i < (int)detail; i++) {
				for (int j = 0; j < (int)detail; j++) {
					blackTexture.SetPixel(i, j, Color.black);
				}
			}
			blackTexture.Apply ();
		}

		void clearTextures(){
			Graphics.Blit (blackTexture, Acceleration);
			Graphics.Blit (blackTexture, Velocity);
			Graphics.Blit (blackTexture, OldVelocity);
			Graphics.Blit (blackTexture, OldPosition);
			Graphics.Blit (blackTexture, Position);
		}

		void DoBlits(){
			if (!init) {
				Position.DiscardContents ();
				Graphics.Blit (initialPosition, Position, ReformatInitialImage);
				Graphics.Blit (initialPosition, OldPosition, ReformatInitialImage);

				init = true;
			} 
			Graphics.Blit (Position, Acceleration, ApplyForceToPosition);
			Graphics.Blit (Acceleration, Velocity, AddVelocityAcceleration);
			Graphics.Blit (Velocity, OldVelocity);
			Graphics.Blit (Velocity, Position, VelocityToPosition);
			Graphics.Blit (Position, OldPosition);
//			Graphics.Blit (Position, Test);
		}
		
		void Update () {

			count += Time.deltaTime;
			if (count > 1)
				DoBlits ();
			else if (count < 1)
				clearTextures ();
			//else if (count > 5) {
			//	Acceleration.Release ();
			//	Velocity.Release ();
			//	Position.Release ();
			//	OldPosition.Release ();
			//	OldVelocity.Release ();


			//}


			/*
			 * new velocity = old velocity + acceleration
			new position = old position + velocity
			initial position texture
			gets affected by a force - goes to acceleration

			*/

//			if (!init) {
//				Position.DiscardContents ();
//
//	//			Graphics.Blit (initialPosition, Acceleration, ApplyForceToPosition);
//				Graphics.Blit (initialPosition, Position);
//				Graphics.Blit (initialPosition, OldPosition);
//
//				init = true;
//			} 
////			else if (init) {
//
////			if (!ping) {
//				Graphics.Blit (Position, Acceleration, ApplyForceToPosition);
////				Position.DiscardContents ();
//
//				////		}
//				Graphics.Blit (Acceleration, Velocity, AddVelocityAcceleration);
////				Acceleration.DiscardContents ();
//				Graphics.Blit (Velocity, OldVelocity);
////				ping = !ping;
////			}
////			if (ping) {
//				//
//				Graphics.Blit (Velocity, Position, VelocityToPosition);
////				Velocity.DiscardContents ();
////				OldVelocity.DiscardContents ();
////				OldPosition.DiscardContents ();
//
//				Graphics.Blit (Position, OldPosition);
//				Graphics.Blit (Position, Test);
//				ping = !ping;
//			}
	
//			Graphics.Blit (Position, Test);
		
	//
	//		for (int i = 0; i < toBlit.Length; i++) {
	//			if (i == 0 && !init) {
	//				Graphics.Blit (initialPosition, rTextures [i], toBlit [i]);
	//				init = true;
	//			} else if (i == 0) {
	//				Graphics.Blit (outTexture, rTextures [i], toBlit [i]);
	//			}
	//			else
	//				Graphics.Blit (rTextures[i-1], rTextures [i], toBlit [i]);
	//		}
	//		Graphics.Blit (rTextures [rTextures.Length - 1], outTexture);
		}
	}

}