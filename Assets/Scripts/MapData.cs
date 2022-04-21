using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using System.IO;
using TMPro;
using UnityEngine.SceneManagement;

public class MapData
{
    public static bool isLoad = false;
    public static Grid grid;

    public static List<Vector3Int> levels;
    public static int minx;
    public static int maxx;
    public static int miny;
    public static int maxy;

    public static List<Vector3Int> clearedTiles;

    public static List<string> textStrings;
    public static List<Vector3> textPositions;
    public static List<bool> textAlphas;

    public MapStruct loadedStruct;
    [Serializable]
    public struct MapStruct
    {
        public List<Vector3Int> slevels;
        public List<Vector3Int> sclearedTiles;
        public List<string> stextStrings;
        public List<Vector3> stextPositions;
        public List<bool> stextAlphas;
        public int exp;
        public List<int> expCurve;
        public int health;
        public int level;
        public int score;
        public float time;
    }

    public void ReadStruct()
    {
        levels = loadedStruct.slevels;
        clearedTiles = loadedStruct.sclearedTiles;
        textStrings = loadedStruct.stextStrings;
        textPositions = loadedStruct.stextPositions;
        textAlphas = loadedStruct.stextAlphas;
    }
    
    public void FinishReadStruct()
    {
        Settings.level = loadedStruct.level;
        Settings.expCurve = loadedStruct.expCurve;
        Settings.exp = loadedStruct.exp;
        Settings.health = loadedStruct.health;
        Settings.score = loadedStruct.score;
        Settings.time = loadedStruct.time;
    }

    public void SaveToJSON()
    {
        //Copy data into the loadedStruct object to prepare for json conversion.
        loadedStruct.slevels = levels;
        loadedStruct.sclearedTiles = clearedTiles;
        loadedStruct.stextStrings = textStrings;
        loadedStruct.stextPositions = textPositions;
        loadedStruct.stextAlphas = textAlphas;
        loadedStruct.level = Settings.level;
        loadedStruct.expCurve = Settings.expCurve;
        loadedStruct.exp = Settings.exp;
        loadedStruct.health = Settings.health;
        loadedStruct.score = Settings.score;
        loadedStruct.time = Settings.time;

        //Convert loadedStruct to json and return the result.
        string json = JsonUtility.ToJson(loadedStruct);
        string path = Path.Combine(PlayerPrefs.GetString("filePath"), "save.json");
        File.WriteAllText(path, json);
    }

    public void LoadFromJSON(string input)
    {
        if (input == "")
            return;

        loadedStruct = JsonUtility.FromJson<MapStruct>(input);
        isLoad = true;
        ReadStruct();
    }

    public static int FindLevel(int x, int y)
    {
        for(int i=0; i<levels.Count; i++)
        {
            if (x == levels[i].x && y == levels[i].y)
                return levels[i].z;
        }
        Debug.Log($"Could not find level data for {x}, {y}.");
        return 0;
    }

    public static void SetAlpha(Vector3 input, bool isVisible)
    {
        for(int i=0; i<textPositions.Count; i++)
        {
            Vector3Int pos = grid.WorldToCell(textPositions[i]);
            if (pos.x == input.x && pos.y == input.y)
            {
                textAlphas[i] = isVisible;
                return;
            }
        }
        Debug.Log($"Could not find text at {input.x}, {input.y}.");
    }
}
