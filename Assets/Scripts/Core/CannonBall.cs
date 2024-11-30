using UnityEngine;

public class CannonBall : MonoBehaviour
{
    [SerializeField] private float m_lifeTime = 5f;
    [SerializeField] private ParticleSystem m_poofVFX;
    [SerializeField] private ParticleSystem m_smokeTrail;
    [SerializeField] private ParticleSystem m_hitEffect;
    [SerializeField] private MeshRenderer m_Renderer;
    [SerializeField] private float m_damage = 100f;

    private Rigidbody rb;
    private Collider m_Collider;
    private bool m_vfxAlreadyPlayed = false;
    private float m_Sleeptimer = 0f;
    private bool m_isAwake = true;
    public Rigidbody Rb { get => rb; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        m_Collider = rb.GetComponent<Collider>();
    }

    private void Update()
    {
        if (!m_isAwake) { return; }
        if(m_Sleeptimer >= m_lifeTime)
        {
            Sleep();
        }

        m_Sleeptimer += Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.TryGetComponent<IDamageable>(out IDamageable comp))
        {
            comp.TakeDamage(m_damage, collision.contacts[0].point);
            m_hitEffect.gameObject.SetActive(true);
            m_hitEffect.Emit(55);
        }
        if(!m_vfxAlreadyPlayed)
        {
            m_poofVFX.Play();
            m_vfxAlreadyPlayed = true;
        }

        m_Renderer.enabled = false;
        m_Collider.enabled = false;
        m_smokeTrail.Stop();
    }

    public void WakeUp(Vector3 direction ,ForceMode mode)
    {
        m_Renderer.enabled = true;
        m_Collider.enabled = true;
        m_smokeTrail.Play();
        m_vfxAlreadyPlayed = false;
        gameObject.SetActive(true);
        m_isAwake = true;
        m_Sleeptimer = 0f;
        rb.AddForce(direction, mode);
    }

    public void Sleep()
    {
        m_vfxAlreadyPlayed = true;
        gameObject.SetActive(false);
        m_isAwake = false;



    }
}
