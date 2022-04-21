using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileInteraction : MonoBehaviour
{
    public Grid grid;
    public static Tilemap tilemap;
    public static Tile hoveredTileRef;
    public static Tile unclickedTileRef;
    public static Tile clickedTileRef;
    public static Tile[] LevelTiles;
    public static Tile[] FlagTiles;
    public static Tile[] HoveredFlagTiles;

    Vector3 pos;
    Vector3Int hoveredTile;
    Vector3Int previousTile;
    public static Tile previousTileObj;

    private void Start()
    {
        //Set to a random tile because otherwise it messes up the tile at 0,0.
        previousTile = new Vector3Int(-100, -100, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!EscMenuToggle.isVisible)
        {
            pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //Remove z in case it matters
            pos = new Vector3(pos.x, pos.y);
            pos.x *= 1 / tilemap.transform.localScale.x;
            pos.y *= 1 / tilemap.transform.localScale.y;
            hoveredTile = grid.WorldToCell(pos);
            if (hoveredTile != previousTile)
                ChangeTile();
            previousTile = hoveredTile;
        }
    }

    void ChangeTile()
    {
        tilemap.SetTile(previousTile, previousTileObj);
        previousTileObj = tilemap.GetTile<Tile>(hoveredTile);
        //If tile is outside of game area, don't run the hover code.
        if (hoveredTile.x < MapData.minx || hoveredTile.x > MapData.maxx || hoveredTile.y < MapData.miny || hoveredTile.y > MapData.maxy)
            return;
        //tilemap.SetTile(hoveredTile, hoveredTileRef);
        if (previousTileObj.name.StartsWith("flag"))
            tilemap.SetTile(hoveredTile, HoveredFlagTiles[Int32.Parse(previousTileObj.name.Substring(4))]);
        else
            tilemap.SetTile(hoveredTile, HoveredFlagTiles[0]);
    }

    public static void ReloadTiles()
    {
        for(int w=0; w<PlayerPrefs.GetInt("width"); w++)
        {
            for(int h=0; h<PlayerPrefs.GetInt("height"); h++)
            {
                Tile t = (Tile)tilemap.GetTile(new Vector3Int(w, h, 0));
                if (t.name.StartsWith("level") && t.name.EndsWith("cb") != EscMenu.isCB)
                {
                    tilemap.SetTile(new Vector3Int(w, h, 0), LevelTiles[Int32.Parse(t.name.Substring(5, 1))]);
                }
            }
        }
    }
}
