using TMPro;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField] private TMP_Text m_ScoreText;

    private int m_Score = 0;

    public void IncremenetScoreText(int amount)
    {
        m_Score += amount;
        m_ScoreText.text = m_Score.ToString();
    }
}
