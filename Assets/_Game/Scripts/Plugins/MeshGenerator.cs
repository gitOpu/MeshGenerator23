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
        
        Mesh mesh = new Mesh();

        vertices[0] = dataSet.point1;
        vertices[1] = dataSet.point2;
        vertices[2] = dataSet.point3;

        uv[0] = dataSet.point1;
        uv[1] = dataSet.point2;
        uv[2] = dataSet.point2;
        
        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;

        mesh.vertices = vertices; mesh.uv = uv; mesh.triangles = triangles;
        GameObject go = new GameObject("Triangle", typeof(MeshFilter), typeof(MeshRenderer));
        go.GetComponent<MeshFilter>().mesh = mesh;
        go.GetComponent<MeshRenderer>().material = material;

        return go;
    }

   
}

