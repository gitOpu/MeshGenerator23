using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;



public class MeshController : MeshGeneratorForScatter
{
    public Transform container;
    public Material material;
    private string fileName = "dataset.json";
    private string defaultfileName = "default.json";

   // public List<DataSet> ListOfDataSet = new List<DataSet>();

    void Awake()
    {
        List<DataSet> ListOfDataSet = FileHandler.ReadFromJson<DataSet>(fileName);
        GenerateMesh(ListOfDataSet);
    }
    public void GenerateMesh(List<DataSet> ListOfDS)
    {
        foreach (Transform mesh in container)
        {
            Destroy(mesh.gameObject);
        }

        foreach (DataSet item in ListOfDS)
        {
            GameObject go = DynamicMeshGenerator(item.list.ToArray(), material);
            if (go)
            {
                go.transform.parent = container;
             
            }
        }
    }

    public void Restore()
    {
        List<DataSet> ListOfDataSet = FileHandler.ReadFromJson<DataSet>(defaultfileName);
        GenerateMesh(ListOfDataSet);
    }

    /*
    public void LoadOnlineData(Action<bool> callback)
    {
        StartCoroutine(GetWebRequest.GetRequest(url, (isSuccess, onlineData)=> {

            if (isSuccess)
            {
                GenerateMesh(onlineData, true);
                callback(true);
            }
            else
            {
                callback(false);
            }
        
        }));
    }
    public void LoadLocalData(Action<bool> callback)
    {
        FileHandler.ReadDataset((isSuccess, localData) => {

            if (isSuccess)
            {
                GenerateMesh(localData, false);
                callback(true);
            }
            else
            {
                callback(false);
            }

        });
    }

    

   */
}
