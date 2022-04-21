using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Tilemaps;

public class Generation : MonoBehaviour
{
    public Tilemap tilemap;

    int w;
    int h;
    public static int maxLevel;
    List<int> levelsBag;

    public Tile unclickedTile;
    public Grid grid;
    public Canvas tileCanvas;

    // Start is called before the first frame update
    void Start()
    {
        EscMenu.isIngame = true;
        w = PlayerPrefs.GetInt("width");
        h = PlayerPrefs.GetInt("height");
        maxLevel = PlayerPrefs.GetInt("maxLevel");
        
        //Flush old contents out
        tilemap.ClearAllTiles();
        TileContainer.Flush();

        Camera.main.transform.position = new Vector3(w / 2, h / 2, Camera.main.transform.position.z);
        MapData.minx = 0;
        MapData.miny = 0;
        MapData.grid = grid;
        if (!MapData.isLoad)
        {
            MapData.levels = new List<Vector3Int>();
            MapData.clearedTiles = new List<Vector3Int>();
            MapData.textStrings = new List<string>();
            MapData.textPositions = new List<Vector3>();
            MapData.textAlphas = new List<bool>();
        }

        levelsBag = GenerateGrabBag();

        for (int i = 0; i < w; i++)
        {
            if (!MapData.isLoad)
            {
                //MapData.levels[i] = new int[h];
                //MapData.clearedTiles[i] = new int[h];
            }
            for(int n = 0; n < h; n++)
            {
                tilemap.SetTile(new Vector3Int(i, n, 0), unclickedTile);
            }
        }

        MapData.maxx = w - 1;
        MapData.maxy = h - 1;
        LoadResources();
        if (MapData.isLoad)
            LoadGame();
        else
            InitialValues.InitializeGameStats();
    }

    public static void LoadResources()
    {
        TileInteraction.unclickedTileRef = (Tile)Resources.Load("Tile/unclicked");
        TileInteraction.clickedTileRef = (Tile)Resources.Load("Tile/clicked");
        TileInteraction.hoveredTileRef = (Tile)Resources.Load("Tile/hovered");

        TileInteraction.LevelTiles = new Tile[maxLevel + 1];
        TileInteraction.FlagTiles = new Tile[maxLevel + 1];
        TileInteraction.HoveredFlagTiles = new Tile[maxLevel + 1];
        for (int i = 0; i < maxLevel + 1; i++)
        {
            if (EscMenu.isCB)
            {
                TileInteraction.LevelTiles[i] = (Tile)Resources.Load($"Tile/level{i}cb");
            }
            else
            {
                TileInteraction.LevelTiles[i] = (Tile)Resources.Load($"Tile/level{i}");
            }
            TileInteraction.FlagTiles[i] = (Tile)Resources.Load($"Tile/flag{i}");
            TileInteraction.HoveredFlagTiles[i] = (Tile)Resources.Load($"Tile/hovered{i}");
        }
        TileInteraction.FlagTiles[0] = TileInteraction.unclickedTileRef;
    }

    List<int> GenerateGrabBag()
    {
        /*
         * Create grab bag of levels
         * Odds for each level:
         * level 5: 1%
         * level 4: 2%
         * level 3: 4%
         * level 2: 6%
         * level 1: 12%
         * level 0: 75%
         */
        int totalTiles = w * h;
        List<int> levelBag = new List<int>();
        List<int> expCurve = new List<int>();
        int index = 0;

        for(int i=0; i<5; i++)
            expCurve.Add(0);
        for(int i=0; i<(w*h)*.01; i++)
        {
            levelBag.Add(5);
            index++;
            expCurve[4] += 5;
        }
        for(int i=0; i<(w*h)*.02; i++)
        {
            levelBag.Add(4);
            index++;
            expCurve[3] += 4;
        }
        for(int i=0; i<(w*h)*.04; i++)
        {
            levelBag.Add(3);
            index++;
            expCurve[2] += 3;
        }
        for(int i=0; i<(w*h)*.06; i++)
        {
            levelBag.Add(2);
            index++;
            expCurve[1] += 2;
        }
        for(int i=0; i<(w*h)*.12; i++)
        {
            levelBag.Add(1);
            index++;
            expCurve[0] += 1;
        }
        //Remove 9 level 0s because they will be manually put into the grid later.
        //Does not include the probability for level 0 because it just fills up to the remaining space in the bag.
        for(int i=index; i<(w*h) - 9; i++)
        {
            levelBag.Add(0);
        }

        Settings.expCurve = expCurve;
        return levelBag;
    }

    public void PopulateTiles(Vector3Int start)
    {
        for(int i=0; i<w; i++)
        {
            for(int n=0; n<h; n++)
            {
                //If the tile is the starting tile or one of its neighbors, make it level 0.
                if ((i >= start.x - 1 && i <= start.x + 1) && (n >= start.y - 1 && n <= start.y + 1))
                    //MapData.levels[i][n] = 0;
                    MapData.levels.Add(new Vector3Int(i, n, 0));
                else
                {
                    int levelIndex = Random.Range(0, levelsBag.Count);
                    //MapData.levels[i][n] = levelsBag[levelIndex];
                    MapData.levels.Add(new Vector3Int(i, n, levelsBag[levelIndex]));
                    levelsBag.RemoveAt(levelIndex);
                }
            }
        }

        //Populate TileContainer with neighbor counts
        FileLoader.SetCanvas(tileCanvas);
        for (int i = 0; i < w; i++)
        {
            for (int n = 0; n < h; n++)
            {
                int num = 0;
                //Tally up neighbor levels
                if (n > 0)
                {
                    if (i > 0)
                        num += MapData.FindLevel(i - 1, n - 1);
                    //num += MapData.levels[i - 1][n - 1];
                    if (i < w - 1)
                        num += MapData.FindLevel(i + 1, n - 1);
                    //num += MapData.levels[i + 1][n - 1];
                    num += MapData.FindLevel(i, n - 1);
                    //num += MapData.levels[i][n - 1];
                }
                if (i > 0)
                    num += MapData.FindLevel(i - 1, n);
                //num += MapData.levels[i - 1][n];
                if (i < w - 1)
                    num += MapData.FindLevel(i + 1, n);
                //num += MapData.levels[i + 1][n];
                if (n < h - 1)
                {
                    if (i > 0)
                        num += MapData.FindLevel(i - 1, n + 1);
                    //num += MapData.levels[i - 1][n + 1];
                    if (i < w - 1)
                        num += MapData.FindLevel(i + 1, n + 1);
                    //num += MapData.levels[i + 1][n + 1];
                    //num += MapData.levels[i][n + 1];
                    num += MapData.FindLevel(i, n + 1);
                }
                Vector3 pos = grid.CellToWorld(new Vector3Int(i, n, 0));
                pos.x += .6f;
                pos.y += .6f;
                FileLoader.CreateTileText(pos, num.ToString(), false, true);
            }
        }
    }

    public void LoadGame()
    {
        Debug.Log("Loading game...");
        for(int i=0; i<MapData.clearedTiles.Count; i++)
        {
            tilemap.SetTile(new Vector3Int(MapData.clearedTiles[i].x, MapData.clearedTiles[i].y, 0), TileInteraction.LevelTiles[MapData.FindLevel(MapData.clearedTiles[i].x, MapData.clearedTiles[i].y)]);
        }
        FileLoader.SetCanvas(tileCanvas);
        for(int i=0; i<MapData.textStrings.Count; i++)
        {
            FileLoader.CreateTileText(MapData.textPositions[i], MapData.textStrings[i], MapData.textAlphas[i], false);
        }
    }
}
