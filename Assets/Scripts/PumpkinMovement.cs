using System.Collections;
using UnityEngine;

[RequireComponent (typeof(Pumpkin))]
public class PumpkinMovement : MonoBehaviour
{
    [SerializeField] private ForceMode ForceMode;
    [SerializeField] private float delayTimeToMoveTowardsPlayer = 2f;

    private Rigidbody rb;
    private Coroutine coroutine;

    private static Transform s_PlayerTrasform = null;
    private static float s_launchForce = 5f;
    private static float s_moveSpeed = 7f;



    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        if(s_PlayerTrasform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null )
            {
                s_PlayerTrasform = player.transform;
               
            }
        }
    }

    public void ResetAction()
    {
        StopCoroutine(coroutine);
    }

    public void LaunchPumpkin(Transform SpawnPoint)
    {
        rb.velocity = Vector3.zero;
        rb.AddForce(s_launchForce * SpawnPoint.forward, ForceMode);

        coroutine = StartCoroutine(ProceedTowardsPlayer());
    }

    private IEnumerator ProceedTowardsPlayer()
    {
        yield return new WaitForSeconds(delayTimeToMoveTowardsPlayer);
        rb.velocity = Vector3.zero;
        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, s_PlayerTrasform.position, Time.deltaTime * s_moveSpeed);
            yield return null;
        }
    }

    public static void AdjustParams(float moveSpeed, float launchForce = 5f)
    {
        s_launchForce = launchForce;
        s_moveSpeed = moveSpeed;
    }

   
}
