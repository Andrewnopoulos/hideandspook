using UnityEngine;
using System.Collections;

public class RigidbodySound : MonoBehaviour {

	/*void OnCollisionEnter(Collision other)
    {
        float relativeForce = other.relativeVelocity.magnitude;
        //AudioSource.PlayClipAtPoint(SoundLibrary.s_soundLibrary.GetSound(), transform.position, relativeForce * SoundLibrary.s_soundLibrary.m_volumeScalar);
    }*/

    CardboardAudioSource source;
    void Start()
    {
        source = gameObject.AddComponent<CardboardAudioSource>();
    }
    void OnCollisionEnter(Collision other)
    {
        if(TriggerManager.s_manager.m_playing == true)
        {
            float relativeForce = other.relativeVelocity.magnitude;
            source.PlayOneShot(SoundLibrary.s_soundLibrary.GetSound(), relativeForce * SoundLibrary.s_soundLibrary.m_volumeScalar);
        }
    }
}
