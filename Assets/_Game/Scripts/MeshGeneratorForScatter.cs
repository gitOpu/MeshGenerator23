using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;


[Serializable]
public class SuperVector3
{
    public Vector3 coordinate1;
    public Vector3 coordinate2;
    public Vector3 coordinate3;
    public SuperVector3(Vector3 vector1, Vector3 vector2, Vector3 vector3)
    {
        Vector3[] vectorParent = new Vector3[] { vector1, vector2, vector3 };
       
        vectorParent = vectorParent.OrderBy(m => m.x).ToArray();
       
        vectorParent = vectorParent.OrderBy(m => m.z).ToArray(); 

        this.coordinate1 = vectorParent[0];
        this.coordinate2 = vectorParent[1];
        this.coordinate3 = vectorParent[2];
    }
}

public class MeshGeneratorForScatter : MonoBehaviour
{
   
    public Vector3[] vertices;
    public Vector3[] sortedVertices;
   

    public int[] triangles;
    public List<int> listOfTriangles;

 
   // public MeshFilter meshFilter;
   // public MeshRenderer meshRenderer;
    // private
    public List<SuperVector3> listOfSuperVector3;
    private string fileName = "dataset.json";
    
    public string Vector3ToJsonString;
 
    
   
    public GameObject DynamicMeshGenerator(Vector3[] vertices, Material material)
    {
        sortedVertices = vertices.OrderBy(m => m.x).ToArray() ;
        sortedVertices = vertices.OrderBy(m => m.z).ToArray() ;
        //sortedVertices = sortedVertices.Select(c => { c.y = 0; return c; }).ToArray();
        for (int i = 0; i < sortedVertices.Length; i++)
        {
            ClosestVector(sortedVertices[i] );
            
        }
        triangles = listOfTriangles.ToArray();
        
        Mesh mesh = new Mesh();
        mesh.vertices = sortedVertices; mesh.triangles = triangles;
        mesh.RecalculateNormals();
        GameObject go = new GameObject("Mesh", typeof(MeshFilter), typeof(MeshRenderer));
        go.GetComponent<MeshFilter>().mesh = mesh;
        go.GetComponent<MeshRenderer>().material = material;

        return go;

    }
   
    private void ClosestVector(Vector3 target)
    {
        
        Vector3[] closestVector = sortedVertices
          .Where(r => r.x >= target.x)
          .Where(r => r.z >= target.z)
           .OrderBy(r => Vector3.Distance(r, target)).ToArray();
       

        int limit = 0; 
        for(int i = 0; i< closestVector.Length; i++)
        {
            if ( (i + 2) < closestVector.Length)
            {
                SuperVector3 superVector = new SuperVector3(closestVector[i], closestVector[i + 1], closestVector[i + 2]);
                if (IsTriangle(superVector))
                {
                    GenerateTriangle(superVector);
                    limit++;
                    if (limit == 2) break;
                }
            }
           
           
        }

    }


    private bool IsTriangle(SuperVector3 sVector)
    {
        bool status = false;
        float side1 = Vector3.Distance(sVector.coordinate1, sVector.coordinate2);
        float side2 = Vector3.Distance(sVector.coordinate2, sVector.coordinate3);
        float side3 = Vector3.Distance(sVector.coordinate3, sVector.coordinate1);
        if (side1 + side2 > side3 && side2 + side3 > side1 && side3 + side1 > side2) return status = true;
        Debug.Log($"IsTriangle {status} : " + side1 + "; " + side2 + "; " + side2);
        return status;
    }

    private void GenerateTriangle(SuperVector3 newcomerSuperVector3)
    {
        bool haveToListed = true;
        if (listOfSuperVector3.Count > 0)
        {
            for (int i = 0; i < listOfSuperVector3.Count; i++)
            {
                if (listOfSuperVector3[i].coordinate1 == newcomerSuperVector3.coordinate1 &&
                    listOfSuperVector3[i].coordinate2 == newcomerSuperVector3.coordinate2 &&
                    listOfSuperVector3[i].coordinate3 == newcomerSuperVector3.coordinate3 )
                {
                    haveToListed = false;
                }
                
            }
        }
        

        if (haveToListed)
        {
            listOfSuperVector3.Add(newcomerSuperVector3);
            listOfTriangles.Add(FindIndex(newcomerSuperVector3.coordinate1));
            listOfTriangles.Add(FindIndex(newcomerSuperVector3.coordinate2));
            listOfTriangles.Add(FindIndex(newcomerSuperVector3.coordinate3));
            //Debug.Log("Listed");
        }
    }
    private int FindIndex(Vector3 selectedVector)
    {
        // SuperVector3 test3 = listOfSuperVector3.SingleOrDefault(s => s == newcomerSuperVector3);
        if (sortedVertices.Contains(selectedVector)){
          return  Array.FindIndex(sortedVertices, s=>s == selectedVector);
        }
        else
        {
            Debug.LogError("Item not in the list");
            return 0;
        }
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

