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
    public int damageRoll(int modifier)
    {
        int result = Random.Range(1, 11);
        result += modifier;
        return result;
    }
}
