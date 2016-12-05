using UnityEngine;
using System.Collections;

public class webCamTexture : MonoBehaviour {


	// Use this for initialization
	void Start() {
		WebCamTexture webcamTexture = new WebCamTexture();
		Renderer renderer = GetComponent<MeshRenderer>();
		renderer.material.mainTexture = webcamTexture;
		webcamTexture.Play();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
