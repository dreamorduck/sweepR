using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launch : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(PlayerPrefs.GetString("fullscreenmode"));
        Debug.Log(PlayerPrefs.GetInt("width"));
        Debug.Log(PlayerPrefs.GetInt("height"));
        if (PlayerPrefs.GetString("fullscreenmode") == "")
        {
            PlayerPrefs.SetString("fullscreenmode", "windowed");
            PlayerPrefs.SetInt("width", 1300);
            PlayerPrefs.SetInt("height", 900);
        }
        string input = PlayerPrefs.GetString("fullscreenmode");
        FullScreenMode fullscreenmode;
        if (input == "windowed")
            fullscreenmode = FullScreenMode.Windowed;
        else if (input == "fullscreen")
            fullscreenmode = FullScreenMode.ExclusiveFullScreen;
        else
            fullscreenmode = FullScreenMode.FullScreenWindow;
        if (PlayerPrefs.GetInt("width") < 1176)
            PlayerPrefs.SetInt("width", 1300);
        if (PlayerPrefs.GetInt("height") < 664)
            PlayerPrefs.SetInt("height", 900);
        Screen.SetResolution(PlayerPrefs.GetInt("width"), PlayerPrefs.GetInt("height"), fullscreenmode);
        //Screen.SetResolution(1920, 1080, FullScreenMode.Windowed);
        if (PlayerPrefs.GetInt("colorblind") == 0)
            EscMenu.isCB = false;
        else
            EscMenu.isCB = true;
        EscMenu.isIngame = false;

        //This should be removed in engine rewrite
        Generation.maxLevel = 5;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
