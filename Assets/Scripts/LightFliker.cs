using System.Collections;
using UnityEngine;

public class LightFliker : MonoBehaviour
{
    [SerializeField] private float m_maxIntensity = 1f;
    [SerializeField] private float targetTime = 0.8f;
    private Light m_lightSource;
  

    private void Awake()
    {
        m_lightSource = GetComponent<Light>();
    }

    private void Start()
    {
        StartCoroutine(FlikerProcess());
    }

    private IEnumerator FlikerProcess()
    {
        float amount = m_lightSource.intensity;
        float multi = 1f;
        float speed = m_maxIntensity / targetTime;  // Calculate speed based on max intensity and time
        while (true)
        {
            amount += Time.deltaTime * multi * speed;
            m_lightSource.intensity = amount;

            if (amount > m_maxIntensity)
            {
                multi *= -1f;
            }
            else if(amount <= 0f)
            {
                multi *= -1f;
            }

            yield return null;
        }
    }
}
