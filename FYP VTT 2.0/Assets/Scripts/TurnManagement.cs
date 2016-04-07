using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;

struct State
{
    public bool turnrote;
    public GameObject[] ServerCombatants;
    public int count;
}

public class TurnManagement : NetworkBehaviour
{
    int c;

    public bool begin;
    public bool turnrotationinprogress = false;
    public bool gameover = false;

    public GameObject[] Combatants;
    public GameObject[] InitiativeList;

    public GameObject ButtonControls;
    public GameObject Map;
    public GameObject NowPlaying;



    [SyncVar]
    State Turnstate;

    //at the start initialise the state of the turnmanager
    void Awake()
    {
        InitState();
    }

    [Server]
    void InitState()
    {
        Turnstate = new State
        {
            turnrote = turnrotationinprogress,
            ServerCombatants = Combatants,
            count = c
        };
    }



    // Use this for initialization
    void Start()
    {
        c = 0;
        begin = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (begin)
        {
            BeginTurnTracking();
            begin = false;
        }

        if (turnrotationinprogress)
        {
            TurnRotation();
        }

        UpdateOnServer();
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

        //make sure the list isn't empty
        if (Combatants == null)
        {
            Debug.Log("No combatants detected");
            return;
        }
        else
        {
            Initiative();
        }
        
    }

    //roll initiative for all creatures in the list, then sort it
    void Initiative()
    {
        //do an initiative order

        InitiativeList = Combatants;

        turnrotationinprogress = true;
        CmdBegin(turnrotationinprogress, InitiativeList);
    }

    //take turns for each creature
    void TurnRotation()
    {
        if (InitiativeList == null)
        {
            return;
        }
        else
        {
            NowPlaying = InitiativeList[c];
            CmdTurn(NowPlaying);
        }
    }

    //Find a way to run this on everyone's client
    void CmdTurn(GameObject current)
    {
        //check if they've ticked the flag to end their turn
        //if they have go onto the next guy, but make sure to untick finished so it doesn't skip their turn forever
        if (current.GetComponent<Creature>().TurnFinished == false)
        {
            

            //set the button controls to them
            ButtonControls.GetComponent<ButtonControls>().ActivateCreature(current);
            Map.GetComponent<TileMap>().LoadInCreature(current);

            //change flags
            current.GetComponent<Creature>().MyTurn = true;
            current.GetComponent<Creature>().TurnFinished = false;

        }

        else
        {
            c++;
            current.GetComponent<Creature>().TurnFinished = false;

            //this line here is just to make certain MyTurn is unticked
            current.GetComponent<Creature>().MyTurn = false;

            if (c >= InitiativeList.Length)
            {
                c = 0;
            }

            CmdIncrementCount(c);
        }

    }




    //server commands
    [Command]
    void CmdIncrementCount(int x)
    {
        Debug.Log("Requesting server incrementation");
        Turnstate = Increment(Turnstate, x);
    }

    State Increment(State prev, int x)
    {
        return new State
        {
            turnrote = prev.turnrote,
            ServerCombatants = prev.ServerCombatants,
            count = x
        };
    }

    [Command]
    void CmdBegin(bool turn, GameObject[] list)
    {
        Debug.Log("Sending game start command to server");
        Turnstate = Begin(Turnstate, turn, list);
    }

    State Begin(State prev, bool turn, GameObject[] list)
    {
        return new State
        {
            turnrote = turn,
            ServerCombatants = list,
            count = prev.count
        };
    }

    void UpdateOnServer()
    {

        turnrotationinprogress = Turnstate.turnrote;
        InitiativeList = Turnstate.ServerCombatants;
        c = Turnstate.count;
    }
}
