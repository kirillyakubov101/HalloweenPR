using System.Collections.Generic;
using UnityEngine;

public class PumpkinPool : MonoBehaviour, IPoolable<Pumpkin>
{
    [SerializeField] private Pumpkin[] m_pumpkinPrefabs;
    [SerializeField][Range(3, 40)] private int m_poolSize = 20;
    [SerializeField] private Transform m_parent;

    private Queue<Pumpkin> m_listOfPumpkins = new Queue<Pumpkin>();
    public bool IsPoolEmpty()
    {
        return m_listOfPumpkins.Count == 0;
    }

    public bool IsPoolFull()
    {
        return m_listOfPumpkins.Count == m_poolSize;
    }

    public Pumpkin TryGetPoolObject()
    {
        Pumpkin pumpkinInstance = null;

        if (!IsPoolFull())
        {
            short randomIndex = (short)Random.Range(0, m_pumpkinPrefabs.Length);
            pumpkinInstance = Instantiate(m_pumpkinPrefabs[randomIndex], m_parent);
            m_listOfPumpkins.Enqueue(pumpkinInstance);
        }
        else
        {
            pumpkinInstance = m_listOfPumpkins.Dequeue();
            m_listOfPumpkins.Enqueue(pumpkinInstance);

        }

        return pumpkinInstance;
    }
}
