using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;



public class MeshController : MeshGenerator
{
    public Transform container;
    public Material material;
    private string url = "https://cysolutions.ca/temp/dataset.json";



     public void GenerateMesh(DataSet dataSet, bool isSaveToLocal)
    {
        // if (!IsTriangle(dataSet)) { return; }
       // IsTriangle(dataSet);
        foreach (Transform item in container)
        {
            Destroy(item.gameObject);
        }
        GameObject go = Generate(dataSet, material);
        if (go)
        {
            go.transform.parent = container;
            go.transform.localPosition = Vector3.zero;
           
            // store data in local
           if(isSaveToLocal) FileHandler.WriteDataset(dataSet);
        }
       
    }
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

    public void Restore( )
    {
        Vector3 point1 = new Vector3(0,0,0);
        Vector3 point2 = new Vector3(0,3,0);
        Vector3 point3 = new Vector3(3,0,0);
        
        DataSet defaultDataSet = new DataSet(point1, point2, point3);
        GenerateMesh(defaultDataSet, false);
    }

   
}
