using UnityEngine;
using System.Collections;

public class TargetSelection : MonoBehaviour {
    public GameObject Buttoncontroller;
    public GameObject target;
    public GameObject UI;

    //Select target

     void Start()
    {
        GameObject[] Buttons = GameObject.FindGameObjectsWithTag("Buttons");
        Buttoncontroller = Buttons[0];

        GameObject[] UIs = GameObject.FindGameObjectsWithTag("UI");
        UI = UIs[0];
    }
    void OnMouseUp()
    {
        Debug.Log("Target Selected " + this.gameObject.tag);
        target = this.gameObject;

        //send it to the button controller for resolving attacks
        Buttoncontroller.GetComponent<ButtonControls>().Target(target);

        //send the target to the statbox so the player can see info on the creature
        UI.GetComponent<Statbox>().getSelected(target);

    }
}
