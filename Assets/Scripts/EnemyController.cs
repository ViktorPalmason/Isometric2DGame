using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] Transform[] patrols;
    [SerializeField] GameObject player;
    [SerializeField] float chaseRadius = 8f;

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
            case States.Chase:
                Chase();
                break;
            case States.Attack:
                break;
            case States.Idol:
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

    void Chase()
    {
        agent.SetDestination(player.transform.position);
        if (Vector2.Distance(player.transform.position, transform.position) > chaseRadius)
        {
            currentState = States.Patrol;
            goToNextPatrolPoint();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(currentState != States.Chase || currentState != States.Attack)
            {
                currentState = States.Chase;
            }
        }
    }
}
