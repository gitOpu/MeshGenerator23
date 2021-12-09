using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Networking;
using UnityEngine.UI;

public static class GetWebRequest 
{
    public static IEnumerator GetRequest(string uri , Action<bool, DataSet> callback)
    {

        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            DataSet dataSet = new DataSet(Vector3.zero, Vector3.zero,Vector3.zero);

            yield return webRequest.SendWebRequest();

            string jsonForm = uri;

            if (webRequest.isNetworkError)
            {
                Debug.Log("Error loading");
                callback(false, dataSet);
            }
            else
            {
                try
                {
                     dataSet = JsonUtility.FromJson<DataSet>(webRequest.downloadHandler.text);
                    Debug.Log("Success " + dataSet.point1 +"; "+ dataSet.point2 +"; "+ dataSet.point3);
                    callback(true, dataSet);
                }
                catch
                {
                    Debug.Log("Error in connection");
                    callback(false, dataSet);
                }
            }

        }
    }
}