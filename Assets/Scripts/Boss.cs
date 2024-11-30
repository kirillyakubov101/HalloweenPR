using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private float m_moveSpeed = 5f;
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (player== null) { return; }
        Vector3 direction = (player.transform.position - transform.position).normalized;
        transform.Translate(direction * Time.deltaTime * m_moveSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("PlayerHealth"))
        PublicEventSystem.Instance.OnGameEnd?.Invoke(); //hit player = game loss
    }
}
    