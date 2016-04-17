using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Valve.VR;

public class SteamMenuOpen : MonoBehaviour {
    
    bool systemMenuOpen = false;
    public GameObject steamOpenIcon;

	void Start()
	{
	//	SteamVR_Render.instance.pauseGameWhenDashboardIsVisible = false;
	}
    
	// Update is called once per frame
	void Update () {
		//systemMenuOpen = SteamVR_Overlay.instance.enabled;

		//for (int i = 0; i < 16; i++)
		//{
		//	var device = SteamVR_Controller.Input(i);
		//	if (device.GetPressDown(SteamVR_Controller.ButtonMask.System))
		//	{
		//		Debug.LogError("system open");
		//		systemMenuOpen = !systemMenuOpen;
		//	}
		//}
		if (systemMenuOpen == true)
        {
            steamOpenIcon.SetActive(true);
        }
        else
        {
            steamOpenIcon.SetActive(false);
        }

    }
}
