using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;



public class MeshController : MonoBehaviour
{
    public GameObject meshTemplate;
    public Transform container;
    
    public List<Material> materials;
    private string fileName = "dataset.json";
    private string defaultfileName = "default.json";
    
    public List<DataSet> userListOfDataSet = new List<DataSet>();
    public string jsonData = "";


    void Awake()
    {
        GenerateMesh(userListOfDataSet);

      
    }
    public void GenerateMesh(List<DataSet> ListOfDS)
    {

        


        foreach (Transform mesh in container)
        {
            Destroy(mesh.gameObject);
        }

        foreach (DataSet item in ListOfDS)
        {
            if(item.list.Count > 0)
            {
                GameObject go = Instantiate(meshTemplate);
                MeshGeneratorForScatter meshGeneratorForScatter = go.GetComponent<MeshGeneratorForScatter>();
                meshGeneratorForScatter.DynamicMeshGenerator(item.list.ToArray(), materials[UnityEngine.Random.Range(0, materials.Count)]);
                go.transform.parent = container;
               
            }
            else
            {
                Debug.LogError("Index is zero");
            }
            
        }
    }

    public void Restore()
    {
       // List<DataSet> ListOfDataSet = FileHandler.ReadFromJson<DataSet>(defaultfileName);
        GenerateMesh(userListOfDataSet);
       
    }

    private void OnDrawGizmos()
    {
        if (userListOfDataSet == null) return;
        foreach(DataSet ds in userListOfDataSet)
        for (int i = 0; i < ds.list.Count; i++)
        {
            Gizmos.DrawSphere(ds.list[i], 0.15f);

        }

    }

   
}


 /* foreach (DataSet ds in userListOfDataSet)
     for (int i = 0; i < ds.list.Count; i++)
     {
         jsonData += "{\"x\":" + ds.list[i].x +
             ",\"y\":" + ds.list[i].y +
             ",\"z\":" + ds.list[i].z + "},"
            ;
     }*/
