using UnityEngine;
using System.Collections;

public class TurnManagement : MonoBehaviour
{
    int c;

    public bool begin;// = false;
    public bool turnrotationinprogress = false;
    public bool gameover = false;

    public GameObject[] Combatants;
    public GameObject[] InitiativeList;

    public GameObject ButtonControls;

	// Use this for initialization
	void Start ()
    {
        c = 0;
        begin = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(begin)
        {
            BeginTurnTracking();
            begin = false;
        }

        if(turnrotationinprogress)
        {
            TurnRotation();
        }
	}

    public void StartTheGame()
    {
        begin = true;
    }

    //at the start compile a list of all creatures capable of taking turns
    void BeginTurnTracking()
    {
        Debug.Log("Game Start called. Detecting creatures capable of fighting");

        Combatants = null;
        InitiativeList = null;

        //get all objects of creatures that can take part in the fight
        //currently only finds players
        Combatants = GameObject.FindGameObjectsWithTag("Player");

        Initiative();
    }

    //roll initiative for all creatures in the list, then sort it
    void Initiative()
    {
        turnrotationinprogress = true;
    }

    //take turns for each creature
    void TurnRotation()
    {
        //now that we have a list of objects in initiative order, make them take their turns
        //while(!gameover)
        //{
        //    Debug.Log("The game is currenlty running");
        //    for(int i = 0; i < Combatants.Length; i++)
        //    {
        //        Turn(Combatants[0]);
        //    }
        //    gameover = true;
        //}
        //Debug.Log("Game over");


        Turn(Combatants[c]);
        Debug.Log("Turn");
    }

    void Turn(GameObject current)
    {
        //check if they've ticked the flag to end their turn
        //if they have go onto the next guy, but make sure to untick finished so it doesn't skip their turn forever
        if (current.GetComponent<Creature>().TurnFinished == false)
        {
            Debug.Log("Turn in progress");
            ButtonControls.GetComponent<ButtonControls>().ActivateCreature(current);
            current.GetComponent<Creature>().MyTurn = true;
            current.GetComponent<Creature>().TurnFinished = false;
            //EndButton.GetComponent<endbutton>().ActivateCreature(current);

        }

        else
        {
            c++;
            current.GetComponent<Creature>().TurnFinished = false;
            //this line here is just to make certain MyTurn is unticked
            current.GetComponent<Creature>().MyTurn = false;

            if (c >= Combatants.Length)
            {
                c = 0;
            }
        }
        
    }
}
