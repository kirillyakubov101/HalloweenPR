using UnityEngine;

namespace HalloweenPR.Core
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform m_playerBody;
        [SerializeField] private Vector3 m_offset;
        [SerializeField] private float m_smoothSpeed = 10f;

        private Vector3 refVelocity;
        private void LateUpdate()
        {
            Vector3 desiredPos = m_playerBody.position + m_offset;
            transform.position = Vector3.SmoothDamp(transform.position,desiredPos, ref refVelocity, m_smoothSpeed * Time.deltaTime);
            
        }
    }

}
