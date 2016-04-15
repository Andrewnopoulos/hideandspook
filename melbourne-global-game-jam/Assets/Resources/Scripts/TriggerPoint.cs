using UnityEngine;
using System.Collections;

public class TriggerPoint : MonoBehaviour {

    public bool m_triggered = false;

    Renderer boxRenderer;

	// Use this for initialization
	void Start ()
    {
        TriggerManager.s_manager.m_triggerList.Add(this);

        boxRenderer = GetComponentInChildren<Renderer>();

        //GetComponentInChildren<ParticleSystem>().Play();
        GetComponentInChildren<Light>().enabled = true;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (m_triggered)
        {
            //boxRenderer.material.color = new Color(0, 0, 1, 1); // blu
            //Spawn Particles from 'candle'
            //GetComponentInChildren<ParticleSystem>().Stop();
            GetComponentInChildren<Light>().enabled = false;
        }
	}

    public void ActivateTrigger()
    {
        m_triggered = true;
        TriggerManager.s_manager.m_activeCount++;
        if (TriggerManager.s_manager.m_activeCount == 5)
        {
            TriggerManager.s_manager.m_finished = true;
        }
    }
}