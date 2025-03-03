using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] Transform[] patrols;

    NavMeshAgent agent;

    enum States { Idol, Patrol, Chase, Attack};
    States currentState = States.Patrol;
    int currentPatrol = 0;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.autoBraking = false;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        goToNextPatrolPoint();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case States.Patrol:
                Patrol();
                break;
        }            
    }

    void Patrol()
    {
        if(!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            goToNextPatrolPoint();
        }
    }

    void goToNextPatrolPoint()
    {
        agent.SetDestination(patrols[currentPatrol].position);
        currentPatrol = (currentPatrol + 1) % patrols.Length;
    }
}
