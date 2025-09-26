using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator anim;
    public Transform eyes;

    [Header("Settings")]
    public Transform[] targets;

    [Header("Vision Settings")]
    public float secondsTillAggroDown = 2f;
    public float rayDistance = 6f;
    public int rays = 4;
    public float angle = 40f;

    private NavMeshAgent agent;
    private Transform currentTarget;
    private Transform currentPlayerTarget;
    private int targetNumber;
    private float unseenTimer;
    private float velocity;
    private bool chasing = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        anim.applyRootMotion = false;


        targetNumber = 0;
        unseenTimer = 0f;

        if (targets.Length > 0)
            currentTarget = targets[targetNumber];

    }


    private void Update()
    {
        PatrolBetweenTargets();
        SearchPlayer();
        HandleChase();
        HandleAnim();
    }

    void PatrolBetweenTargets()
    {
        if (!chasing && targets.Length > 0)
        {
            currentTarget = targets[targetNumber];
            agent.destination = currentTarget.position;

            Vector3 difference = transform.position - currentTarget.position;

            if (difference.magnitude <= 1.1f)
            {
                Debug.Log("Llegue al punto numero: " + (targetNumber + 1));
                targetNumber = GetRandomTargetIndex();
                currentTarget = targets[targetNumber];
            }
        }
    }

    void HandleChase()
    {
        if (chasing && currentPlayerTarget != null)
        {
            agent.destination = currentPlayerTarget.position;

            unseenTimer += Time.deltaTime;
            if (unseenTimer >= secondsTillAggroDown)
            {
                Debug.Log("Perdio de vista al jugador");
                chasing = false;
                targetNumber = GetRandomTargetIndex();
                currentTarget = targets[targetNumber];
                agent.destination = currentTarget.position;
            }
        }
    }

    void SearchPlayer()
    {
        for (int i = 0; i < rays; i++)
        {
            float fraction = (float)i / (rays - 1);
            float currentAngle = -angle / 2f + fraction * angle;

            Vector3 dir = Quaternion.Euler(0, currentAngle, 0) * eyes.forward;

            if (Physics.Raycast(eyes.position, dir, out RaycastHit hit, rayDistance))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    currentPlayerTarget = hit.transform;
                    unseenTimer = 0f;
                    chasing = true;
                }
            }
        }
    }

    void HandleAnim()
    {
        if (agent != null && anim != null)
        {
            anim.SetFloat("Velocidad", agent.velocity.magnitude);
        }
    }


    void OnDrawGizmosSelected()
    {
        if (!eyes) return;

        Gizmos.color = Color.red;
        for (int i = 0; i < rays; i++)
        {
            float fraction = (float)i / (rays - 1);
            float currentAngle = -angle / 2f + fraction * angle;

            Vector3 dir = Quaternion.Euler(0, currentAngle, 0) * eyes.forward;
            Gizmos.DrawRay(eyes.position, dir * rayDistance);
        }
    }

    int GetRandomTargetIndex()
    {
        if (targets.Length == 0) return 0;
        return Random.Range(0, targets.Length);
    }
}
