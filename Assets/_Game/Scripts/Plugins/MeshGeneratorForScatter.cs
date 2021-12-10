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
       // if   z are equal then small-x will be first point
       // if   x are equarl then  small-z will be fist point

        

        this.coordinate1 = vectorParent[0];
        this.coordinate2 = vectorParent[1];
        this.coordinate3 = vectorParent[2];
    }

}
public enum CheckDataBy
{
    Ascending, Descending
}
public class MeshGeneratorForScatter : MonoBehaviour
{
    public int xSize = 3; // rect
    public int zSize = 2;
    public Vector3[] vertices;
    public Vector3[] sortedVertices;
    
    public Vector3[] sortedVerticesIgnorY;
    //public List<Vector3> listOfVertices;
    public int[] triangles;
    public List<int> listOfTriangles;

   // public GameObject pointIndicator;
    // public Material material;
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;
    // private
   
    public List<SuperVector3> listOfSuperVector3;
    
    

    void Start()
    {
      //  GridTerinSurface();

        
        sortedVertices = vertices.OrderBy(m => m.z).ToArray() ;
        sortedVertices = sortedVertices.Select(c => { c.y = 0; return c; }).ToArray();


        /*
        sortedVerticesIgnorY = new Vector3[sortedVertices.Length];

        for (int i = 0; i < sortedVertices.Length; i++)
        {
            Vector3 temp = sortedVertices[i];
            temp.y = 0;
            sortedVerticesIgnorY[i] = temp;
        }
        sortedVertices = sortedVerticesIgnorY;
        */





        StartCoroutine(StartCalculating());



         /*for (int i = sortedVertices.Length-1; i >= 0; i--)
         {
             SuperVector3 closestVector = ClosestVector(sortedVertices[i], sortedVertices, CheckDataBy.Descending);
             if (closestVector != null) { GenerateTriangle(closestVector); }
         }*/


        
       

    }
    void Update()
    {
        triangles = listOfTriangles.ToArray();
        GenerateMeshNow(sortedVertices, triangles);
    }

   
    IEnumerator StartCalculating()
    {
        for (int i = 0; i < sortedVertices.Length; i++)
        {
            // SuperVector3 closestVector = ClosestVector(sortedVertices[i], sortedVertices, CheckDataBy.Ascending);
            //   if(closestVector != null)  { GenerateTriangle(closestVector); }
            ClosestVector(sortedVertices[i], sortedVertices, CheckDataBy.Ascending);
            yield return new WaitForSeconds(0.1f);
        }
        
    }
    private void ClosestVector(Vector3 target, Vector3[] verticesArray, CheckDataBy checkDataBy)
    {
        // Vector3[] closestVector = verticesArray;
        // if (checkDataBy == CheckDataBy.Ascending)
        // {
        Vector3[] closestVector = verticesArray
          .Where(r => r.x >= target.x).Where(r => r.z >= target.z)
           .OrderBy(r => Vector3.Distance(r, target)).ToArray();
       // }

         if (checkDataBy == CheckDataBy.Descending)
        {
            closestVector = verticesArray
          .Where(r => r.x <= target.x).Where(r => r.z <= target.z)
           .OrderBy(r => Vector3.Distance(r, target)).ToArray();
        }


        //  if(checkDataBy == CheckDataBy.Ascending)
        //  assendingByZvertices = assendingByZvertices.Where(r => r.x >= target.x).Where(r => r.z >= target.z).ToArray();

        if (closestVector.Length >= 4)
        {
            GenerateTriangle(new SuperVector3(closestVector[0], closestVector[1], closestVector[2]));
            GenerateTriangle(new SuperVector3(closestVector[1], closestVector[2], closestVector[3]));
           
        }

        else if (closestVector.Length >= 3)
        {
            GenerateTriangle(new SuperVector3(closestVector[0], closestVector[1], closestVector[2]));
        }
       
       
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
                    listOfSuperVector3[i].coordinate3 == newcomerSuperVector3.coordinate3)
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
            Debug.Log("Listed");
        }
    }
    private int FindIndex(Vector3 selectedVector)
    {
        //    SuperVector3 test3 = listOfSuperVector3.SingleOrDefault(s => s == newcomerSuperVector3);
        if (sortedVertices.Contains(selectedVector)){
          return  Array.FindIndex(sortedVertices, s=>s == selectedVector);
        }
        else
        {
            Debug.LogError("Item not in the list");
            return 0;
        }
    }
    private void GridTerinSurface()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float y = Mathf.PerlinNoise(x * 0.3f, z *0.3f) * 2;
                vertices[i] = new Vector3(x, y, z);
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
                
            }
            vert++;
        }


        // triangles = tr.ToArray();
        UpdateMesh();
    }

    void GenerateMeshNow(Vector3[] ver, int[] tris)
    {
        Mesh mesh = new Mesh();
        mesh.vertices = ver; mesh.triangles = tris;
        //Vector3[] norm = ver.OrderBy(m => m.z).ToArray();
        //mesh.normals = norm;
        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;

    }

    void UpdateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;  mesh.triangles = triangles;
        
       // mesh.RecalculateNormals();
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






/*if (!listOfSuperVector3.Contains(newcomerSuperVector3)){
    listOfSuperVector3.Add(newcomerSuperVector3);
    Debug.Log(" Not listed ");
}
else
{
    Debug.Log("already listed this combinations");
}*/

//if (listOfSuperVector3.Count > 0)
//{
//    // SuperVector3 test1 =  listOfSuperVector3.First(s => s == newSuperVector3);
//    // SuperVector3 test2 = listOfSuperVector3.Single(s => s == newSuperVector3);
//    SuperVector3 test3 = listOfSuperVector3.SingleOrDefault(s => s == newcomerSuperVector3);
//    if (test3 == null)
//    {
//        listOfSuperVector3.Add(newcomerSuperVector3);
//        Debug.Log(" Not listed ");
//    }
//    else
//    {
//        Debug.Log("already listed this combinations");
//    }
//}
//else
//{
//    listOfSuperVector3.Add(newcomerSuperVector3);
//    Debug.Log("1st element ");
//}