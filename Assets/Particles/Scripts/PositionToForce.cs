using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ONGLParticles{
	public class PositionToForce : MonoBehaviour {

		public Material mat;
		public string property;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			mat.SetVector (property, this.transform.position);
		}
	}
}