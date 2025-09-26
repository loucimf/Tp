using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentScript : MonoBehaviour
{
    NavMeshAgent agent;
    [SerializeField] Transform targets;
    [SerializeField] Animator anim;
    [SerializeField] float velocity;

    

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    
    // Update is called once per frame
    void Update()
    {
        agent.destination = targets.position;
        velocity = agent.velocity.magnitude;
        anim.SetFloat("Speed",velocity);
    }
}
