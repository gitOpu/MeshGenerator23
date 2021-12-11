using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Linq;

public static class FileHandler
{


    public static void SaveToJson<T>(List<T> toSave, string fileName, Action<float, bool> callback)
    {
        string content = JsonHelper.ToJson<T>(toSave.ToArray());
        //Debug.Log("Path " + GetPath(fileName));
        WriteFile(GetPath(fileName), content, callback);
    }
    public static List<T> ReadFromJson<T>(string fileName)
    {
        string content = ReadFile(GetPath(fileName));
        if (string.IsNullOrEmpty(content) || content == "{}")
        {
            return new List<T>();
        }
        List<T> res = JsonHelper.FromJson<T>(content).ToList();
        return res;

    }
    private static string GetPath(string fileName)
    {
        return Application.streamingAssetsPath + "/" + fileName;
        // return Application.persistentDataPath+"/"+fileName;
    }
    private static void WriteFile(string path, string content, Action<float, bool> callback)
    {
        FileStream fileStream = new FileStream(path, FileMode.Create);
        using (StreamWriter writer = new StreamWriter(fileStream))
        {
            writer.Write(content);
            callback(100, true);
        }

    }
    private static string ReadFile(string path)
    {
        if (File.Exists(path))
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string content = reader.ReadToEnd();
                return content;
            }
        }
        else return "";
    }

}
// covert objects list to array
public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}
