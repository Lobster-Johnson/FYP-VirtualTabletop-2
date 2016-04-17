using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

public class Statbox : NetworkBehaviour
{
    string ongoingmessage;

    private Text myGuiText;

    new string name;
    int maxhealth = 0;
    int currenthealth = 0;
    int maxspeed = 0;
    int armor = 0;
    int str = 0;
    int initiative = 0;
    string normal = "No target";

    public GameObject target;

    // Use this for initialization
    void Start ()
    {
        myGuiText = GetComponent<Text>();
        ongoingmessage = null;
        ongoingmessage = "1 \n 2 \n 3 \n 4\n 5\n 6 \n 7\n";
    }
	
	// Update is called once per frame
	void Update ()
    {
        ongoingmessage = updateMessage();
        myGuiText.text = ongoingmessage;
    }

    public string updateMessage()
    {
        string message;
        
            message =
                "Name: " + name + "\n" +
                "Health: " + currenthealth + "/" + maxhealth + "\n"
                 + "Speed: " + maxspeed + "\n"
                 + "Initiative: " + initiative + "\n"
                 + "Strength: " + str;
       
        return message;
    }

    public void getSelected(GameObject x)
    {
        //take stats from the gameobject, and get rid of the word Clone
        name = x.GetComponent<Stats>().name;

        //take numeric stats
        maxhealth = x.GetComponent<Stats>().maxhealth;
        currenthealth = x.GetComponent<Stats>().currenthealth;
        maxspeed = x.GetComponent<Stats>().maxspeed;
        armor = x.GetComponent<Stats>().armor;
        str = x.GetComponent<Stats>().str;
        initiative = x.GetComponent<Stats>().initiative;
    }
}
