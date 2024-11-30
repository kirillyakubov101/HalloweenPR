using System.Collections;
using UnityEngine;
using System.Linq;

public class PumpkinSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] m_spawnPoints;
    [SerializeField] private Transform m_bossSpawn;
    [SerializeField] private Boss m_bossPrefab;
    [SerializeField] private float m_spawnSpeed = 1f;

    private WaitForSeconds m_sleep;
    private PumpkinPool m_PumpkinPool;

    private void Awake()
    {
        m_PumpkinPool = FindObjectOfType<PumpkinPool>();
    }

    private void Start()
    {
        StartCoroutine(SpawnProcess());
    }

    private void OnEnable()
    {
        GameManager.OnSpawnBossTime += SpawnBoss;
    }

    private void OnDisable()
    {
        GameManager.OnSpawnBossTime -= SpawnBoss;
    }

    private IEnumerator SpawnProcess()
    {
        int randomIndex = -1;
        while (true)
        {
            var newPumpkin = m_PumpkinPool.TryGetPoolObject();
            newPumpkin.WakeUp();
            randomIndex = Random.Range(0, m_spawnPoints.Length);
            newPumpkin.SetSpawnPoint(m_spawnPoints[randomIndex]);

            yield return new WaitForSeconds(m_spawnSpeed);
        }
    }

    private void SpawnBoss()
    {
        PublicEventSystem.Instance.OnBossSpawn?.Invoke();
        Instantiate(m_bossPrefab, m_bossSpawn);
    }


    public void StopSpawn()
    {
        StopAllCoroutines();
        var activePumpkins = FindObjectsOfType<Pumpkin>().Where(pumpkin => pumpkin.gameObject.activeSelf);
        foreach (var ele in activePumpkins)
        {
            ele.gameObject.SetActive(false);
        }

        var boss = FindObjectOfType<Boss>();
        if (boss)
        {
            boss.gameObject.SetActive(false);
        }
       
         
    }

    public void AdjustSpawnRate(float spawnRate)
    {
        this.m_spawnSpeed = spawnRate;
    }

    public void AdjustPumpkinsSpeed(float moveSpeed)
    {
        PumpkinMovement.AdjustParams(moveSpeed);
    }

}
