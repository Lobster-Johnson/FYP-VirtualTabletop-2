using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TileMap : MonoBehaviour
{
    public TileType[] tileTypes;
    int[,] tiles;


    int mapsizeX = 10;
    int mapsizeY = 10;


    // Use this for initialization
    void Start ()
    {
        generateMapData();
        generatePathFindingGraph();
        generateMapVisuals();
    }

    //create tiles and assign tile types
    //hard coded at the moment
    void generateMapData()
    {
        //create a map of default tiles
        tiles = new int[mapsizeX, mapsizeY];
        for (int x = 0; x < mapsizeX; x++)
        {
            for (int y = 0; y < mapsizeY; y++)
            {
                tiles[x, y] = 0;
            }
        }

        //hardcoded: Remove
        //this could cause problems as all I'm doing here is created a new block on top of the old one
        //generate test wall
        tiles[0, 4] = 2;
        tiles[1, 4] = 2;
        tiles[2, 4] = 2;
        tiles[3, 4] = 2;
        tiles[4, 4] = 2;

        //generate test swamp
        tiles[9, 9] = 1;
        tiles[8, 9] = 1;
        tiles[9, 8] = 1;
        tiles[8, 8] = 1;
    }

    //generate graph
    void generatePathFindingGraph()
    {

    }

    void generateMapVisuals()
    {
        ////go to every tile on the map and make them selectable
        //for (int x = 0; x < mapsizeX; x++)
        //{
        //    for (int y = 0; y < mapsizeY; y++)
        //    {

        //        GameObject go = (GameObject)Instantiate((tileTypes[tiles[x, y]]).tileVisual, new Vector3(x, y, -1), Quaternion.identity);

        //        TileSelectable ct = go.GetComponent<TileSelectable>();
        //        ct.tileX = x;
        //        ct.tileY = y;
        //        ct.map = this;
        //    }
        //}
    }
}
