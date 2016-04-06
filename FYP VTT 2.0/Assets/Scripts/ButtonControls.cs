using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ButtonControls : NetworkBehaviour
{
    public GameObject currentCreature;

    //load from the turn manager the current creature
    public void ActivateCreature(GameObject next)
    {
        currentCreature = null;
        currentCreature = next;
    }


    //if the move button is pressed send the command to the creature
    public void MoveCommand()
    {
        if (currentCreature == null)
        {
            Debug.Log("No one here to command");
            return;
        }
        Debug.Log("MOVE COMMAND INITIATED");
        currentCreature.GetComponent<Creature>().CmdMoveNextTile();
    }

    //if the end turn button is pressed send the command to the creature
    public void EndCommand()
    {
        if (currentCreature == null)
        {
            Debug.Log("No one here to command");
            return;
        }
        Debug.Log("END TURN COMMAND INITIATED");
        currentCreature.GetComponent<Creature>().EndTurn();
    }


}
