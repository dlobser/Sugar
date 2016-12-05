using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace ONGLParticles
{

    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshFilter))]
    public class MakeParticlesVerts : MonoBehaviour
    {

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
        int triCount2 = 0;

        Vector2[] uvs;
        Vector2[] uvs2;
        Vector3[] vs;
        Color[] colors;
        int[] tris;

        void Awake()
        {
            MFilter = GetComponent<MeshFilter>();

            Init();
        }

        public void Init()
        {
            meshObjects = new List<GameObject>();
            vertPositions = new List<Vector3>();
            makeMesh();
            GenerateMesh();
        }


        void makeMesh()
        {
            GameObject n = new GameObject();
            n.transform.parent = this.transform;
            MeshRenderer rend = n.AddComponent<MeshRenderer>();
            rend.material = particleMaterial;
            MFilter = n.AddComponent<MeshFilter>();
            MFilter.mesh = new Mesh();
            //		n.AddComponent<updateBounds> ();
            meshObjects.Add(n);
        }

        void meshElements()
        {
            count = -1;
            tCount = -1;
            triCount = -1;
            triCount2 = 0;
        }

        
        void GenerateMesh()
        {

            pointCount = Mathf.Min(65000, numPoints * numPoints);

            meshElements();

            if (MMesh == null)
            {
                MFilter.mesh = new Mesh();
                MMesh = MFilter.sharedMesh;
            }

            MMesh.Clear();
            MMesh.vertices = new Vector3[pointCount];
            MMesh.triangles = new int[pointCount * 3];
            MMesh.uv = new Vector2[pointCount];
            MMesh.uv2 = new Vector2[pointCount];
            MMesh.colors = new Color[pointCount];

            uvs = MMesh.uv;
            uvs2 = MMesh.uv2;
            vs = new Vector3[pointCount];
            colors = new Color[pointCount];
            tris = new int[pointCount * 3];

            int h = -1;
            int v = 0;
            int total = -1;

            for (int j = 0; j < numPoints * numPoints; j++)
            {
                h += 1;
                //Debug.Log("h: " + h + " v: " + v + " total: " + total);
                if (h > numPoints - 1)
                {
                   
                    h = 0;
                    v += 1;
                }
                total += 1;
                if (total + 1 > 65000)
                {
                    MMesh.vertices = vs;
                    MMesh.triangles = tris;
                    MMesh.uv = uvs;
                    MMesh.uv2 = uvs2;
                    MMesh.colors = colors;

                    MMesh.RecalculateNormals();
                    MMesh.RecalculateBounds();
                    meshElements();
                    makeMesh();

                    MFilter.mesh = new Mesh();
                    MMesh = MFilter.sharedMesh;

                    MMesh.Clear();
                    MMesh.vertices = new Vector3[pointCount];
                    MMesh.triangles = new int[pointCount * 3];
                    MMesh.uv = new Vector2[pointCount];
                    MMesh.uv2 = new Vector2[pointCount];
                    MMesh.colors = new Color[pointCount];


                    uvs = MMesh.uv;
                    uvs2 = MMesh.uv2;
                    vs = new Vector3[pointCount];
                    tris = new int[pointCount * 3];
                    colors = new Color[pointCount];
                    total = -1;

                }

                float off = (float)1 / (numPoints);

                Vector2 goodUV = new Vector2((float)h / numPoints, (float)v / numPoints);
                Vector3 goodPos = new Vector3(goodUV.x, goodUV.y, 0);
                Color randColor = new Color(Random.value, Random.value, Random.value, 1.0f);

                Vector2[] uv2v = new Vector2[]{
                    new Vector2(0,0),
                    new Vector2(0,1),
                    new Vector2(1,0),
                    new Vector2(1,1)};

                vs[++count] = goodPos;
                uvs[count] = goodUV;
                uvs2[count] = uv2v[0];
                colors[count] = randColor;

                triCount2++;
                if (triCount2 == 4)
                {

                    Debug.Log(tCount);
                    tris[++tCount] = triCount + 0;
                    tris[++tCount] = triCount + 1;
                    tris[++tCount] = triCount + 2;

                    tris[++tCount] = triCount + 0;
                    tris[++tCount] = triCount + 3;
                    tris[++tCount] = triCount + 2;

       

                    triCount += 4;
                    triCount2 = 0;
                }

            }

            //int nPoints = numPoints - 1;
            //int[] triangles = new int[nPoints * nPoints * 6];
            //for (int ti = 0, vi = 0, y = 0; y < nPoints; y++, vi++)
            //{
            //    for (int x = 0; x < nPoints; x++, ti += 6, vi++)
            //    {
            //        triangles[ti] = vi;
            //        triangles[ti + 3] = triangles[ti + 2] = vi + 1;
            //        triangles[ti + 4] = triangles[ti + 1] = vi + nPoints + 1;
            //        triangles[ti + 5] = vi + nPoints + 2;
            //    }
            //}

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
