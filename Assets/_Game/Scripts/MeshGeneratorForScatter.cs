using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;



public class MeshGeneratorForScatter  : MonoBehaviour
{
   

    private Vector3[] vertices;
    private Vector3[] sortedVertices;
    private Material material;

    private int[] triangles;
    private List<int> listOfTriangles;
    
     public MeshFilter meshFilter;
     public MeshRenderer meshRenderer;

    // private
    public List<SuperVector3> listOfSuperVector3;
   
    
    public Vector3[] closestVector;
    public Vector3[] closestVector2;
    
    
     
    public void DynamicMeshGenerator(Vector3[] scatterData,  Material material)
    {
        
       vertices = new Vector3[scatterData.Length];
        sortedVertices = new Vector3[scatterData.Length];
        triangles = new int[scatterData.Length * 2 * 3];
        listOfTriangles = new List<int>(scatterData.Length * 2 * 3);
         

        vertices = scatterData;
        sortedVertices = vertices.OrderBy(m => m.x).ToArray();
        sortedVertices = vertices.OrderBy(m => m.z).ToArray();

        //sortedVertices = sortedVertices.Select(c => { c.y = 0; return c; }).ToArray();
        for (int i = 0; i < sortedVertices.Length; i++)
        {
            ClosestVectorFor(sortedVertices[i]);
        }
        
        triangles = listOfTriangles.ToArray();
        // normal calculation
        for (int i = 0; i < triangles.Length; i += 6)
        {
            int temp0 = triangles[i];
            triangles[i] = triangles[i + 2];
            triangles[i + 2] = temp0;
        }
        
        Mesh mesh = new Mesh();
        mesh.Clear();

        mesh.vertices = sortedVertices; 
        mesh.triangles = triangles;
        //mesh.RecalculateNormals();
        // mesh.triangles = mesh.triangles.Reverse().ToArray();
        //  GameObject go = new GameObject("Mesh", typeof(MeshFilter), typeof(MeshRenderer));
        // go.GetComponent<MeshFilter>().mesh = mesh;
        // go.GetComponent<MeshRenderer>().material = material;

        // return go;

        meshFilter.mesh = mesh;
        meshRenderer.material = material;
    }

    
    private void ClosestVectorFor(Vector3 target)
    {

        closestVector = sortedVertices
         .Where(r => r.x >= target.x)
         .Where(r => r.z >= target.z)
          .Where(r => r != target)
          .OrderBy(r => Vector3.Distance(r, target)).ToArray();




        int limit = 0;
        for (int i = 0; i < closestVector.Length; i++)
        {
            if ((i + 1) < closestVector.Length)
            {

                closestVector2 = ClosestVectorsAgainist(closestVector[i], target);

                for (int j = 0; j < closestVector2.Length; j++)
                {
                    SuperVector3 superVector = new SuperVector3(target, closestVector[i], closestVector2[j]);
                    if (IsTriangle(superVector))
                    {

                        if (IsNotListedListedNow(superVector))
                        {
                            CreateTriangleNow(superVector);
                            limit++;
                            break;
                        }


                    }

                }

                if (limit == 2) break;

            }
        }


    }

    private Vector3[] ClosestVectorsAgainist(Vector3 vectorFor, Vector3 except)
    {
        return sortedVertices
              .OrderBy(r => Vector3.Distance(r, vectorFor))
              .Where(r => r != vectorFor)
              .Where(r => r != except)
              .Where(r => r.x >= vectorFor.x)
              .Where(r => r.z >= vectorFor.z)
              .ToArray();

    }


    private bool IsTriangle(SuperVector3 sVector)
    {
        bool status = false;
        float side1 = Vector3.Distance(sVector.coordinate1, sVector.coordinate2);
        float side2 = Vector3.Distance(sVector.coordinate2, sVector.coordinate3);
        float side3 = Vector3.Distance(sVector.coordinate3, sVector.coordinate1);


        if (side1 + side2 > side3 && side2 + side3 > side1 && side3 + side1 > side2)
        {
            // intermediate any points
            if (!IsAnyPointInTriangleEdge(sVector.coordinate1, sVector.coordinate2) &&
                !IsAnyPointInTriangleEdge(sVector.coordinate2, sVector.coordinate3) &&
                !IsAnyPointInTriangleEdge(sVector.coordinate3, sVector.coordinate1))
            {
                status = true;
            }

        }
       // Debug.Log($"IsTriangle for Targer:{sVector.coordinate1} => {status} :  {sVector.coordinate1}, {sVector.coordinate2}, {sVector.coordinate3} - {side1}/{side2}/{side3}");

        return status;


    }

    private bool IsAnyPointInTriangleEdge(Vector3 vector1, Vector3 vector2)
    {
        bool status = false;
        if (vector1.z == vector2.z)
        {
            float smallX = vector1.x < vector2.x ? vector1.x : vector2.x;
            float largeX = smallX == vector1.x ? vector2.x : vector1.x;

            for (int i = 0; i < closestVector.Length; i++)
            {
                if (closestVector[i].z == vector1.z && closestVector[i].x > smallX && closestVector[i].x < largeX)
                {
                    status = true; Debug.Log(" in between point found"); break;
                }
            }
        }

        if (vector1.x == vector2.x)
        {
            float smallZ = vector1.z < vector2.z ? vector1.z : vector2.z;
            float largeZ = smallZ == vector1.z ? vector2.z : vector1.z;

            for (int i = 0; i < closestVector.Length; i++)
            {
                if (closestVector[i].x == vector1.x && closestVector[i].z > smallZ && closestVector[i].z < largeZ)
                {
                    status = true; Debug.Log("in between point found"); break;
                }
            }
        }



        return status;
    }
    private bool IsNotListedListedNow(SuperVector3 newcomerSuperVector3)
    {
        bool isNotListed = true;
        if (listOfSuperVector3.Count > 0)
        {
            for (int i = 0; i < listOfSuperVector3.Count; i++)
            {
                if (listOfSuperVector3[i].coordinate1 == newcomerSuperVector3.coordinate1 &&
                    listOfSuperVector3[i].coordinate2 == newcomerSuperVector3.coordinate2 &&
                    listOfSuperVector3[i].coordinate3 == newcomerSuperVector3.coordinate3)
                {
                    isNotListed = false;
                }
            }
        }
        if (isNotListed)
        {
            listOfSuperVector3.Add(newcomerSuperVector3);
            // CreateDummyTriangle(newcomerSuperVector3);
        }
        return isNotListed;
    }
    
    private void CreateTriangleNow(SuperVector3 newcomerSuperVector3)
    {
        listOfTriangles.Add(FindIndex(newcomerSuperVector3.coordinate1));
        listOfTriangles.Add(FindIndex(newcomerSuperVector3.coordinate2));
        listOfTriangles.Add(FindIndex(newcomerSuperVector3.coordinate3));
        //Debug.Log("Listed");

    }
    private int FindIndex(Vector3 selectedVector)
    {
        // SuperVector3 test3 = listOfSuperVector3.SingleOrDefault(s => s == newcomerSuperVector3);
        if (sortedVertices.Contains(selectedVector))
        {
            return Array.FindIndex(sortedVertices, s => s == selectedVector);
        }
        else
        {
            Debug.LogError("Item not in the list");
            return 0;
        }
    }
    /*
    
    private void OnDrawGizmos()
    {
        if (vertices == null) return;
        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.15f);

        }

    }*/
}

