using UnityEngine;
using System.Collections;

public class TileSelectable : MonoBehaviour
{
    public int tileX;
    public int tileY;
    public TileMap map;
    public bool occupied;

    //if this tile gets selected, move to this position
    void OnMouseUp()
    {
        Debug.Log("Click!" + tileX + " " + tileY);
        map.Destination(tileX, tileY);

    }

}
