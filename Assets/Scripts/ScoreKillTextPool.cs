using System.Collections.Generic;
using UnityEngine;

public class ScoreKillTextPool : MonoBehaviour,IPoolable<GameObject>
{
    [SerializeField] private GameObject m_ScoreTextPrefab;
    [SerializeField][Range(3, 40)] private int m_poolSize = 20;
    [SerializeField] private Transform m_parent;

    private Queue<GameObject> m_listOfScoreTexts = new Queue<GameObject>();
    public bool IsPoolEmpty()
    {
        return m_listOfScoreTexts.Count == 0;
    }

    public bool IsPoolFull()
    {
        return m_listOfScoreTexts.Count == m_poolSize;
    }

    public GameObject TryGetPoolObject()
    {
        GameObject scoreText = null;

        if (!IsPoolFull())
        {
            scoreText = Instantiate(m_ScoreTextPrefab, m_parent);
            m_listOfScoreTexts.Enqueue(scoreText);
        }
        else
        {
            scoreText = m_listOfScoreTexts.Dequeue();
            m_listOfScoreTexts.Enqueue(scoreText);

        }

        return scoreText;
    }
}
