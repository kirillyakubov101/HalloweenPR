using UnityEngine;

namespace HalloweenPR.Core
{
    public class Cannon : MonoBehaviour
    {
        [SerializeField] private float m_LaunchForce = 10f;
        [SerializeField] private float m_shootDistance = 20f;
        [SerializeField] private CannonBall m_cannonBallPrefab;
        [SerializeField] private Transform m_shootPosition;
        [SerializeField] private Vector3 m_shootDirection;
        [SerializeField] private ForceMode m_forceMode;
        [SerializeField] private Transform m_CannonBody;
        [SerializeField] private ParticleSystem m_explostionVFX;


        private Vector3 m_initialPosition;
        private Vector3 m_relativePosition_t;
        private Vector3 m_intialiVelocity;
        private Vector3 m_gravity;

        private CannonBallsPool m_CannonBallsPool;
        private CannonBall m_currentCannonBallInstance;
        private bool isReloading = false;
        private float m_reloadTimer = 0f;

        private static float m_reloadTime = 1f;


        private void Awake()
        {
            m_CannonBallsPool = FindObjectOfType<CannonBallsPool>();
        }

        private void Start()
        {
            m_gravity = Physics.gravity;
        }


        private void Update()
        {
            Ray mouseWorldRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 temp = mouseWorldRay.origin + mouseWorldRay.direction * m_shootDistance;
            Vector3 relativeDir = temp - m_CannonBody.position;

            m_CannonBody.localRotation = Quaternion.LookRotation(relativeDir);
            m_shootDirection = relativeDir.normalized;

            if(isReloading)
            {
                ReloadProcess();
            }

            if ((Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0)) && !isReloading)
            {
                isReloading = true;
                m_explostionVFX.Play();
                m_currentCannonBallInstance = m_CannonBallsPool.TryGetPoolObject();

                PublicEventSystem.Instance.OnShotTaken?.Invoke(m_reloadTime);
            }
        }


        private void ReloadProcess()
        {
            if(m_reloadTimer < m_reloadTime)
            {
                m_reloadTimer += Time.deltaTime;
            }
            else
            {
                isReloading = false;
                m_reloadTimer = 0f;
            }
        }
       

        private void FixedUpdate()
        {
            if (m_currentCannonBallInstance != null)
            {
                m_currentCannonBallInstance.Rb.velocity = Vector3.zero;
                m_currentCannonBallInstance.transform.position = m_shootPosition.position;
                m_currentCannonBallInstance.WakeUp(m_shootDirection * m_LaunchForce, m_forceMode);
                m_currentCannonBallInstance = null;
            }
        }

        //private void ProcessTrajectoryDraw()
        //{
        //    m_trajectoryDrawer.NewDraw(m_shootPosition.position, m_shootPosition.position, m_gravity, m_shootDirection * m_LaunchForce);
        //}

        public void AdjustReloadTime(float NewreloadTime)
        {
            m_reloadTime = NewreloadTime;
        }

        public void AdjustForce(float force)
        {
            this.m_LaunchForce = force;
        }

    }

}

