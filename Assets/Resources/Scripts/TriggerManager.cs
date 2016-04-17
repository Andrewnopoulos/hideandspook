using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class TriggerManager : MonoBehaviour
{
	public int m_activeCount = 0;

	public List<TriggerPoint> m_triggerList = new List<TriggerPoint>();

	public bool m_finished = false;

	public uint m_activePlayers = 0;

	public int m_maxCandles = 0;

	public bool m_playing = false;
	private static TriggerManager sm_manager;
	public static TriggerManager s_manager
	{
		get
		{
			if (sm_manager == null)
			{
				sm_manager = Instantiate(Resources.Load<GameObject>("Prefabs/TriggerPoints")).GetComponent<TriggerManager>();
			}
			return sm_manager;
		}
	}

	void Awake()
    {
        sm_manager = this;
		DontDestroyOnLoad(this);
    }

	public void Reset()
	{
		m_triggerList = new List<TriggerPoint>();
		m_activeCount = 0;
		m_finished = false;
		m_playing = false;
		m_activePlayers = 0;
		m_maxCandles = 0;
	}

	public static void EndGame()
	{
        GameObject endObj = Instantiate(Resources.Load<GameObject>("Prefabs/EndGame"));
        Vector3 euler = Camera.main.transform.rotation.eulerAngles;
        euler.x = 0;
        euler.z = 0;
        endObj.transform.rotation = Quaternion.Euler(euler);
		bool ghostsWin = s_manager.m_activeCount >= s_manager.m_maxCandles;
		endObj.transform.Find("Canvas/AlchWins").gameObject.SetActive(!ghostsWin);
		endObj.transform.Find("Canvas/GhostsWin").gameObject.SetActive(ghostsWin);

		StateManager.instance.FadeIn(State.PostGame, null);
	}

	public static void ReloadGame()
	{
		s_manager.Reset();
		SceneManager.LoadScene(0);
		StateManager.instance.FadeOut(State.PreGame, null);
	}
}
