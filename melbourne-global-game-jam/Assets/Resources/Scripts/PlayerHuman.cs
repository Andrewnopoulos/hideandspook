using UnityEngine;
using System.Collections;

public class PlayerHuman : MonoBehaviour {

    public TextMesh m_finishedTextMesh;
    public TextMesh m_gameOverTextMesh;
    public TextMesh m_titleTextMesh;
    public TextMesh m_readyTextMesh;

    public PlayerGhost ghost1;
    public PlayerGhost ghost2;

    public float m_gameOverTime = 5.0f;
    float m_currentGameOverTime = 0;

    public int m_deadPlayers = 0;

    public GameObject reticle;

    Transform m_reticleTransform;

    public LayerMask layerMask;

    public bool readyToReset = false;
    bool m_restarting = false;

	// Use this for initialization
	void Start ()
    {
        m_reticleTransform = reticle.GetComponent<Transform>();
	}
	
    private void EndGame()
    {
        TriggerManager.s_manager.m_playing = false;
        ghost1.m_readyToPlay = false;
        ghost2.m_readyToPlay = false;

        readyToReset = true;
        TriggerManager.s_manager.m_activePlayers = 0;

        ghost1.Reset(false);
        ghost2.Reset(false);
    }

	// Update is called once per frame
	void Update ()
    {
	    if(TriggerManager.s_manager.m_finished)
        {
            m_gameOverTextMesh.text = "Game Over";
            m_finishedTextMesh.text = "The ghosts consumed your soul! :C";
            m_titleTextMesh.text = "";
            if (readyToReset == false)
                EndGame();
        }        

        if (TriggerManager.s_manager.m_playing == false)
        {
            m_readyTextMesh.text = TriggerManager.s_manager.m_activePlayers + " ghost(s) ready\nPress red rutton to begin";

            if (readyToReset && !m_restarting)
            {
                m_restarting = true;
                TriggerManager.s_manager.m_playing = true;

                //if (TriggerManager.s_manager.m_activePlayers == 2)
                //{
                    //Never triggered at all. :O
                    //Application.LoadLevelAsync(Application.loadedLevel);
                StartCoroutine(IGameOver(m_gameOverTime));
                readyToReset = false;
                //}
            }
        }

        if (TriggerManager.s_manager.m_playing)
        {
            m_titleTextMesh.text = "";
            m_gameOverTextMesh.text = "";
            m_finishedTextMesh.text = "";
            m_readyTextMesh.text = "";
        }

        Ray viewRay = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(viewRay, out hit, Mathf.Infinity, layerMask))
        {
            m_reticleTransform.position = hit.point;
            m_reticleTransform.up = hit.normal;
        }

        if (m_deadPlayers == 2)
        {
            m_gameOverTextMesh.text = "Game Over";
            m_finishedTextMesh.text = "You survived the ritual!";
            m_titleTextMesh.text = "";
            if (readyToReset == false)
                EndGame();
        }
	}

    IEnumerator IGameOver(float _secondsToWait)
    {
        while(m_currentGameOverTime < _secondsToWait)
        {
            m_currentGameOverTime += Time.deltaTime;
            
            yield return new WaitForEndOfFrame();
        }

        Application.LoadLevel(Application.loadedLevel);
        yield return null;
    }

    public void DamageEnemy(PlayerGhost _target)
    {
        Debug.Log("Damaging lol");
        //_target.health -= val * Time.deltaTime;
        //_target.alpha -= val * Time.deltaTime;
    }
}
