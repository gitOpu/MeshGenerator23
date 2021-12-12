using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;





[Serializable]
public class DataSet
{
    public List<Vector3> list;
}

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
