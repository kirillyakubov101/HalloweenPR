using UnityEngine;

namespace HalloweenPR.Core
{
    public class Launcher : MonoBehaviour
    {
        public float launchForce = 10f;
        public Vector3 launchDirection = Vector3.forward;
        public float sphereRadius = 0.5f;
        public int resolution = 30;  // Number of points in the trajectory
        public float timeStep = 0.1f; // Time between points
        public float maxSimulationTime = 5f;  // Maximum simulation time (seconds)

        private LineRenderer lineRenderer;
        private bool isLaunched = false;
        private Rigidbody rb;  // Reference to the Rigidbody component

        void Start()
        {
            // Add LineRenderer component for visualizing the trajectory
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;

            // Get the Rigidbody component
            rb = GetComponent<Rigidbody>();

            if (rb == null)
            {
                rb = gameObject.AddComponent<Rigidbody>();  // Add a Rigidbody if none exists
            }

            // Disable gravity initially, so the object doesn't fall before launch
            rb.useGravity = false;
        }

        void Update()
        {
            if (isLaunched) { return; }

            DrawTrajectory();

            // Check for space bar press and launch if not already launched
            if (Input.GetKeyDown(KeyCode.Space) && !isLaunched)
            {
                Launch();
            }
        }

        void Launch()
        {
            isLaunched = true;

            // Reset any velocity the Rigidbody might have
            rb.velocity = Vector3.zero;

            // Enable gravity after launching
            rb.useGravity = true;

            // Apply force in the launch direction using Rigidbody
            Vector3 launchVelocity = launchDirection.normalized * launchForce;
            rb.AddForce(launchVelocity, ForceMode.VelocityChange); // Apply the force immediately
        }

        void DrawTrajectory()
        {
            // Set the initial velocity from the launch force and direction
            Vector3 initialVelocity = launchDirection.normalized * launchForce;
            Vector3 startPosition = transform.position;

            // Ensure the LineRenderer has the correct number of points
            lineRenderer.positionCount = resolution;

            // Store points of the trajectory
            Vector3[] trajectoryPoints = new Vector3[resolution];

            // Loop to calculate the trajectory points based on time steps
            for (int i = 0; i < resolution; i++)
            {
                float time = i * timeStep;
                if (time > maxSimulationTime) break;

                // Calculate position at time 't' using projectile motion equations
                Vector3 newPosition = CalculatePositionAtTime(startPosition, initialVelocity, time);
                trajectoryPoints[i] = newPosition;
            }

            // Set the points to the LineRenderer for visualization
            lineRenderer.SetPositions(trajectoryPoints);
        }

        // Function to calculate the position of the sphere at a given time
        Vector3 CalculatePositionAtTime(Vector3 startPosition, Vector3 initialVelocity, float time)
        {
            Vector3 gravity = Physics.gravity; // Use Unity's gravity
            Vector3 position = startPosition + initialVelocity * time + 0.5f * gravity * time * time;
            return position;
        }

        // Optional: Draw gizmos for better visualization in the scene view
        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, sphereRadius);
        }
    }
}
