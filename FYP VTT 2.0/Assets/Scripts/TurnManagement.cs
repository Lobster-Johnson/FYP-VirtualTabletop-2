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
    public int z;
    public bool begin;
    public bool turnrotationinprogress = false;
    public bool gameover = false;
    public bool turncheck;

    public GameObject[] Combatants;
    public GameObject[] InitiativeList;
    public GameObject[] turnmanagers;

    //public GameObject ButtonControls;
    //public GameObject Map;
    public GameObject NowPlaying;

    public GameObject localmanager;

    public GameObject Messenger;


    [SyncVar]
    State Turnstate;

    //----------------------------------------------------------------------------------------
    //at the start initialise the state of the turnmanager
    //keep in master
    void Awake()
    {
        InitState();
        Combatants = null;
        InitiativeList = null;
        turnmanagers = null;
    }

    //keep in master
    [Server]
    void InitState()
    {
        //if (!isServer)
        //{
        //    return;
        //}

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

        //get the message manager
        GameObject[] Msgs = GameObject.FindGameObjectsWithTag("Messages");
        Messenger = Msgs[0];
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

        if(gameover)
        {
            Messenger.GetComponent<MessageBox>().NewMessage("Game Over");
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
        foreach (GameObject Creature in Combatants)
        {
            Debug.Log(Creature.GetComponent<Stats>().initiative);
        }

        InitiativeList = Combatants;


        //execute all begin commands
        turnrotationinprogress = true;

        Messenger.GetComponent<MessageBox>().NewMessage("Game has started");
        Messenger.GetComponent<MessageBox>().NewMessage(InitiativeList[0].GetComponent<Stats>().name + " takes the first turn");

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
            if (NowPlaying != null)
            {
                //get the connection id of the player and give them authority over this
                if (isServer)
                {
                    if (this.gameObject.GetComponent<NetworkIdentity>().AssignClientAuthority(NowPlaying.GetComponent<NetworkIdentity>().connectionToClient))
                    {
                        //Debug.Log("Successful");
                    }
                }

                turnmanagers = GameObject.FindGameObjectsWithTag("LocalManager");
                //CmdTurn(NowPlaying);
                localmanager.GetComponent<LocalTurn>().begin(NowPlaying);

                bool check = false;
                check = localmanager.GetComponent<LocalTurn>().increment;

                //if it detects the creature has ended it's turn
                if (check)
                {
                    //update message manager
                    string name = NowPlaying.GetComponent<Stats>().name;
                    Messenger.GetComponent<MessageBox>().NewMessage(name +  " ended it's turn");

                    c++;
                    if (c >= InitiativeList.Length)
                    {
                        c = 0;
                    }

                    CmdIncrementCount(c);
                    localmanager.GetComponent<LocalTurn>().increment = false;
                    if (isServer)
                    {
                        this.gameObject.GetComponent<NetworkIdentity>().RemoveClientAuthority(NowPlaying.GetComponent<NetworkIdentity>().connectionToClient);
                    }
                    //let the next guy know it's their turn
                    string name2 = InitiativeList[c].GetComponent<Stats>().name;
                    Messenger.GetComponent<MessageBox>().NewMessage(name2 + "'s turn");
                }
            }
        }
    }


    //---------------------------------------------------------------------------------------------------------------

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
    [Server]
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
