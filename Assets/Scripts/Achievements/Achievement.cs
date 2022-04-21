using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Achievement
{
    public int id;
    public string title;
    public string description;
    public bool isComplete;
    public string img;

    public Achievement()
    {
        isComplete = false;
    }
}
