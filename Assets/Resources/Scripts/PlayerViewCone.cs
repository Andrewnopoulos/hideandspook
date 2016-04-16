using UnityEngine;
using System.Collections;

public class PlayerViewCone : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player1" || other.tag == "Player2")
        {
            GetComponentInParent<PlayerHuman>().DamageEnemy(other.gameObject.GetComponent<PlayerGhost>());
        }
    }
}
