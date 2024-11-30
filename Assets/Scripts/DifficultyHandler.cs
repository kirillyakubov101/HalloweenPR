using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
struct CustomKeyValuePair
{
    public int Key;
    public UnityEvent Value;
}

public class DifficultyHandler : MonoBehaviour
{
    [SerializeField] private List<CustomKeyValuePair> AllEvents;

    private void OnEnable()
    {
        GameManager.OnScoreChanged += AdjustDifficultyLevel;
    }

    private void OnDisable()
    {
        GameManager.OnScoreChanged -= AdjustDifficultyLevel;
    }

    private void AdjustDifficultyLevel(int score)
    {
        CustomKeyValuePair foundChecpoint = AllEvents.Where(x => x.Key == score).FirstOrDefault();
        if(foundChecpoint.Key != 0)
        {
            foundChecpoint.Value?.Invoke();
        }
    }
}
