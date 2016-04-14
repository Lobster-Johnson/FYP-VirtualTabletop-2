using UnityEngine;
using System.Collections;

public class DiceRoller : MonoBehaviour {

    //dice rolls for initiative, to hit, etc
    public int DiceRoll(int modifier)
    {

        int result = Random.Range(1, 21);
        result += modifier;
        return result;
    }

    //damage rolls
    //takes in the modifier and damage dice
    public int damageRoll(int modifier, int dice)
    {
        int result = Random.Range(1, (dice + 1));
        result += modifier;
        return result;
    }
}
