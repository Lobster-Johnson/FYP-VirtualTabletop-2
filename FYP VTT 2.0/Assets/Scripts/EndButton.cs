using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class EndButton : NetworkBehaviour
{
    public GameObject currentCreature;

    public void ActivateCreature(GameObject next)
    {
        currentCreature = null;
        currentCreature = next;
    }

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
