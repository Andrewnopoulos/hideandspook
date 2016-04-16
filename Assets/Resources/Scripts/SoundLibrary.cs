using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundLibrary : MonoBehaviour {

    [Range(0, 10.0f)]
    public float m_volumeScalar = 10.0f;

    public List<AudioClip> m_clangSounds;

    public static SoundLibrary s_soundLibrary;

	// Use this for initialization
	void Start ()
    {
        s_soundLibrary = this;
	}

    public AudioClip GetSound()
    {
        int randomVal;
        randomVal = Random.Range(0, m_clangSounds.Count);
        return m_clangSounds[randomVal];        
    }
}
