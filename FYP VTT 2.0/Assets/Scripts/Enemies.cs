using UnityEngine;
using System.Collections;



public class Enemies : MonoBehaviour
{

    public int number;
    public GameObject[] EnemyCreatures;

    // Use this for initialization
    void Start ()
    {
        //generate enemy data
        GenerateMonsters();

	}

    void GenerateMonsters()
    {
        for(int i = 0; i < number; i++)
        {
            int x = i + 1;
            int y = x * 2;
            GameObject go = (GameObject)Instantiate(EnemyCreatures[i], new Vector3(y,x,-0.5f), Quaternion.identity);
            Creature cr = go.GetComponent<Creature>();
            cr.tileX = y;
            cr.tileY = x;
        }
    }
	
}
