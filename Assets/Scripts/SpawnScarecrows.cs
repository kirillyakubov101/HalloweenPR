using UnityEngine;

public class SpawnScarecrows : MonoBehaviour
{
   
    [SerializeField] private Scarecrow[] m_scarecrowsPrefabs = new Scarecrow[2];
    [SerializeField] private Transform[] m_SpawnPositions;
    [SerializeField] private int m_crow_1_checkpointScore = 5000;
    [SerializeField] private int m_crowOffsetScore = 1000;
    [SerializeField] private Scarecrow[] m_ScareCrowArray = new Scarecrow[2];

    private bool m_firstCrowSpawned = false;
    private bool m_secondCrowSpawned = false;
    private int m_crow_2_checkpointScore = -1;
    private int m_crowToSpawnIndex = -1;
    private int m_currentIndexForCrowArray = -1;

    private void OnEnable()
    {
        GameManager.OnScoreChanged += ConsiderSpawningCrow;
    }

    private void OnDisable()
    {
        GameManager.OnScoreChanged -= ConsiderSpawningCrow;
    }

    private void ConsiderSpawningCrow(int score)
    {
        //first crow spawn condition
        if (score >= m_crow_1_checkpointScore && !m_firstCrowSpawned)
        {
            SpawnCrow();
            m_firstCrowSpawned = true;
            m_crow_2_checkpointScore = m_crow_1_checkpointScore + m_crowOffsetScore;
        }

        //Second corw spawn condition
        else if(score >= m_crow_2_checkpointScore && m_firstCrowSpawned && m_ScareCrowArray[m_currentIndexForCrowArray].IsDead() && !m_secondCrowSpawned)
        {
            m_secondCrowSpawned = true;
            SpawnCrow();
        }
    }

    private void SpawnCrow()
    {
        int randomIndex = Random.Range(0, m_SpawnPositions.Length);

        m_ScareCrowArray[++m_currentIndexForCrowArray] = Instantiate(m_scarecrowsPrefabs[++m_crowToSpawnIndex], m_SpawnPositions[randomIndex].position, m_SpawnPositions[randomIndex].rotation);
    }
}
