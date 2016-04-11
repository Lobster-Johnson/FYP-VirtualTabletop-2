using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;

struct LState
{
    public bool Increment;
}

public class LocalTurn : NetworkBehaviour
{
    public int y;

    public GameObject ButtonControls;
    public GameObject Map;
    public GameObject NowPlaying;
    public GameObject MasterManager;

    [SyncVar]
    LState turn;

    public bool increment;

    bool result;
    bool gameover;
    

    void Start()
    {
        GameObject[] managers = GameObject.FindGameObjectsWithTag("MainManager");
        MasterManager = managers[0];
    }

    //called every frame
    void Update()
    {
        if (!gameover)
        {
            if(NowPlaying != null)
            {
                Turn(NowPlaying);
            }
        }
        
    }

    public void Turn(GameObject current)
    {
        increment = false;
        result = false;
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

            current.GetComponent<Creature>().TurnFinished = false;

            //this line here is just to make certain MyTurn is unticked
            current.GetComponent<Creature>().MyTurn = false;

            increment = true;

            
        }
        
    }

    public void begin(GameObject np)
    {
        //server call
        if (!isServer)
            return;

        //starting variables


        RpcBegin(np);
    }

    [ClientRpc]
    void RpcBegin(GameObject np)
    {
        NowPlaying = np;
    }

    

    

}
