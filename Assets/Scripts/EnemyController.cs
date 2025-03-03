using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] Transform[] patrols;
    [SerializeField] GameObject player;
    [SerializeField] float chaseRadius = 8f;
    [SerializeField] float attackRadius = 2f;
    [SerializeField] float attackPower = 10f;
    [SerializeField] float attackRate = 1.5f;
    [SerializeField] float firstAttackDelay = 0.5f;
    [SerializeField] float timeToStayIdle = 1f;
    [SerializeField] int health = 20;
    NavMeshAgent agent;

    public enum States { Patrol, Chase, Attack, Idle };
    public States currentState = States.Patrol;
    int currentPatrol = 0;
    float timeUntilNextAttack = 0f;
    float timeUntilPatrol = 0f;

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
                Attack();
                break;
            case States.Idle:
                Idle();
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
            agent.isStopped = true;
            timeUntilPatrol = 0;
            currentState = States.Idle;
        }
        else if (agent.remainingDistance < attackRadius) {
            currentState = States.Attack;
        }
    }

    void Attack()
    {
        if(timeUntilNextAttack < firstAttackDelay)
        {
            timeUntilNextAttack += Time.deltaTime;
        }
        else if(Time.time > timeUntilNextAttack)
        {
            player.GetComponent<PlayerController>().takeDamage(attackPower);
            timeUntilNextAttack = Time.time + attackRate;
        }

        agent.SetDestination(player.transform.position);
        if (agent.remainingDistance > attackRadius)
        {
            timeUntilNextAttack = 0f;
            currentState = States.Chase;
        }
    }

    void Idle()
    {
        if(timeUntilPatrol < timeToStayIdle)
        {
            timeUntilPatrol += Time.deltaTime;
        }
        else
        {
            timeUntilPatrol = 0f;
            currentState = States.Patrol;
            agent.isStopped = false;
            goToNextPatrolPoint();
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(currentState != States.Chase || currentState != States.Attack)
            {
                agent.isStopped = false;
                currentState = States.Chase;
            }
        }
    }
}
