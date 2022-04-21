using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EscMenu : MonoBehaviour
{
    public static bool isIngame;
    bool isVisible;
    public Canvas canvas;
    public TMP_Dropdown windowSizeDropdown;
    public Toggle windowedToggle;
    public Toggle fullscreenToggle;
    public Toggle borderlessToggle;
    public Toggle cbToggle;
    public static bool isCB;
    bool shouldToggle;

    // Start is called before the first frame update
    void Start()
    {
        shouldToggle = false;
        if (PlayerPrefs.GetString("fullscreenmode") == "windowed")
            windowedToggle.isOn = true;
        else if (PlayerPrefs.GetString("fullscreenmode") == "fullscreen")
            fullscreenToggle.isOn = true;
        else if (PlayerPrefs.GetString("fullscreenmode") == "borderless")
            borderlessToggle.isOn = true;
        if (PlayerPrefs.GetInt("height") == 1080)
            windowSizeDropdown.value = 0;
        else if (PlayerPrefs.GetInt("height") == 900)
            windowSizeDropdown.value = 1;
        else if (PlayerPrefs.GetInt("height") == 768)
            windowSizeDropdown.value = 2;
        else if (PlayerPrefs.GetInt("height") == 720)
            windowSizeDropdown.value = 3;
        else if (PlayerPrefs.GetInt("height") == 664)
            windowSizeDropdown.value = 4;
        if (PlayerPrefs.GetInt("colorblind") == 0)
            cbToggle.isOn = false;
        else
            cbToggle.isOn = true;
        shouldToggle = true;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetWindowSize(string input)
    {
        if (!shouldToggle)
            return;
        FullScreenMode fullscreenmode;
        if (input == "windowed")
            fullscreenmode = FullScreenMode.Windowed;
        else if (input == "fullscreen")
            fullscreenmode = FullScreenMode.ExclusiveFullScreen;
        else
            fullscreenmode = FullScreenMode.FullScreenWindow;
        Screen.SetResolution(PlayerPrefs.GetInt("width"), PlayerPrefs.GetInt("height"), fullscreenmode);
        PlayerPrefs.SetString("fullscreenmode", input);
    }

    public void SetResolution(int w, int h)
    {
        if (!shouldToggle)
            return;
        Screen.SetResolution(w, h, Screen.fullScreenMode);
        PlayerPrefs.SetInt("width", w);
        PlayerPrefs.SetInt("height", h);
    }

    public void SetResolution()
    {
        string input = windowSizeDropdown.options[windowSizeDropdown.value].text;
        if (input == "1920x1080")
            SetResolution(1920, 1080);
        else if (input == "1600x900")
            SetResolution(1600, 900);
        else if (input == "1366x768")
            SetResolution(1366, 768);
        else if (input == "1280x720")
            SetResolution(1280, 720);
        else if (input == "1176x664")
            SetResolution(1176, 664);
    }

    public void SetCB()
    {
        isCB = cbToggle.isOn;
        if (!shouldToggle)
            return;
        Debug.Log(isCB);
        int i;
        if (isCB)
            i = 1;
        else
            i = 0;
        PlayerPrefs.SetInt("colorblind", i);
        Generation.LoadResources();
        if (isIngame)
            TileInteraction.ReloadTiles();
    }
}
