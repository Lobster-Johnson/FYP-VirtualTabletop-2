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
        GenerateMonsters(7, 5);

	}

    void GenerateMonsters(int x, int y)
    {
        //for(int i = 0; i < number; i++)
        //{
        //    int x = i + 1;
        //    int y = x * 2;
        //    GameObject go = (GameObject)Instantiate(EnemyCreatures[i], new Vector3(y,x,-0.5f), Quaternion.identity);
        //    Creature cr = go.GetComponent<Creature>();
        //    cr.tileX = y;
        //    cr.tileY = x;
        //}
        GameObject enemy = Instantiate(EnemyCreatures);
        enemy.GetComponent<Creature>().forcespawn(x, y);
        NetworkServer.Spawn(enemy);
    }
	
}
