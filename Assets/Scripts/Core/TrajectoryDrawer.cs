using System.Collections.Generic;
using UnityEngine;

namespace HalloweenPR.Core
{
    public class TrajectoryDrawer : MonoBehaviour
    {
        [SerializeField] private LineRenderer m_lineRenderer;
        [SerializeField] private int m_resolution = 30;
        [SerializeField] private float m_timeStep = 0.01f;
        [SerializeField] private LayerMask mask = new LayerMask();
        [SerializeField] private GameObject m_HitDecal;

        private List<Vector3> trajectoryPoints = new List<Vector3>();


        private Vector3 center;
        private RaycastHit hit;

        private void Start()
        {
            m_lineRenderer.startWidth = 0.1f;
            m_lineRenderer.endWidth = 0.1f;
            
        }

        public void NewDraw(Vector3 startPos, Vector3 currentpos, Vector3 gravity, Vector3 startVelocity)
        {
            trajectoryPoints.Clear();
            trajectoryPoints.Capacity = 0;

            float time = 0f;
            bool found = false;
            Vector3 prevPos = startPos;
            int count = 0;
            //int halfPoints = m_resolution;
            for (int i = 0; i < m_resolution; i++, count++)
            {
                Vector3 newPos = startPos + startVelocity * time + 0.5f * gravity * time * time;
                trajectoryPoints.Add(newPos);
                time += m_timeStep;

                Vector3 dir = newPos - prevPos;
                prevPos = newPos;
                center = prevPos;

                if (IsHit(prevPos, dir, out RaycastHit hitinfo) && !found)
                {
                    hit = hitinfo;
                    found = true;
                    PopulateLineRenderer(++count);
                    m_HitDecal.SetActive(true);
                    m_HitDecal.transform.position = hit.point + hit.normal * 0.5f;
                    return;
                }
                else
                {
                    m_HitDecal.SetActive(false);
                }
            }

            PopulateLineRenderer(count);
        }

        private void PopulateLineRenderer(int count)
        {
            m_lineRenderer.positionCount = count;
            m_lineRenderer.SetPositions(trajectoryPoints.ToArray());
        }

        private bool IsHit(Vector3 origin,Vector3 direction, out RaycastHit hitinfo)
        {
            return Physics.SphereCast(origin,0.3f,direction.normalized, out hitinfo,1f, mask);
        }

        //private void OnDrawGizmos()
        //{
        //    Gizmos.color = Color.red;
        //    Gizmos.DrawSphere(hit.point, 0.3f);
        //}
    }

}
