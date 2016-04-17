using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;

struct health
{
    public int maxhp;
    public int currhp;
}
public class Stats : NetworkBehaviour
{
    public new string name;
    public int maxhealth;
    public int currenthealth;
    public int maxspeed;
    public int armor;
    public int str;
    public int initiative;
    public int damageDice;

    [SyncVar]
    health life;

    void Awake()
    {
        InitState();
    }
    [Server]
    void InitState()
    {
        //if (!isServer)
        //{
        //    return;
        //}

        life = new health
        {
            maxhp = maxhealth,
            currhp = currenthealth
        };
    }

    
    // Use this for initialization
    void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(currenthealth <=0 )
        {
            CmdDie();
        }
	}

    //probably replace this with "change health"
    public void TakeDamage(int x)
    {
        Debug.Log("Took " + x + " damage!");
        Cmdserverdamage(x);
    }

    [Command]
    public void Cmdserverdamage(int x)
    {
        life = SDamage(life, x);
    }



    health SDamage(health previous, int x)
    {
        return new health
        {
            maxhp = previous.maxhp,
            currhp = previous.maxhp - x
        };
    }

    [Command]
    public void CmdDie()
    {
        //Destroy(this.gameObject);
        NetworkServer.Destroy(this.gameObject);
    }

    [Server]
    public void Die()
    {
        Destroy(this.gameObject);
    }

    public void updatehealth()
    {
        currenthealth = life.currhp;
    }
}
