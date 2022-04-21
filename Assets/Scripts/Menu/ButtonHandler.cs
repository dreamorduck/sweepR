using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.IO;

public class ButtonHandler : MonoBehaviour
{
    public void ChangeScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void LoadGame()
    {
        string path = System.IO.Path.Combine(PlayerPrefs.GetString("filePath"), "save.json");
        if (!File.Exists(path))
            return;
        MapData data = new MapData();
        //MapData.LoadFromJSON(File.ReadAllText(path));
        UIUpdater.data = data;
        data.LoadFromJSON(File.ReadAllText(path));
        ChangeScene("game");
    }
}
