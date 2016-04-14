using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Networking;

public class MessageBox : NetworkBehaviour
{
    //[SyncVar]
    string ongoingmessage;

    [SyncVar]
    public string networkedmessage;

    private Text myGuiText;
    
    // Use this for initialization
    void Start()
    {
        myGuiText = GetComponent<Text>();
        ongoingmessage = null;
        NewMessage("Welcome to the VTT!");
        //Cmdupdatestring(ongoingmessage);
    }

    // Update is called once per frame
    void Update()
    {
        getmessage();
        myGuiText.text = ongoingmessage;
        //if(ongoingmessage.Length > 399)
        //{
        //    ongoingmessage = ongoingmessage.Substring(21, ongoingmessage.Length - 21);
        //}
    }

    public void NewMessage(string msg)
    {
        ongoingmessage += msg + " \n";
        Cmdupdatestring(msg);
    }

    ////sync messages across clients
    [Command]
    void Cmdupdatestring(string msg)
    {
        networkedmessage += msg + " \n";
    }

    ////update all clients to get the message
    void getmessage()
    {
        ongoingmessage = networkedmessage;
    }
}
