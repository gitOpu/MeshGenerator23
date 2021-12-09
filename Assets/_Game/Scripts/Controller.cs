//using Assets.Scripts.Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public static Controller self;

    public MeshController meshController;
    public NavigationController navigationController;
    private void Awake()
    {
        if (self == null)
        {
            self = this;
        }
    }


}
