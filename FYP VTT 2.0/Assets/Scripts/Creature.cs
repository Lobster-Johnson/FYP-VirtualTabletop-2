﻿using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;

struct CreatureState
{
    public int stilex;
    public int stiley;
}

public class Creature : NetworkBehaviour
{
    //generic stuff for every creature, whether PC or enemy
    [SyncVar]
    CreatureState serverState;

    [SyncVar]
    public string pname;

    //every creature has a location on the map
    public int tileX;
    public int tileY;

    public TileMap wmap;

    //every creature has a path
    public List<Node> currentPath = null;

    //placeholder movespeed and bool to prevent movement
    float movespeed = 1;

    //turn management bools
    public bool MyTurn = true;
    public bool TurnFinished = false;

    void Awake()
    {
        InitState();
    }

    [Server]
    void InitState()
    {
        //if (!isServer)
        //{
        //    return;
        //}

        serverState = new CreatureState
        {
            stilex = tileX,
            stiley = tileY
        };
    }

    public void forcespawn(int x, int y)
    {
        tileX = x;
        tileY = y;
        NewServerPos(tileX, tileY);
    }

    // Use this for initialization
    void Start ()
    {
        //connect it to it's map
        GameObject[] Map = GameObject.FindGameObjectsWithTag("MainMap");
        wmap = Map[0].GetComponent<TileMap>();
        transform.position = new Vector3(tileX, tileY, -1f);
        UpdateOnServer();
    }
	


	// Update is called once per frame
	void Update ()
    {
        //change it's tile x and y to it's current position in the world
        tileX = (int)transform.position.x;
        tileY = (int)transform.position.y;

        //name
        if(isLocalPlayer)
        {
            pname = "You";
        }

        DebugLine();
        UpdateOnServer();
    }

    //draw a line in debug mode to make sure pathfinding works
    void DebugLine()
    {
        //draw a debug line
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

            }
        }
    }

    //based on the creatures path, move it to this tile
    public void CmdMoveNextTile()
    {
        Debug.Log("Movement command recieved");
        if (isLocalPlayer)
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

                //now grab the new first node, set the tileX and tileY values to it and move us there
                tileX = currentPath[0].x;
                tileY = currentPath[0].y;
                transform.position = wmap.TileCoordToWorldCoord(currentPath[0].x, currentPath[0].y);
                transform.position += new Vector3(0, 0, -1f);

                remainingmovespeed--;

                //server update
                CmdNewServerPos(currentPath[0].x, currentPath[0].y);

                if (currentPath.Count == 1)
                {
                    //we only have tile left in the path, that tile is the destination
                    //clear pathfinding info
                    currentPath = null;
                }
            }
        }
    }

    //command to end the creatures turn
    public void EndTurn()
    {
        if (MyTurn && isLocalPlayer)
        {
            Debug.Log("End turn command recieved");
            MyTurn = false;
            currentPath = null;
            TurnFinished = true;
        }

    }

    //the following 3 functions are to move it on the server side. 
    //That is, when a local player moves a creature, it updates on everyone else's screen
    [Command]
    void CmdNewServerPos(int x, int y)
    {
        Debug.Log("Requesting update on server");
        serverState = Move(serverState, x, y);
    }

    [Server]
    void NewServerPos(int x, int y)
    {
        Debug.Log("Requesting update on server");
        serverState = Move(serverState, x, y);
    }

    CreatureState Move(CreatureState previous, int x, int y)
    {
        return new CreatureState
        {
            stilex = x,
            stiley = y
        };
    }

    void UpdateOnServer()
    {
        transform.position = serverState.stilex * Vector3.right + serverState.stiley * Vector3.up;
        transform.position = new Vector3(transform.position.x, transform.position.y, -0.5f);
        //Debug.Log("Server updated, Co-ordinates: " + serverState.stilex + " " + serverState.stiley);
        this.gameObject.GetComponent<Stats>().updatehealth();
    }

    
}
