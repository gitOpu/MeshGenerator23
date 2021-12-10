using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MeshGenerator : MonoBehaviour
{
   
    public GameObject Generate(DataSet dataSet, Material material)
    {

        Vector3[] vertices = new Vector3[3];
        Vector2[] uv = new Vector2[3];
        int[] triangles = new int[3];
        /*
        vertices[0] = dataSet.point1;
        vertices[1] = dataSet.point2;
        vertices[2] = dataSet.point3;
        uv[0] = dataSet.point1;
        uv[1] = dataSet.point2;
        uv[2] = dataSet.point2;

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;
        */
        vertices = new Vector3[]
        {
          //  new Vector3(0,0,0),
          dataSet.point1,
          dataSet.point2,
          dataSet.point3
        };
        triangles = new int[]
        {
            0,1,2
        };

        Mesh mesh = new Mesh();

        mesh.vertices = vertices; mesh.uv = uv; mesh.triangles = triangles;
        mesh.RecalculateNormals();

        GameObject go = new GameObject("Triangle", typeof(MeshFilter), typeof(MeshRenderer));
        go.GetComponent<MeshFilter>().mesh = mesh;
        go.GetComponent<MeshRenderer>().material = material;

        return go;
    }

   
}

