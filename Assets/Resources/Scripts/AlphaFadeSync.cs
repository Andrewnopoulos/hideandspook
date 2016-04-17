using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AlphaFadeSync : MonoBehaviour
{
	void Update()
    {
        StateManager sm = StateManager.instance;
        float alpha = sm.alpha;

        Text text = GetComponent<Text>();
        if (text != null)
        {
            GetComponent<Text>().color = new Color(1, 1, 1, alpha);
        }
	}
}
