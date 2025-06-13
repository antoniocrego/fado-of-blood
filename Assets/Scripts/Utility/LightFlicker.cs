using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public Light flameLight;
    public float minIntensity = 1.5f;
    public float maxIntensity = 2.5f;
    public float flickerSpeed = 0.1f;

    private void Start()
    {
        if (flameLight == null)
        {
            flameLight = GetComponent<Light>();
        }
    }

    void Update()
    {
        flameLight.intensity = Random.Range(minIntensity, maxIntensity);
    }
}

