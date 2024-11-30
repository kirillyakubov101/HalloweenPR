using System;
using UnityEngine;

namespace HalloweenPR.Core
{
    [RequireComponent(typeof(Rigidbody))]
    public class Movement : MonoBehaviour
    {
        [SerializeField] private float m_moveSpeed = 20f;
        [SerializeField] private float m_force = 2000f;
        [SerializeField] private bool m_ApplyTransformTranslation;
        [SerializeField] private bool m_ApplyPhysicsTranslation;
        [SerializeField] private float m_maxVelocitySpeed = 10f;

        private Rigidbody m_Rigidbody;

        private Vector3 m_moveDirection = Vector3.zero;

        public static event Action OnMove;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Confined;
        }

        private void Awake()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
        }
        private void Update()
        {
            m_moveDirection.x = Input.GetAxisRaw("Horizontal");

            if(m_moveDirection.magnitude == 0)
            {
                OnMove?.Invoke();
            }

            if (m_ApplyTransformTranslation)
            {
                ApplyTransformTranslation();
                return;
            }

            if (m_ApplyPhysicsTranslation)
            {
                SpeedControl();
            }
        }

        private void FixedUpdate()
        {
            if (m_ApplyPhysicsTranslation)
            {
                ApplyPhysicsTranslation();
            }
        }

        private void ApplyTransformTranslation()
        {
            transform.position += m_moveDirection * Time.deltaTime * m_moveSpeed;
        }

        private void ApplyPhysicsTranslation()
        {
            m_Rigidbody.AddForce(m_moveDirection * Time.fixedDeltaTime * m_force);
        }
        private void SpeedControl()
        {
            if(m_Rigidbody.velocity.magnitude >= m_maxVelocitySpeed)
            {
                var temp = m_Rigidbody.velocity;
                temp = temp.normalized * m_maxVelocitySpeed;
                m_Rigidbody.velocity = temp;
            }
        }
    }

}
