using System.Collections.Generic;
using UnityEngine;

public class CannonBallsPool : MonoBehaviour, IPoolable <CannonBall>
{
    [SerializeField][Range(3,40)] private int m_poolSize = 20;
    [SerializeField] private CannonBall m_CannonBallPrefab = null;
    [SerializeField] private Transform m_parent;
    
    private Queue<CannonBall> m_listOfCannonBalls = new Queue<CannonBall>();

    public bool IsPoolEmpty()
    {
        return m_listOfCannonBalls.Count == 0;
    }

    public bool IsPoolFull()
    {
        return m_listOfCannonBalls.Count == m_poolSize;
    }

    public CannonBall TryGetPoolObject()
    {
        CannonBall cannonBall = null;

        if (!IsPoolFull())
        {
            cannonBall = Instantiate(m_CannonBallPrefab, m_parent);
            m_listOfCannonBalls.Enqueue(cannonBall);
        }
        else
        {
            cannonBall = m_listOfCannonBalls.Dequeue();
            m_listOfCannonBalls.Enqueue(cannonBall);
            
        }

        return cannonBall;
    }
}
