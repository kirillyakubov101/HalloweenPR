using UnityEngine;

public class Scarecrow : MonoBehaviour, IDamageable
{
    private float m_hp = 300f;
    private SpawnScarecrows m_SpawnScarecrows;

    public bool IsDead() { return m_hp <= 0; }

    private void Awake()
    {
        m_SpawnScarecrows = FindObjectOfType<SpawnScarecrows>();
    }
    public void TakeDamage(float damage, Vector3 ContactPoint)
    {
        m_hp = Mathf.Max(0f, m_hp - damage);
        if(m_hp <= 0f)
        {
            Die();
        }
    }

    public void Die()
    {
        GameManager.KillScarecrow();
        Destroy(gameObject);
    }
}
