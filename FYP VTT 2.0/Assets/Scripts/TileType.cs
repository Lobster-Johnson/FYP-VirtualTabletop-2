using UnityEngine;
using System.Collections;

[System.Serializable]
public class TileType
{
    //holds all the properties of this tile
    public string name;
    public GameObject tileVisual;
    //currently not in use
    public bool impassible;
    public float movementCost = 1;


}
