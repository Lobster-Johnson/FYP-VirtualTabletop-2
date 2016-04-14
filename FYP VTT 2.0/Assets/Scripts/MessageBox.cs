using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Networking;

public class MessageBox : NetworkBehaviour
{
    //[SyncVar]
    string ongoingmessage;

    private Text myGuiText;

    // Use this for initialization
    void Start()
    {
        myGuiText = GetComponent<Text>();
        ongoingmessage = null;
        ongoingmessage = "Welcome to the VTT! \n";
    }

    // Update is called once per frame
    void Update()
    {
        myGuiText.text = ongoingmessage;
    }

    public void NewMessage(string msg)
    {
        ongoingmessage += msg + " \n";
        //Rpcupdate(ongoingmessage);
    }

    //sync messages across clients
}
