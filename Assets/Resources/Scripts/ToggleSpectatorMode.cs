using UnityEngine;
using System.Collections;

public class ToggleSpectatorMode : MonoBehaviour {
    public GameObject spectatorModeUI;
    public GameObject staticBackGround;

    private bool spectatorMode = true;
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetKeyDown(KeyCode.S))
        {
            spectatorMode = !spectatorMode;
        }
        
        spectatorModeUI.SetActive(spectatorMode);
        staticBackGround.SetActive(!spectatorMode);
    }
}
