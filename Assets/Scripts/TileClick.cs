using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;
using System;

public class TileClick : MonoBehaviour
{
    public Tilemap tilemap;
    public Grid grid;
    public Tile clickedTileRef;
    public Generation generator;

    Vector3Int clickedTile;
    int level;
    bool isFirstClick = true;

    MapData data = new MapData();

    // Start is called before the first frame update
    void Start()
    {
        if (MapData.isLoad)
            isFirstClick = false;
        TileInteraction.tilemap = tilemap;
    }

    // Update is called once per frame
    void Update()
    {
        if (!EscMenuToggle.isVisible)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                pos.x *= 1 / tilemap.transform.localScale.x;
                pos.y *= 1 / tilemap.transform.localScale.y;
                HandleClick(grid.WorldToCell(pos));
            }
            if (Input.GetMouseButtonDown(1))
            {
                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                pos.x *= 1 / tilemap.transform.localScale.x;
                pos.y *= 1 / tilemap.transform.localScale.y;
                HandleFlag(grid.WorldToCell(pos));
            }
        }
    }

    void HandleClick(Vector3Int input)
    {
        //If outside the bounds of the board, cancel the function.
        if (input.x < MapData.minx || input.x > MapData.maxx || input.y < MapData.miny || input.y > MapData.maxy)
            return;

        //If this is the first tile to be clicked, run the second half of generation
        if(isFirstClick)
        {
            generator.PopulateTiles(input);
            isFirstClick = false;
        }

        //If tile has already been clicked, run alternative function.
        if (IsClicked())
        {
            /*Vector3 pos = grid.CellToWorld(input);
            pos.x += .6f;
            pos.y += .6f;
            TMP_Text t = TileContainer.Find(pos);
            if (t == null)
                return;
            if (t.alpha == 1)
            {
                t.alpha = 0;
                MapData.SetAlpha(input, false);
            }
            else
            {
                t.alpha = 1;
                MapData.SetAlpha(input, true);
            }*/
        }
        else
        {
            //level = MapData.levels[input.x][input.y];
            level = MapData.FindLevel(input.x, input.y);
            tilemap.SetTile(input, GetTile());
            TileInteraction.previousTileObj = GetTile();

            //Combat logic
            if (level > 0)
            {
                RunCombat();
                Vector3 pos = grid.CellToWorld(input);
                pos.x += .6f;
                pos.y += .6f;
                TMP_Text t = TileContainer.Find(pos);
                t.alpha = 1;
            }
            else
            {
                Vector3 pos = grid.CellToWorld(input);
                pos.x += .6f;
                pos.y += .6f;
                TMP_Text t = TileContainer.Find(pos);
                t.alpha = 1;
                MapData.SetAlpha(input, true);
                if (Int32.Parse(t.text) == 0)
                {
                    //If level is 0, automatically click neighbors as well
                    NeighborClick(new Vector3Int(input.x - 1, input.y - 1, 0));
                    NeighborClick(new Vector3Int(input.x, input.y - 1, 0));
                    NeighborClick(new Vector3Int(input.x + 1, input.y - 1, 0));
                    NeighborClick(new Vector3Int(input.x - 1, input.y, 0));
                    NeighborClick(new Vector3Int(input.x + 1, input.y, 0));
                    NeighborClick(new Vector3Int(input.x - 1, input.y + 1, 0));
                    NeighborClick(new Vector3Int(input.x, input.y + 1, 0));
                    NeighborClick(new Vector3Int(input.x + 1, input.y + 1, 0));
                }
            }
        }

        MapData.clearedTiles.Add(new Vector3Int(input.x, input.y, 0));

        //Save the game
        Debug.Log("Saving game.");
        data.SaveToJSON();
    }

    void NeighborClick(Vector3Int input)
    {
        //If outside the bounds of the board, cancel the function.
        if (input.x < MapData.minx || input.x > MapData.maxx || input.y < MapData.miny || input.y > MapData.maxy)
            return;

        //If tile has already been clicked, run alternative function.
        if(IsClicked(input))
        {
            Vector3 pos = grid.CellToWorld(input);
            pos.x += .6f;
            pos.y += .6f;
            TMP_Text t = TileContainer.Find(pos);
            if (t == null)
                return;
            t.alpha = 1;
            MapData.SetAlpha(input, true);
        }
        else
        {
            //level = MapData.levels[input.x][input.y];
            level = MapData.FindLevel(input.x, input.y);
            tilemap.SetTile(input, GetTile());
            TileInteraction.previousTileObj = GetTile();

            //Combat logic
            if (level > 0)
                RunCombat();
            else
            {
                Vector3 pos = grid.CellToWorld(input);
                pos.x += .6f;
                pos.y += .6f;
                TMP_Text t = TileContainer.Find(pos);
                t.alpha = 1;
                MapData.SetAlpha(input, true);
                if (Int32.Parse(t.text) == 0)
                {
                    //If level is 0, automatically click neighbors as well
                    NeighborClick(new Vector3Int(input.x - 1, input.y - 1, 0));
                    NeighborClick(new Vector3Int(input.x, input.y - 1, 0));
                    NeighborClick(new Vector3Int(input.x + 1, input.y - 1, 0));
                    NeighborClick(new Vector3Int(input.x - 1, input.y, 0));
                    NeighborClick(new Vector3Int(input.x + 1, input.y, 0));
                    NeighborClick(new Vector3Int(input.x - 1, input.y + 1, 0));
                    NeighborClick(new Vector3Int(input.x, input.y + 1, 0));
                    NeighborClick(new Vector3Int(input.x + 1, input.y + 1, 0));
                }
            }
        }

        MapData.clearedTiles.Add(new Vector3Int(input.x, input.y, 0));
    }

    Tile GetTile()
    {
        return TileInteraction.LevelTiles[level];
    }

    void RunCombat()
    {
        //If player is equal or higher level, no damage is dealt.
        //2 damage per level difference
        if (!(Settings.level >= level))
            Settings.health -= (level - Settings.level) * 2;
        //Add enemy level to exp
        Settings.exp += level;
        Settings.score += level * 100;
    }

    void HandleFlag(Vector3Int input)
    {
        //If outside the bounds of the board, cancel the function.
        if (input.x < MapData.minx || input.x > MapData.maxx || input.y < MapData.miny || input.y > MapData.maxy)
            return;

        //If this is before the first click, cancel the flag.
        if (isFirstClick)
            return;

        //If this tile has already been cleared, cancel the flag.
        if (IsClicked())
            return;

        for(int i=5; i>=0; i--)
        {
            if(TileInteraction.previousTileObj == TileInteraction.FlagTiles[i])
            {
                if (i == 5)
                    tilemap.SetTile(input, TileInteraction.FlagTiles[0]);
                else
                    tilemap.SetTile(input, TileInteraction.FlagTiles[i + 1]);
            }
        }
        TileInteraction.previousTileObj = tilemap.GetTile<Tile>(input);
    }

    bool IsClicked()
    {
        if (TileInteraction.previousTileObj == TileInteraction.unclickedTileRef)
            return false;
        for(int i=0; i<6; i++)
        {
            if (TileInteraction.previousTileObj == TileInteraction.FlagTiles[i])
                return false;
        }
        return true;
    }

    bool IsClicked(Vector3Int input)
    {
        if (tilemap.GetTile<Tile>(input) == TileInteraction.unclickedTileRef)
            return false;
        for(int i=0; i<6; i++)
        {
            if (tilemap.GetTile<Tile>(input) == TileInteraction.FlagTiles[i])
                return false;
        }
        return true;
    }
}
