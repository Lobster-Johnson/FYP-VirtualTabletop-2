using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ButtonControls : NetworkBehaviour
{
    public GameObject currentCreature;
    public GameObject target;
    public GameObject msgbox;
    public GameObject dice;

    public void start()
    {


    }


    //load from the turn manager the current creature
    public void ActivateCreature(GameObject next)
    {
        currentCreature = null;
        currentCreature = next;
    }

    public void Target(GameObject choice)
    {
        target = null;
        target = choice;
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

    public void AttackCommand()
    {
        int range = 1;
        //attacker locaction
        int x1;
        int y1;

        //target location
        int x2;
        int y2;

        if (currentCreature == null)
        {
            Debug.Log("No one here to command");
            return;
        }
        if (target == null)
        {
            Debug.Log("Select a target");
            return;
        }

        if (target == currentCreature)
        {
            Debug.Log("You cant attack yourself lad");
            return;
        }
        x1 = currentCreature.GetComponent<Creature>().tileX;
        y1 = currentCreature.GetComponent<Creature>().tileY;

        x2 = target.GetComponent<Creature>().tileX;
        y2 = target.GetComponent<Creature>().tileY;

        if ((x1 - x2) > range ||
            (x1 - x2) < -range ||
            (y1 - y2) > range ||
            (y1 - y2) < -range)
        {
            Debug.Log("Out of range");
        }
        else
        {
            Debug.Log("Within Range");
            // msgbox.GetComponent<MessageBox>().NewMessage("Attack");
            //int attack = currentCreature.GetComponent<Stats>().str;
            //int result = dice.GetComponent<DiceRoller>().DiceRoll(attack);
            //Debug.Log(result);



        }
    }


}
