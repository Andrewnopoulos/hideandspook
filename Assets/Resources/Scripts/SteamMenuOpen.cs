using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Valve.VR;

public class SteamMenuOpen : MonoBehaviour {
    
    bool systemMenuOpen = false;
    public GameObject steamOpenIcon;
    
	
	// Update is called once per frame
	void Update () {
        for(int i = 1; i < 16; i++)
        {
            var device = SteamVR_Controller.Input(i);
            if(device.GetPressDown(EVRButtonId.k_EButton_System))
            {
                //It's now up
                systemMenuOpen = !systemMenuOpen;
            }
        }
        if(systemMenuOpen == true)
        {
            steamOpenIcon.SetActive(true);
        }
        else
        {
            steamOpenIcon.SetActive(false);
        }

    }
}
