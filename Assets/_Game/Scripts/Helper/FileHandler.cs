using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Linq;


public static class FileHandler 
{
    public static void ReadDataset(Action<bool, DataSet> callback)
    {
        DataSet dataSet = new DataSet(Vector3.zero, Vector3.zero, Vector3.zero);
        string path = GetPath();

        if (File.Exists(path))
        {
            string fileContents = File.ReadAllText(path);
            dataSet = JsonUtility.FromJson<DataSet>(fileContents);
            // Debug.Log($"Read Successfull {dataset.ToString()}");
            callback(true, dataSet);
        }
        else
        {
            Debug.Log("Empty");
            callback(false, dataSet);
        }

    }
    public static void WriteDataset(DataSet dataset)
    {
        string jsonDataset = JsonUtility.ToJson(dataset);
        File.WriteAllText(GetPath(), jsonDataset);
       // Debug.Log($"Write Successfull ");

    }

    private static string GetPath()
    {
        return Application.streamingAssetsPath + "/dataset.json";
        // return Application.persistentDataPath + "/dataset.json";
    }

}