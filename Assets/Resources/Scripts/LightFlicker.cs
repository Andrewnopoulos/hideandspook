using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Light))]
public class LightFlicker : MonoBehaviour {
    public float maxModifier  = 0.08f;
    public float minModifier = 0.03f;
    public float minTime = 0.01f;
    public float maxTime = 0.09f;

    private Light light;
    private float startIntensity;
    void Start()
    {
        light = GetComponent<Light>();
        startIntensity = light.intensity;

        StartCoroutine(UpdateLight());
    }
    //This might happen in the background so we could check if the object/light is dissabled and break the loop
    IEnumerator UpdateLight () {
        
        while(true)
        {
            float waitTime = Random.Range(minTime, maxTime);
            light.intensity = Random.Range(startIntensity - minModifier, startIntensity + maxModifier);
            yield return new WaitForSeconds(waitTime);
        }
        
	}
}
