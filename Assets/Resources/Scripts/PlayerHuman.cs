using UnityEngine;
using System.Collections;

public class PlayerHuman : MonoBehaviour {

    public TextMesh m_finishedTextMesh;
    public TextMesh m_gameOverTextMesh;
    public TextMesh m_titleTextMesh;
    public TextMesh m_readyTextMesh;

    public PlayerGhost ghost1;
    public PlayerGhost ghost2;

    public int m_deadPlayers = 0;

    public GameObject reticle;

 //   Transform m_reticleTransform;

    public LayerMask layerMask;

    public bool readyToReset = false;

	// Use this for initialization
	void Start ()
    {
      //  m_reticleTransform = reticle.GetComponent<Transform>();
	}
	
    private void EndGame()
    {
        TriggerManager.s_manager.m_playing = false;
        ghost1.m_readyToPlay = false;
        ghost2.m_readyToPlay = false;

        readyToReset = true;
        TriggerManager.s_manager.m_activePlayers = 0;

        ghost1.Reset();
        ghost2.Reset();
    }

	// Update is called once per frame
	void Update ()
    {
		if (TriggerManager.s_manager.m_finished)
		{
			m_gameOverTextMesh.text = "Game Over";
			m_finishedTextMesh.text = "The ghosts consumed your soul! :C";
			m_titleTextMesh.text = "";
			if (StateManager.instance.state == State.Playing)
			{
				//EndGame();
				TriggerManager.EndGame();
			}
        }

        if(m_deadPlayers == 2)
        {
            m_gameOverTextMesh.text = "Game Over";
            m_finishedTextMesh.text = "You survived the ritual!";
            m_titleTextMesh.text = "";
            if (StateManager.instance.state == State.Playing)
			{
				//EndGame();
				TriggerManager.EndGame();
			}
        }

        if (TriggerManager.s_manager.m_playing == false)
        {
            m_readyTextMesh.text = TriggerManager.s_manager.m_activePlayers + " ghost(s) ready\nPress red rutton to begin";

            //if (readyToReset)
            //{
            //    if (TriggerManager.s_manager.m_activePlayers == 2)
            //    {
            //        Application.LoadLevel(Application.loadedLevel);
            //    }
            //}
        }

        if (TriggerManager.s_manager.m_playing)
        {
            m_titleTextMesh.text = "";
            m_gameOverTextMesh.text = "";
            m_finishedTextMesh.text = "";
            m_readyTextMesh.text = "";
        }

        //Ray viewRay = new Ray(transform.position, transform.forward);
        //RaycastHit hit;

        //if (Physics.Raycast(viewRay, out hit, Mathf.Infinity, layerMask))
        //{
        //    m_reticleTransform.position = hit.point;
        //    m_reticleTransform.up = hit.normal;
        //}
	}

    public void DamageEnemy(PlayerGhost _target)
    {
        Debug.Log("Damaging lol");
        //_target.health -= val * Time.deltaTime;
        //_target.alpha -= val * Time.deltaTime;
    }
}
