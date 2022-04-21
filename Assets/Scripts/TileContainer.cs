using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class TileContainer : MonoBehaviour
{
    public static List<TMP_Text> objs = new List<TMP_Text>();
    public static List<Vector3> pos = new List<Vector3>();

    public static void Add(TMP_Text text, Vector3 position)
    {
        objs.Add(text);
        pos.Add(position);
    }

    public static void AddToSave(TMP_Text text, Vector3 position, bool isVisible)
    {
        MapData.textStrings.Add(text.text);
        MapData.textPositions.Add(position);
        MapData.textAlphas.Add(isVisible);
    }

    public static void Remove(Vector3 position)
    {
        for (int i = 0; i < objs.Count; i++)
        {
            if (position.x == pos[i].x && position.y == pos[i].y)
            {
                objs.RemoveAt(i);
                pos.RemoveAt(i);
                Debug.Log($"Found text object at {position}.");
                return;
            }
        }
        Debug.Log($"Could not find text object at {position}.");
    }

    public static void Flush()
    {
        objs = new List<TMP_Text>();
        pos = new List<Vector3>();
    }

    public static TMP_Text Find(Vector3 position)
    {
        for(int i=0; i<objs.Count; i++)
        {
            if (position.x == pos[i].x && position.y == pos[i].y)
                return objs[i];
        }
        Debug.Log($"Could not find text object at {position}.");
        return null;
    }
}
