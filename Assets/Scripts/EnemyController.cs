using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject patrolPoints;
    [SerializeField] Transform[] patrols;
    [SerializeField] GameObject player;
    [SerializeField] float chaseRadius = 8f;
    [SerializeField] float attackRadius = 2f;
    [SerializeField] int attackPower = 10;
    [SerializeField] float attackRate = 1.5f;
    [SerializeField] float firstAttackDelay = 0.5f;
    [SerializeField] float timeToStayIdle = 1f;
    [SerializeField] int health = 20;
    [SerializeField] Slider slider;
    [SerializeField] GameObject abilityProjectile;
    [SerializeField] Transform abilityWeapon;
    [SerializeField] Transform abilitySpawn;
    [SerializeField] float abilityRate = 3.5f;
    [SerializeField] float abilityCooldown = 0f;

    NavMeshAgent agent;
    Animator anim;

    public enum States { Patrol, Chase, Attack, Idle };
    public States currentState = States.Patrol;
    int currentPatrol = 0;
    int numberOfPatrols = 0;
    float timeUntilNextAttack = 0f;
    float timeUntilPatrol = 0f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        currentState = States.Patrol;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.autoBraking = false;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(patrolPoints == null)
        {
            patrolPoints = GameObject.Find("PatrolPoints");
        }
        numberOfPatrols = patrolPoints.transform.childCount;
        if(patrols.Length != numberOfPatrols)
        {
            patrols = new Transform[numberOfPatrols];
            for (int i = 0; i < numberOfPatrols; i++)
            {
                patrols[i] = patrolPoints.transform.GetChild(i).transform;
            }
        }
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        if (gameManager == null)
        {
            gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        }
        anim.SetBool("IsPatroling", true);
        anim.SetBool("IsChasing", false);
        anim.SetBool("IsAttacking", false);
        anim.SetBool("IsIdle", false);
        slider.maxValue = health;
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
        
        if(currentState == States.Attack || currentState == States.Chase)
        {
            Vector3 dir = abilityWeapon.position - player.transform.position;
            float rotz = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            abilityWeapon.rotation = Quaternion.Euler(0, 0, rotz);

            abilityCooldown += Time.deltaTime;
            if(abilityCooldown >= abilityRate)
            {
                castAbility();
                abilityCooldown = 0f;
            }
        }
    }

    void Patrol()
    {
        anim.SetBool("IsPatroling", true);
        if(!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            goToNextPatrolPoint();
        }
    }

    void goToNextPatrolPoint()
    {
        // Get a random index to a patrol point
        int nextPatrol = Random.Range(0, numberOfPatrols);
        currentPatrol = nextPatrol;
        agent.SetDestination(patrols[nextPatrol].position);
    }

    void Chase()
    {
        anim.SetBool("IsChasing", true);
        if (gameManager.isPlayerDead)
        {
            agent.isStopped = true;
            timeUntilPatrol = 0;
            anim.SetBool("IsChasing", false);
            currentState = States.Idle;
            return;
        }

        agent.SetDestination(player.transform.position);

        if (Vector2.Distance(player.transform.position, transform.position) > chaseRadius)
        {
            agent.isStopped = true;
            timeUntilPatrol = 0;
            abilityCooldown = 0f;
            anim.SetBool("IsChasing", false);
            currentState = States.Idle;
        }
        else if (agent.remainingDistance < attackRadius) {
            anim.SetBool("IsChasing", false);
            currentState = States.Attack;
        }
    }

    void Attack()
    {
        anim.SetBool("IsAttacking", true);
        if (timeUntilNextAttack < firstAttackDelay)
        {
            timeUntilNextAttack += Time.deltaTime;
        }
        else if(Time.time > timeUntilNextAttack)
        {
            if (!gameManager.isPlayerDead)
            {
                player.GetComponent<PlayerCombat>().TakeDamage(attackPower);
                timeUntilNextAttack = Time.time + attackRate;
            }
            
            if(gameManager.isPlayerDead)
            {
                agent.isStopped = true;
                timeUntilPatrol = 0;
                anim.SetBool("IsAttacking", false);
                currentState = States.Idle;
                return;
            }
        }

        agent.SetDestination(player.transform.position);
        if (agent.remainingDistance > attackRadius)
        {
            timeUntilNextAttack = 0f;
            anim.SetBool("IsAttacking", false);
            currentState = States.Chase;
        }
    }

    void Idle()
    {
        anim.SetBool("IsIdle", true);
        if (timeUntilPatrol < timeToStayIdle)
        {
            timeUntilPatrol += Time.deltaTime;
        }
        else
        {
            timeUntilPatrol = 0f;
            currentState = States.Patrol;
            agent.isStopped = false;
            anim.SetBool("IsIdle", false);
            goToNextPatrolPoint();
        }
    }

    void castAbility()
    {
        Instantiate(abilityProjectile, abilitySpawn.position, Quaternion.identity);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        SetCurrentHealth(health);
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    void SetCurrentHealth(int currentHealth)
    {
        slider.value = currentHealth;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            anim.SetBool("IsPatroling", false);
            if(currentState != States.Chase || currentState != States.Attack)
            {
                agent.isStopped = false;
                currentState = States.Chase;
            }
        }
    }
}
