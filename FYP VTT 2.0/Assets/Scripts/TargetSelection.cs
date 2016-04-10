using UnityEngine;
using System.Collections;

public class TargetSelection : MonoBehaviour {
    public GameObject Buttoncontroller;
    public GameObject target;
    //Select target

     void Start()
    {
        GameObject[] Buttons = GameObject.FindGameObjectsWithTag("Buttons");
        Buttoncontroller = Buttons[0];
    }
    void OnMouseUp()
    {
        Debug.Log("Target Selected " + this.gameObject.tag);
        target = this.gameObject;
        Buttoncontroller.GetComponent<ButtonControls>().Target(target);

    }
}
