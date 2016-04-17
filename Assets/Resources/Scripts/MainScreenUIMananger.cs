using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;

public class MainScreenUIMananger : MonoBehaviour {
    /*
    public Image ghost1Circle;
    public Image ghost2Circle;
    public Color colorGrey;
    public Color colorRed;
    public Color colorBlue;
    */

    public PlayerGhost ghost1;
    public PlayerGhost ghost2;
    public PlayerHuman playerHuman;

    public UnityEvent onStart;
    public UnityEvent onGhost1Join;
    public UnityEvent onGhost2Join;
    public UnityEvent onGhost1Dead;
    public UnityEvent onGhost2Dead;
    public UnityEvent onGhostsWin;
    public UnityEvent onAlchemistWins;

    //public int players = 0;


    // Use this for initialization
    void Start () {
        onStart.Invoke();
    }
	
	// Update is called once per frame
	void Update () {

        //if(TriggerManager.s_manager.m_activePlayers == 1)
       if(ghost1.m_readyToPlay)
        {
            onGhost1Join.Invoke();
        }
        //if(TriggerManager.s_manager.m_activePlayers == 2)
        if(ghost2.m_readyToPlay)
        {
            onGhost2Join.Invoke();
        }

        if(ghost1.dead == true)
        {
            onGhost1Dead.Invoke();
        }

        if (ghost2.dead == true)
        {
            onGhost2Dead.Invoke();
        }

        if(TriggerManager.s_manager.m_finished)
        {
            if (playerHuman.m_deadPlayers == 2)
            {
                onAlchemistWins.Invoke();
            }
            else
            {
                onGhostsWin.Invoke();
            }
        }

    }
}
