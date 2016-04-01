using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;

public class Creature : NetworkBehaviour
{
    //generic stuff for every creature, whether PC or enemy

    //every creature has a location on the map
    public int tileX;
    public int tileY;

    public TileMap wmap;

    //every creature has a path
    public List<Node> currentPath = null;

    //placeholder movespeed and bool to prevent movement
    float movespeed = 1;
    public bool InUse = true;

    // Use this for initialization
    void Start ()
    {
        //connect it to it's map
        GameObject[] Map = GameObject.FindGameObjectsWithTag("MainMap");
        wmap = Map[0].GetComponent<TileMap>();
        transform.position = new Vector3(tileX, tileY, -0.5f);
    }
	
	// Update is called once per frame
	void Update ()
    {
        tileX = (int)transform.position.x;
        tileY = (int)transform.position.y;

        if (currentPath != null)
        {
            if (wmap == null)
            {
                Debug.Log("Error, failed to connect to map");

            }
            else
            {
                int currnode = 0;
                //draw a line to the end in debug
                while (currnode < currentPath.Count - 1)
                {
                    Vector3 start = wmap.TileCoordToWorldCoord(currentPath[currnode].x,
                                                                currentPath[currnode].y
                                                                )
                                                                + new Vector3(0, 0, 0);
                    Vector3 end = wmap.TileCoordToWorldCoord(currentPath[currnode + 1].x,
                                                                currentPath[currnode + 1].y
                                                                )
                                                                + new Vector3(0, 0, 0);

                    Debug.DrawLine(start, end, Color.red);

                    currnode++;
                }
                //draw a box at the end
                Vector3 dest = wmap.TileCoordToWorldCoord(currentPath[currnode].x,
                                                                currentPath[currnode].y
                                                                )
                                                                + new Vector3(0, 0, 0);

            }
        }
    }

    //based on the creatures path, move it to this tile
    public void MoveNextTile()
    {
        Debug.Log("Movement command");
        if (InUse)
        {
            float remainingmovespeed = movespeed;
            while (remainingmovespeed > 0)
            {
                //if there's no path nothing happens
                if (currentPath == null)
                {
                    return;
                }
                //remove the old/current node from path
                currentPath.RemoveAt(0);

                //now grab the new first node and move us to that position
                tileX = currentPath[0].x;
                tileY = currentPath[0].y;
                transform.position = wmap.TileCoordToWorldCoord(currentPath[0].x, currentPath[0].y);
                transform.position += new Vector3(0, 0, -0.5f);

                remainingmovespeed--;

                if (currentPath.Count == 1)
                {
                    //we only have tile left in the path, that tile is the destination
                    //clear pathfinding info
                    currentPath = null;
                }
            }
        }
    }
}
