using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialValues
{
    public static void InitializePrefs()
    {
        Debug.Log("Establishing intial PlayerPref info.");
        PlayerPrefs.SetInt("width", 10);
        PlayerPrefs.SetInt("height", 10);
        PlayerPrefs.SetInt("maxLevel", 5);
        PlayerPrefs.SetInt("currentLevel", 1);
        PlayerPrefs.SetInt("exp", 0);
        PlayerPrefs.SetInt("health", 10);
        PlayerPrefs.SetInt("score", 0);
        //Create file structure in user's mydocuments.
        PlayerPrefs.SetString("filePath", System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/dreamorduck/SweepRPG/Saves");
        System.IO.Directory.CreateDirectory(PlayerPrefs.GetString("filePath"));
        //Create achievement objects
        Achievements.InstantiateAchievements();
        //Create initial scores
        for(int i=0; i<10; i++)
        {
            PlayerPrefs.SetInt($"highscore{i}", 0);
        }
    }

    public static void InitializeGameStats()
    {
        Settings.health = PlayerPrefs.GetInt("health");
        Settings.level = PlayerPrefs.GetInt("currentLevel");
        Settings.exp = PlayerPrefs.GetInt("exp");
        Settings.score = PlayerPrefs.GetInt("score");
        Settings.time = 0;
    }
}
