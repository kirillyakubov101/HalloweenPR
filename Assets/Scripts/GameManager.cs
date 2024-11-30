using HalloweenPR.Core;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Texture2D m_crosshairCursor;

    [Header("Player Cannons")]
    [SerializeField] private Transform m_cannon_1;
    [SerializeField] private Transform m_cannon_2;

    public int m_score = 0;
    public static event Action OnSpawnBossTime;
    public static event Action<int> OnScoreChanged;

    private static int m_amountOfKilledCrows = 0;

    Vector2 m_mouseHotSpot = new Vector2(10f , 10f);

    private void Awake()
    {
        OnSpawnBossTime = null;
        OnScoreChanged = null;
        m_amountOfKilledCrows = 0;
    }

   

    private void Update()
    {
        Cursor.SetCursor(m_crosshairCursor, m_mouseHotSpot, CursorMode.ForceSoftware);
    }


    private void OnApplicationFocus(bool focus)
    {
        Cursor.SetCursor(m_crosshairCursor, Vector2.zero, CursorMode.ForceSoftware);
    }

    public void EnterSoundOptions()
    {
        Time.timeScale = 0f;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

    }

    public void ExitSoundOptions()
    {
        Time.timeScale = 1f;
        Cursor.SetCursor(m_crosshairCursor, Vector2.zero, CursorMode.Auto);
    }

    public void ReloadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void AddScore()
    {
        m_score += 250;
        OnScoreChanged?.Invoke(m_score);
    }

    public static void KillScarecrow()
    {
        m_amountOfKilledCrows++;

        if (m_amountOfKilledCrows == 2)
        {
            OnSpawnBossTime?.Invoke();
        }
        
    }

    public void DisablePlayer()
    {
        FindObjectsOfType<Cannon>()
           .Where(x => x.enabled)
           .ToList()
           .ForEach(x => x.enabled = false);

        FindObjectsOfType<Movement>()
          .Where(x => x.enabled)
          .ToList()
          .ForEach(x => x.enabled = false);
    }

    public void EnableSmallCannon_1()
    {
        m_cannon_1.gameObject.SetActive(true);
    }

    public void EnableSmallCannon_2()
    {
        m_cannon_2.gameObject.SetActive(true);
    }


}
