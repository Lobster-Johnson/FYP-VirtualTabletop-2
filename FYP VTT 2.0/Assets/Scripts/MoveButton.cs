using UnityEngine;
using System.Collections;

public class MoveButton : MonoBehaviour {

    public GameObject currentMover;
    

    //when the map sends a destination to a creature it sends that creature to the move button
    public void newMover(GameObject next)
    {
        currentMover = next;
    }
	
    public void MoveCommand()
    {
        if (currentMover == null)
        {
            Debug.Log("No one here");
            return;
        }
        Debug.Log("MOVE COMMAND INITIATED");
        currentMover.GetComponent<Creature>().MoveNextTile();
    }
}
