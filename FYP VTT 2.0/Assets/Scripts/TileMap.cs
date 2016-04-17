using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Networking;

public class TileMap : MonoBehaviour
{
    //tile info
    public TileType[] tileTypes;
    int[,] tiles;
    
    public GameObject creature;

    //map size
    int mapsizeX = 10;
    int mapsizeY = 10;

    //graph and nodes
    List<Node> currentPath = null;
    Node[,] graph;


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
        tiles[9, 3] = 1;
        tiles[8, 3] = 1;
        tiles[7, 3] = 1;
        tiles[9, 2] = 1;
        tiles[8, 2] = 1;
        tiles[7, 2] = 1;
        Debug.Log("Map Data Generated");
    }

    //generate graph
    void generatePathFindingGraph()
    {
        graph = new Node[mapsizeX, mapsizeY];
        for (int x = 0; x < mapsizeX; x++)
        {
            for (int y = 0; y < mapsizeY; y++)
            {
                graph[x, y] = new Node();
                graph[x, y].x = x;
                graph[x, y].y = y;
            }
        }

        for (int x = 0; x < mapsizeX; x++)
        {
            for (int y = 0; y < mapsizeY; y++)
            {
                //add all neightbours for each node
                //need to add diagonals
                //left neightbour
                if (x > 0)
                {
                    graph[x, y].neighbours.Add(graph[x - 1, y]);
                    //diagnols
                    if (y < mapsizeY - 1)
                    {
                        graph[x, y].neighbours.Add(graph[x - 1, y + 1]);
                    }
                    if (y > 0)
                    {
                        graph[x, y].neighbours.Add(graph[x - 1, y - 1]);
                    }
                }
                //right neighbour
                if (x < mapsizeX - 1)
                {
                    graph[x, y].neighbours.Add(graph[x + 1, y]);
                    //diagnols
                    if (y < mapsizeY - 1)
                    {
                        graph[x, y].neighbours.Add(graph[x + 1, y + 1]);
                    }
                    if (y > 0)
                    {
                        graph[x, y].neighbours.Add(graph[x + 1, y - 1]);
                    }
                }
                //bottom neightbour
                if (y > 0)
                {
                    graph[x, y].neighbours.Add(graph[x, y - 1]);
                }
                //top neighbour
                if (y < mapsizeY - 1)
                {
                    graph[x, y].neighbours.Add(graph[x, y + 1]);
                }


            }
        }
        Debug.Log("Graph Generated");
    }

    //instantiate all the tiles based on the map data
    void generateMapVisuals()
    {
        //go to every tile on the map and make them selectable
        for (int x = 0; x < mapsizeX; x++)
        {
            for (int y = 0; y < mapsizeY; y++)
            {

                GameObject go = (GameObject)Instantiate((tileTypes[tiles[x, y]]).tileVisual, new Vector3(x, y, -1), Quaternion.identity);

                TileSelectable ct = go.GetComponent<TileSelectable>();
                ct.tileX = x;
                ct.tileY = y;
                ct.map = this;
            }
        }
        Debug.Log("Map Generated");
    }




    //move cost to enter a tile for pathing purposes
    float CostToEnterTile(int sourceX, int sourceY, int targetX, int targetY)
    {
        TileType tt = tileTypes[tiles[targetX, targetY]];
        float cost = tt.movementCost;
        if (sourceX != targetX && sourceY != targetY)
        {
            //we are moving diagonally
            cost += 0.00001f;
        }
        return cost;
    }

    //tile location -> world location
    public Vector3 TileCoordToWorldCoord(int x, int y)
    {
        return new Vector3(x, y, 0);
    }


    //get passed in a creature from the turn manager
    public void LoadInCreature(GameObject cr)
    {
        creature = null;
        creature = cr;
    }

    //pathfinding
    //path creature to this location
    public void Destination(int x, int y)
    {
        //if it's no one's turn
        if(creature == null)
        {
            Debug.Log("ERROR no current player*");
            return;
        }

        Debug.Log("Got one");

        //clear out preexisting path
        creature.GetComponent<Creature>().currentPath = null;


        //A*
        Dictionary<Node, float> dist = new Dictionary<Node, float>();
        Dictionary<Node, Node> prev = new Dictionary<Node, Node>();

        List<Node> unvisited = new List<Node>();
        List<Node> removed = new List<Node>();

        //where you start
        Node source = graph[
                            creature.GetComponent<Creature>().tileX,
                            creature.GetComponent<Creature>().tileY
                           ];

       //where you want to go
       Node target = graph[
                           x,
                           y
                          ];

       dist[source] = 0;
       prev[source] = null;

        //add all the nodes to the dist
       foreach (Node v in graph)
       {
         if (v != source)
         {
            dist[v] = Mathf.Infinity;
            prev[v] = null;
          }
       }

        //put in the source and all his neighbours
        unvisited.Add(source);
        foreach (Node v in source.neighbours)
        {
            
            unvisited.Add(v);
        }

        //while they're nodes to visit
        //I am now implementing A*
        //-------------------------
        while (unvisited.Count > 0)
        {

            //get the node with the lowest dist (F) value
            //if starting off this will be the source
            Node u = null;
            foreach (Node PossibleU in unvisited)
            {
                //check every unvisited node for the lowest one
                if (u == null || dist[PossibleU] < dist[u])
                {
                    u = PossibleU;
                }

            }

            //is it the destination?
            if (u == target)
            {
                break;
            }

            //remove u from the options
            unvisited.Remove(u);
            removed.Add(u);

            

            foreach (Node v in u.neighbours)
            {
                //basically calculate the previous G by going G = F - H
                float G = dist[u] - u.DistanceTo(target); 
                float H = v.DistanceTo(target);
                float cost = CostToEnterTile(u.x, u.y, v.x, v.y);
                float F = G + cost + H;

                if (F < dist[v])
                {
                    dist[v] = F;
                    prev[v] = u;

                    if(!removed.Contains(v))
                    {
                        unvisited.Add(v);
                    }
                    
                }
            }
        }
        //------------------------

        //here indicates we have either found the shortest route or there is no route
        if (prev[target] == null)
                {
                    //no route between target and source
                    return;
                }

                //construct path
                currentPath = new List<Node>();

                Node curr = target;

                //go back through the prev chain and add to path
                while (curr != null)
                {
                    currentPath.Add(curr);
                    curr = prev[curr];
                }

                //invert path
                currentPath.Reverse();

    creature.GetComponent<Creature>().currentPath = currentPath;
        
    }
}
