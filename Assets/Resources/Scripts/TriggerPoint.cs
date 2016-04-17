using UnityEngine;
using System.Collections;

public class TriggerPoint : MonoBehaviour {

    public bool m_triggered = false;

	// Use this for initialization
	void Awake ()
    {
        TriggerManager.s_manager.m_triggerList.Add(this);

        GetComponentInChildren<ParticleSystem>().Play();
        GetComponentInChildren<Light>().enabled = true;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (m_triggered || TriggerManager.s_manager.m_finished)
        {
            //boxRenderer.material.color = new Color(0, 0, 1, 1); // blu
            //Spawn Particles from 'candle'
            GetComponentInChildren<ParticleSystem>().Stop();
            GetComponentInChildren<Light>().enabled = false;
        }

		if (StateManager.instance.state == State.PostGame)
		{
			GetComponentInChildren<ParticleSystem>().Stop();
		}
	}

    public void ActivateTrigger()
    {
        m_triggered = true;
        TriggerManager.s_manager.m_activeCount++;
        if (TriggerManager.s_manager.m_activeCount == TriggerManager.s_manager.m_maxCandles)
        {
            TriggerManager.s_manager.m_finished = true;
        }
    }
}