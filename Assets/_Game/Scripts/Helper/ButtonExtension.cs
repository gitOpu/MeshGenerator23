using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public static class ButtonExtension
{
    public static void AddEventListener<T>(this Button buttons, T param, Action<T> onClick)
    {
        buttons.onClick.AddListener(delegate ()
        {
            onClick(param);
        });
    }


}
