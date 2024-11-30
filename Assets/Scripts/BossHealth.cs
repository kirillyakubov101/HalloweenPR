using System;
using UnityEngine;

public class BossHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private ParticleSystem m_hitEffectVfx;
    [SerializeField] private float m_Health;
    [SerializeField] private float m_maxHealth = 1000f;
    [SerializeField] private ParticleSystem m_explosionVFX;
    [SerializeField] private Transform m_body;
    [SerializeField] private Collider[] m_colliders;
    [SerializeField] private ParticleSystem m_skullsVFX;

    public static event Action<float,float> OnHit;

    private Boss Boss;
    private void Awake()
    {
        Boss = GetComponent<Boss>();
    }

    private void Start()
    {
        m_Health = m_maxHealth;
    }

    public float Health { get => m_Health; }

    public void TakeDamage(float damage, Vector3 ContactPoint)
    {
        OnHit?.Invoke(damage, m_maxHealth);
        m_Health = Mathf.Max(0f, m_Health - damage);
        m_hitEffectVfx.transform.position = ContactPoint;
        m_hitEffectVfx.Play();

        if (m_Health <= 0f)
        {
            BossDie();
        }
    }

    private void BossDie()
    {
        m_explosionVFX.Play();
        m_skullsVFX.gameObject.SetActive(false);
        m_body.gameObject.SetActive(false);
        for (int i = 0; i < m_colliders.Length; i++)
        {
            m_colliders[i].enabled = false;
        }
        
        if(Boss)
        {
            Boss.enabled = false;
        }



        PublicEventSystem.Instance.OnBossDeath?.Invoke();
    }
}