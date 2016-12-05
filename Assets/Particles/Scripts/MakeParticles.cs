using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace ONGLParticles{

	[RequireComponent (typeof (MeshRenderer))]
	[RequireComponent (typeof (MeshFilter))]
	public class MakeParticles : MonoBehaviour {

		public int numPoints = 1024;
		public Material particleMaterial;

		MeshFilter MFilter;
		Mesh MMesh;

		Texture2D texture;
		List<Vector3> vertPositions;

		int pointCount;

		List<GameObject> meshObjects;

		int count = -1;
		int tCount = -1;
		int triCount = 0;



		void Awake () {
			MFilter = GetComponent<MeshFilter> ();

			Init();
		}

		public void Init() {
			meshObjects = new List<GameObject>();
			vertPositions = new List<Vector3>();
			makeMesh();
			GenerateMesh();
		}


		void makeMesh(){
			GameObject n = new GameObject ();
			n.transform.parent = this.transform;
			MeshRenderer rend = n.AddComponent<MeshRenderer> ();
			rend.material = particleMaterial;
			MFilter = n.AddComponent<MeshFilter> ();
			MFilter.mesh = new Mesh ();
            n.AddComponent<updateBounds>();
            meshObjects.Add(n);
		}

		void meshElements(){
			count = -1;
			tCount = -1;
			triCount = 0;
		}

		void GenerateMesh(){

			pointCount = Mathf.Min(65000,numPoints*numPoints * 4);

			meshElements ();

			if (MMesh == null){
				MFilter.mesh = new Mesh();
				MMesh = MFilter.sharedMesh;
			}

			MMesh.Clear();
			MMesh.vertices = new Vector3[pointCount];
			MMesh.triangles = new int[pointCount*3];
			MMesh.uv = new Vector2[pointCount];
			MMesh.uv2 = new Vector2[pointCount];
			MMesh.colors = new Color[pointCount];

			Vector2[] uvs = MMesh.uv;
			Vector2[] uvs2 = MMesh.uv2;
			Vector3[] vs = new Vector3[pointCount];
			Color[] colors = new Color[pointCount];
			int[] tris = new int[pointCount * 3];

			int h = -1;
			int v = -1;
			int total = -1;

			for(int j = 0 ; j < numPoints*numPoints ; j++) {
				h+=1;
				if(h>numPoints-1){
					h=0;
					v+=1;
				}
				total+=4;
				if(total+4>65000){
					MMesh.vertices = vs;
					MMesh.triangles = tris;
					MMesh.uv = uvs;
					MMesh.uv2 = uvs2;
					MMesh.colors = colors;

					MMesh.RecalculateNormals();
					MMesh.RecalculateBounds();
					meshElements ();
					makeMesh ();

					MFilter.mesh = new Mesh();
					MMesh = MFilter.sharedMesh;

					MMesh.Clear();
					MMesh.vertices = new Vector3[pointCount];
					MMesh.triangles = new int[pointCount*3];
					MMesh.uv = new Vector2[pointCount];
					MMesh.uv2 = new Vector2[pointCount];
					MMesh.colors = new Color[pointCount];


					uvs = MMesh.uv;
					uvs2 = MMesh.uv2;
					vs = new Vector3[pointCount];
					tris = new int[pointCount * 3];
					colors = new Color[pointCount];
					total=-1;

				}

				float off = (float) 1/(numPoints);

				Vector2 goodUV = new Vector2((float)h/numPoints,(float)v/numPoints);
				Vector3 goodPos = new Vector3(goodUV.x,goodUV.y,0);
				Color randColor = new Color (Random.value,Random.value,Random.value, 1.0f);

				Vector2[] uv2v = new Vector2[]{
					new Vector2(0,0),
					new Vector2(0,1),
					new Vector2(1,0),
					new Vector2(1,1)};

				vs[++count] = goodPos;
				uvs[count] = goodUV;
				uvs2[count] = uv2v[0];
				colors [count] = randColor;

				vs[++count] = goodPos;
				uvs[count] = goodUV;
				uvs2[count] = uv2v[1];
				colors [count] = randColor;

				vs[++count] = goodPos;
				uvs[count] = goodUV;
				uvs2[count] = uv2v[2];
				colors [count] = randColor;

				vs[++count] = goodPos;
				uvs[count] = goodUV;
				uvs2[count] = uv2v[3];
				colors [count] = randColor;

				Vector3 rando = Random.insideUnitSphere;
				vertPositions.Add(rando);

				tris[++tCount] = triCount+0;
				tris[++tCount] = triCount+1;
				tris[++tCount] = triCount+3;

				tris[++tCount] = triCount+0;
				tris[++tCount] = triCount+3;
				tris[++tCount] = triCount+2;

				triCount+=4;

			}

			MMesh.vertices = vs;
			MMesh.triangles = tris;
			MMesh.uv = uvs;
			MMesh.uv2 = uvs2;
			MMesh.colors = colors;

			MMesh.RecalculateNormals();
			MMesh.RecalculateBounds();

		}

	}
}
