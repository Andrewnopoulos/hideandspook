using UnityEngine;
using UnityEngine.SceneManagement;

using System.Collections;

public class ResetScene : MonoBehaviour {
    
	// Update is called once per frame
	void Update () {
        if(Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.R))
        {
            //Application.LoadLevel(Application.loadedLevel);
            SceneManager.LoadScene(0);        
        }
	}
}
