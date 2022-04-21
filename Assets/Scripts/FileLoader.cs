using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;

public class FileLoader : MonoBehaviour
{
    private static Canvas canvas;
    private static GameObject objRef = (GameObject)Resources.Load("prefabs/tileText");
    public static void CreateTileText(Vector3 position, string s, bool isVisible, bool addToSave)
    {
        GameObject tobj = GameObject.Instantiate(objRef, canvas.transform);
        TMP_Text t = tobj.GetComponentInChildren<TMP_Text>();
        t.transform.position = position;
        t.text = s;
        if (isVisible)
            t.alpha = 1;
        else
            t.alpha = 0;
        TileContainer.Add(t, position);
        if (addToSave)
            TileContainer.AddToSave(t, position, isVisible);
    }

    public static void SetCanvas(Canvas input)
    {
        canvas = input;
    }
}
