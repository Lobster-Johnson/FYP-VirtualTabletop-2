using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class MoveButton : NetworkBehaviour
{

    public GameObject currentMover;
    

    //when the map sends a destination to a creature it sends that creature to the move button
    public void newMover(GameObject next)
    {
        currentMover = null;
        currentMover = next;
    }
	
    public void MoveCommand()
    {
        if (currentMover == null)
        {
            Debug.Log("No one here to command");
            return;
        }
        Debug.Log("MOVE COMMAND INITIATED");
        currentMover.GetComponent<Creature>().CmdMoveNextTile();
    }
}
