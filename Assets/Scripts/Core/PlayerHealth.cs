using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int health = 3;

    private bool m_isDead = false;
    private void OnTriggerEnter(Collider other)
    {
        TakeDamage();
        if(other.TryGetComponent(out Pumpkin pumpkin))
        {
            pumpkin.HitPlayer();
        }
    }

    private void TakeDamage()
    {
        health--;
        if (health <= 0 && !m_isDead)
        {
            m_isDead = true;
            PublicEventSystem.Instance.OnGameEnd?.Invoke();
        }
    }
}
