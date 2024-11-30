using UnityEngine;
using UnityEngine.Audio;

public class SoundHandler : MonoBehaviour
{
    [Header("Cannon")]
    [SerializeField] private AudioSource m_CannonAudioSource;
    [SerializeField] private float m_CannonMinPitch = 0.8f;
    [SerializeField] private float m_CannonMaxPitch = 1.2f;

    [Header("Pumpkin")]
    [SerializeField] private AudioSource m_PumpkinAudioSource;
    [SerializeField] private float m_PumpkinMinPitch = 0.8f;
    [SerializeField] private float m_PumpkinMaxPitch = 1.2f;

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer m_Mixer;

    private const string SFX_PARAM = "SFX_Volume";
    private const string BG_PARAM = "BG_Volume";

    private const float minDb = -80f;  // Minimum dB level
    private const float maxDb = 10f;   // Maximum dB level



    public void PlayCannonShot()
    {
        m_CannonAudioSource.pitch = Random.Range(m_CannonMinPitch, m_CannonMaxPitch);
        m_CannonAudioSource.Play();
    }

    public void PlayPumkinHit()
    {
        m_PumpkinAudioSource.pitch = Random.Range(m_PumpkinMinPitch, m_PumpkinMaxPitch);
        m_PumpkinAudioSource.Play();
    }

    public void Update_SFX_Volume(float sliderValue)
    {
        float dBValue = Mathf.Lerp(minDb, maxDb, sliderValue);
        m_Mixer.SetFloat(SFX_PARAM, dBValue);
    }

    public void Update_BG_Volume(float sliderValue)
    {
        float dBValue = Mathf.Lerp(minDb, maxDb, sliderValue);
        m_Mixer.SetFloat(BG_PARAM, dBValue);
    }


}
