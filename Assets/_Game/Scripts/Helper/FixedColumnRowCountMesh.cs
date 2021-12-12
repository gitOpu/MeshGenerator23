using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class FixedColumnRowCountMesh : MonoBehaviour
{
    public int xSize = 20; // rect
    public int zSize = 10;
    public Vector3[] vertices;
    public int[] triangles;
    public GameObject pointIndicator;
    // public Material material;
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;
    public string  jsonData = "";
    void Start()
    {
        StartCoroutine(CreateShape());
        foreach (Vector3 item in vertices)
        {
            jsonData += "{\"x\":"+item.x +
                ",\"y\":" + item.y +
                ",\"z\":" + item.z + "},"
               ;
        }
    }

    IEnumerator CreateShape()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float y = Mathf.PerlinNoise(x * 0.3f, z * 0.3f) * 2;
                vertices[i] = new Vector3(x-10 , y, z-7);
                i++;
            }
        }

        /* foreach(Vector3 item in vertices)
         {
             GameObject temp = Instantiate(pointIndicator);
             temp.transform.position = item;

         }*/

        triangles = new int[xSize * zSize * 3 * 2];
        int vert = 0;
        int tris = 0;
        // List<int> tr = new List<int>();

        for (int j = 0; j < zSize; j++)
        {
            for (int i = 0; i < xSize; i++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;
                vert++;
                tris += 6;

                /* tr.Add(i);
                 tr.Add(xSize + i + 1);
                 tr.Add(i + 1);
                 tr.Add(i + 1);
                 tr.Add(xSize + i + 1);
                 tr.Add(xSize + i + 2);*/

                // triangles[i] = i;
                //  triangles[i+1] = (xSize) +i+1;
                // triangles[i+2] = i+1;
                yield return new WaitForSeconds(0.01f);
            }
            vert++;
        }


        // triangles = tr.ToArray();

    }

    void Update()
    {
        UpdateMesh();
    }
    void UpdateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices; mesh.triangles = triangles;
        mesh.RecalculateNormals();
        //GameObject go = new GameObject("Mesh", typeof(MeshFilter), typeof(MeshRenderer));
        meshFilter.mesh = mesh;

    }
    private void OnDrawGizmos()
    {
        if (vertices == null) return;
        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.15f);

        }
    }

}
   