using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;



public class Enemies : NetworkBehaviour
{

    public int number;
    public GameObject EnemyCreatures;

    // Use this for initialization
    void Start ()
    {
        //generate enemy data
        GenerateMonsters(3, 5);
        GenerateMonsters(7, 4);
        GenerateMonsters(8, 1);
        GenerateMonsters(1, 8);

    }

    void GenerateMonsters(int x, int y)
    {
        
        GameObject enemy = Instantiate(EnemyCreatures);
        enemy.GetComponent<Creature>().forcespawn(x, y);
        NetworkServer.Spawn(enemy);
    }
	
}
