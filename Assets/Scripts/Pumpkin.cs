using System.Collections.Generic;
using UnityEngine;

struct BreakablePartsStructure
{

    public Vector3 initialPosition;
    public Quaternion initialRotation;

    public BreakablePartsStructure(Vector3 initialPosition, Quaternion initialRotation)
    {
        this.initialPosition = initialPosition;
        this.initialRotation = initialRotation;
    }
}

public class Pumpkin : MonoBehaviour, IDamageable
{
    [SerializeField] private Transform[] m_breakableParts;
    [SerializeField] private float m_health = 100f;
    [SerializeField] private MeshRenderer m_mainBody;
    [SerializeField] private Collider m_collder;
    [SerializeField] private float m_explosionForce;
    [SerializeField] private float m_explosionRadius;

    private Rigidbody m_rb;
    private PumpkinMovement m_moveComponent;
    private Dictionary<Transform, BreakablePartsStructure> m_DefaultPosForBreakableParts = new Dictionary<Transform, BreakablePartsStructure>();
    private bool m_diedOnce = false;
    private bool m_dead = false;
    private Vector3 m_lastContactPoint;

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
        m_moveComponent = GetComponent<PumpkinMovement>();
    }

    private void Start()
    {
        InitBreakableParts();
    }

    private void InitBreakableParts()
    {
        foreach (Transform ele in m_breakableParts)
        {
            m_DefaultPosForBreakableParts.Add(ele, new BreakablePartsStructure(ele.localPosition, ele.localRotation));
        }
    }

    public void SetSpawnPoint(Transform SpawnPoint)
    {
        if(m_moveComponent)
        {
            transform.position = SpawnPoint.position;
            m_moveComponent.LaunchPumpkin(SpawnPoint);
        }
    }

    public void TakeDamage(float damage, Vector3 ContactPoint)
    {
        if (m_dead) { return; }
        PublicEventSystem.Instance.OnPumpkinHit?.Invoke();
        m_lastContactPoint = ContactPoint;
        m_health -= damage;
        if(m_health <= 0 )
        {
            Die();
            PublicEventSystem.Instance.OnTargetKill.Invoke(transform.position);
        }
    }
 
    public void HitPlayer()
    {
        if (m_dead) { return; }
        PublicEventSystem.Instance.OnPumpkinHit?.Invoke();
        PublicEventSystem.Instance.OnPlayerHit?.Invoke();
        m_lastContactPoint = transform.position;
        m_health = 0f;
        m_dead = true;
        m_moveComponent.ResetAction();
        m_diedOnce = true;
        m_mainBody.enabled = false;
        m_collder.enabled = false;
       
        if(!m_rb.isKinematic)
        {
            m_rb.velocity = Vector3.zero;
            m_rb.angularVelocity = Vector3.zero;
        }
      
        m_rb.isKinematic = true;

    }

    private void Die()
    {
        m_moveComponent.ResetAction();
        m_dead = true;
        m_diedOnce = true;
        m_mainBody.enabled = false;
        m_collder.enabled = false;
        if (!m_rb.isKinematic)
        {
            m_rb.velocity = Vector3.zero;
            m_rb.angularVelocity = Vector3.zero;
        }
        m_rb.isKinematic = true;
        foreach (Transform ele in m_breakableParts)
        {
            ele.gameObject.SetActive(true);
            ele.GetComponent<Rigidbody>().AddExplosionForce(m_explosionForce, m_lastContactPoint, m_explosionRadius);
        }

        Invoke(nameof(HideParts), 1.5f);
    }

    private void HideParts()
    {
        foreach (Transform ele in m_breakableParts)
        {
            ele.gameObject.SetActive(false);
        }
    }

    public void WakeUp()
    {
        if (!m_diedOnce) { return; }
        CancelInvoke(nameof(HideParts));
        m_dead = false;
        m_rb.isKinematic = false;
        m_rb.velocity = Vector3.zero;
        m_rb.angularVelocity = Vector3.zero;
        m_mainBody.enabled = true;
        m_collder.enabled = true;
        m_health = 100;

        foreach (var ele in m_DefaultPosForBreakableParts)
        {
            ele.Key.gameObject.SetActive(false);
            ele.Key.GetComponent<Rigidbody>().velocity = Vector3.zero;
            ele.Key.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            ele.Key.localPosition = ele.Value.initialPosition;
            ele.Key.localRotation = ele.Value.initialRotation;
        }
    }
}
