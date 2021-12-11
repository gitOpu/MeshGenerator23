using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;


 

 
public class Practice : MonoBehaviour
{
 

    public Vector3[] vertices;
    public Vector3[] sortedVertices;
    public Material material;

    
   
    public int[] triangles;
    public List<int> listOfTriangles;
   

 
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;
    // private
    public List<SuperVector3> listOfSuperVector3;
    private string fileName = "dataset.json";

   

    public Vector3[] closestVector;
    public Vector3[] closestVector2;
    
    
    

    void Start()
    {
      
        DynamicMeshGenerator(vertices, material);
        UpdateMesh();
    }
    
    void UpdateMesh()
    {
        triangles = listOfTriangles.ToArray();

        for (int i = 0; i < triangles.Length; i += 6)
        {
            int temp0 = triangles[i];
            triangles[i] = triangles[i + 2];
            triangles[i + 2] = temp0;
        }

        Mesh mesh = new Mesh();
        mesh.vertices = sortedVertices; mesh.triangles = triangles;
       //mesh.RecalculateNormals();
       // FixMeshUtil.FixNormals(mesh);
        meshFilter.mesh = mesh;

       // tempTriangles = new int[triangles.Length];
       // tempTriangles = triangles;
        

       
       // mesh.triangles = mesh.triangles.Reverse().ToArray();


    }
    public void DynamicMeshGenerator(Vector3[] vertices, Material material)
    {
        sortedVertices = vertices.OrderBy(m => m.x).ToArray() ;
       
        sortedVertices = vertices.OrderBy(m => m.z).ToArray() ;
        //sortedVertices = sortedVertices.Select(c => { c.y = 0; return c; }).ToArray();


      //  StartCoroutine(CalculationWithDelay());
        CalculationWithDelay();

    }
   
    void CalculationWithDelay()
    {
        for (int i = 0; i < sortedVertices.Length; i++)
        {
            ClosestVectorFor(sortedVertices[i]);
           // yield return new WaitForSeconds(0.2f);
        }
    }
    private void ClosestVectorFor(Vector3 target)
    {
        
         closestVector = sortedVertices
          .Where(r => r.x >= target.x)
          .Where(r => r.z >= target.z)
           .Where(r => r != target)
           .OrderBy(r => Vector3.Distance(r, target)).ToArray();

         
        

        int limit = 0; 
        for(int i = 0; i< closestVector.Length; i++)
        {
            if ( (i + 1) < closestVector.Length)
            {

                closestVector2 = ClosestVectorFor(closestVector[i], target);
                
                for (int j = 0; j < closestVector2.Length; j++)
                {
                    SuperVector3 superVector = new SuperVector3(target, closestVector[i], closestVector2[j]);
                    if (IsTriangle(superVector))
                    {

                        if (IsNotListedListedNow(superVector)) {
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

    private Vector3[] ClosestVectorFor(Vector3 vectorFor, Vector3 except)
    {
      return sortedVertices
            .OrderBy(r => Vector3.Distance(r, vectorFor))
            .Where(r=> r != vectorFor)
            .Where(r=> r != except)
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
            if( !IsAnyPointInTriangleEdge(sVector.coordinate1,sVector.coordinate2) &&
                !IsAnyPointInTriangleEdge(sVector.coordinate2, sVector.coordinate3) &&
                !IsAnyPointInTriangleEdge(sVector.coordinate3, sVector.coordinate1) )
            {
                status = true;
            }
               
        }
        Debug.Log($"IsTriangle for Targer:{sVector.coordinate1} => {status} :  {sVector.coordinate1}, {sVector.coordinate2}, {sVector.coordinate3} - {side1}/{side2}/{side3}");

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
        if(isNotListed)
        {
            listOfSuperVector3.Add(newcomerSuperVector3);
           // CreateDummyTriangle(newcomerSuperVector3);
        }
        return isNotListed;
    }
    private void CreateDummyTriangle(SuperVector3 sv)
    {
        if (listOfSuperVector3.Count > 1)
        {
           
            for (int i = 0; i < listOfSuperVector3.Count; i++)
            {
                if (listOfSuperVector3[i].coordinate1 == sv.coordinate1 && listOfSuperVector3[i].coordinate2 == sv.coordinate2 )
                {
                    SuperVector3 dumy1 =   new SuperVector3(sv.coordinate1, sv.coordinate3, listOfSuperVector3[i].coordinate3);
                    SuperVector3 dumy2 =   new SuperVector3(sv.coordinate2, sv.coordinate3, listOfSuperVector3[i].coordinate3);

                    if (IsTriangle(dumy1)) listOfSuperVector3.Add(dumy1);
                    if (IsTriangle(dumy2)) listOfSuperVector3.Add(dumy2);
                }
                else if (listOfSuperVector3[i].coordinate2 == sv.coordinate2 && listOfSuperVector3[i].coordinate3 == sv.coordinate3)
                {
                    SuperVector3 dumy1 = new SuperVector3(sv.coordinate2, sv.coordinate1, listOfSuperVector3[i].coordinate1);
                    SuperVector3 dumy2 = new SuperVector3(sv.coordinate3, sv.coordinate1, listOfSuperVector3[i].coordinate1);

                    if (IsTriangle(dumy1)) listOfSuperVector3.Add(dumy1);
                    if (IsTriangle(dumy2)) listOfSuperVector3.Add(dumy2);
                }
                else if (listOfSuperVector3[i].coordinate3 == sv.coordinate3 && listOfSuperVector3[i].coordinate1 == sv.coordinate1)
                {
                    SuperVector3 dumy1 = new SuperVector3(sv.coordinate1, sv.coordinate2, listOfSuperVector3[i].coordinate2);
                    SuperVector3 dumy2 = new SuperVector3(sv.coordinate3, sv.coordinate2, listOfSuperVector3[i].coordinate2);

                    if (IsTriangle(dumy1)) listOfSuperVector3.Add(dumy1);
                    if (IsTriangle(dumy2)) listOfSuperVector3.Add(dumy2);
                }

            }
        }
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
        if (sortedVertices.Contains(selectedVector)){
          return  Array.FindIndex(sortedVertices, s=>s == selectedVector);
        }
        else
        {
            Debug.LogError("Item not in the list");
            return 0;
        }
    }
     
    /*
    void GenerateMeshNow(Vector3[] ver, int[] tris, Material material)
    {
        Mesh mesh = new Mesh();
        mesh.vertices = ver; mesh.triangles = tris;
        mesh.RecalculateNormals();
        
        GameObject go = new GameObject("Triangle", typeof(MeshFilter), typeof(MeshRenderer));
        go.GetComponent<MeshFilter>().mesh = mesh;
        go.GetComponent<MeshRenderer>().material = material;


    }
    */
    private void OnDrawGizmos()
    {
        if (vertices == null) return;
        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.15f);
           
        }
        
    }
}

