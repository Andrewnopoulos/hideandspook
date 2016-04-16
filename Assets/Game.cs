using UnityEngine;
using System.Collections;
using Valve.VR;

public class Game : MonoBehaviour {

    HmdQuad_t _playArea;

    Vector3[] corners = new Vector3[4];

    public GameObject testText;

	// Use this for initialization
	void Start () {
        InitGameArea();
    }

    void InitGameArea()
    {
        SteamVR_PlayArea.GetBounds(SteamVR_PlayArea.Size.Calibrated, ref _playArea);

        corners[0] = new Vector3(_playArea.vCorners0.v0, _playArea.vCorners0.v1, _playArea.vCorners0.v2);
        corners[1] = new Vector3(_playArea.vCorners1.v0, _playArea.vCorners1.v1, _playArea.vCorners1.v2);
        corners[2] = new Vector3(_playArea.vCorners2.v0, _playArea.vCorners2.v1, _playArea.vCorners2.v2);
        corners[3] = new Vector3(_playArea.vCorners3.v0, _playArea.vCorners3.v1, _playArea.vCorners3.v2);

        int i = 0;
        foreach (Vector3 v in corners)
        {
            GameObject test = GameObject.Instantiate(testText);
            test.transform.position = v;
            test.GetComponent<TextMesh>().text = "index: " + i;
            i++;
        }
    }
	
	// Update is called once per frame
	void Update () {

	}
}
