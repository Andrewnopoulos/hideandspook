using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TriggerManager : MonoBehaviour
{
    [Range(0.0f, 0.5f)]
    public float m_delayModifier = 0.25f;
    [Range(500, 3000)]
    public ushort m_lengthModifier = 1000;
    public float m_heightMin = 1.0f;
    public float m_heightMax = 1.5f;

    public int m_activeCount = 0;

    public int m_currentIndex = 0;

    public List<TriggerPoint> m_triggerList = new List<TriggerPoint>();
    public static TriggerManager s_manager = null;

    public bool m_finished = false;

    public uint m_activePlayers = 0;

    public bool m_playing = false;

    void Awake()
    {
        s_manager = this;
    }

}
