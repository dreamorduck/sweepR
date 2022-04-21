using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initializer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //InitialValues.InitializePrefs();
        if (PlayerPrefs.GetString("filePath") == "")
            InitialValues.InitializePrefs();
    }
}
