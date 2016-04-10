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
    public int c;

    public bool begin;
    public bool turnrotationinprogress = false;
    public bool gameover = false;

    public GameObject[] Combatants;
    public GameObject[] InitiativeList;

    //public GameObject ButtonControls;
    //public GameObject Map;
    public GameObject NowPlaying;

    public GameObject localmanager;



    [SyncVar]
    State Turnstate;

    //----------------------------------------------------------------------------------------
    //at the start initialise the state of the turnmanager
    //keep in master
    void Awake()
    {
        InitState();
    }
    //keep in master
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
    //keep in master
    void Start()
    {
        c = 0;
        begin = false;
        gameover = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameover)
        {
            if (begin && isServer)
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
    }

    //keep in master
    public void StartTheGame()
    {
        begin = true;
    }

    //at the start compile a list of all creatures capable of taking turns
    //keep in master
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
    //keep in master
    void Initiative()
    {
        //do an initiative order

        InitiativeList = Combatants;


        //execute all begin commands
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

            //CmdTurn(NowPlaying);
            localmanager.GetComponent<LocalTurn>().begin(NowPlaying);


            //if one master manager detects a turn has ended, increment

            //GameObject[] managers = null;
            //managers = GameObject.FindGameObjectsWithTag("LocalManager");
            //foreach(GameObject Entity in managers)
            //{
            //    Debug.Log("Manager found");
            //}

            bool check = localmanager.GetComponent<LocalTurn>().increment;
            if (check)
            {
                Debug.Log("Message");
                c++;
                if (c >= InitiativeList.Length)
                {
                    c = 0;
                }

                CmdIncrementCount(c);
                localmanager.GetComponent<LocalTurn>().increment = false;
            }
        }
    }

    //---------------------------------------------------------------------------------------------------------------

    ////Find a way to run this on everyone's client
    //void CmdTurn(GameObject current)
    //{
    //    //check if they've ticked the flag to end their turn
    //    //if they have go onto the next guy, but make sure to untick finished so it doesn't skip their turn forever
    //    if (current.GetComponent<Creature>().TurnFinished == false)
    //    {
            

    //        //set the button controls to them
    //        ButtonControls.GetComponent<ButtonControls>().ActivateCreature(current);
    //        Map.GetComponent<TileMap>().LoadInCreature(current);

    //        //change flags
    //        current.GetComponent<Creature>().MyTurn = true;
    //        current.GetComponent<Creature>().TurnFinished = false;

    //    }

    //    else
    //    {
            
    //        current.GetComponent<Creature>().TurnFinished = false;

    //        //this line here is just to make certain MyTurn is unticked
    //        current.GetComponent<Creature>().MyTurn = false;

    //        //edit flags and increment the count
    //        c++;
    //        if (c >= InitiativeList.Length)
    //        {
    //            c = 0;
    //        }

    //        CmdIncrementCount(c);
            
    //    }

    //}


    //----------------------------------------------------------------------------------------------------------

    //server commands
    //keep these on master
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

    //command to begin on all turn managers
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
