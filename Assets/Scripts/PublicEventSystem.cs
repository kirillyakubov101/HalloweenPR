using UnityEngine;
using UnityEngine.Events;

public class PublicEventSystem : MonoBehaviour
{
    public UnityEvent<Vector3> OnTargetKill;
    public UnityEvent<float> OnShotTaken;
    public UnityEvent OnPumpkinHit;
    public UnityEvent OnGameStart;
    public UnityEvent OnGameEnd;
    public UnityEvent OnPlayerHit;
    public UnityEvent OnBossSpawn;
    public UnityEvent OnBossDeath;

    private static PublicEventSystem instance;

    public static PublicEventSystem Instance { get => instance; }

    private void Awake()
    {
        instance = this;
    }

    public void StartGame()
    {
        Invoke(nameof(StartGameProcess), 0.5f);
    }

    private void StartGameProcess()
    {
        OnGameStart?.Invoke();
    }
}
