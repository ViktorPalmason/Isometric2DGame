using Unity.VisualScripting;
using UnityEngine;

public class EnemyAbility : MonoBehaviour
{
    [SerializeField] float abilityForce = 5f;
    [SerializeField] float abilityRange = 10f;
    [SerializeField] int abilityPower = 25;

    Rigidbody2D body;
    Transform playerTransform;

    Vector3 startPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        startPos = transform.position;

        Vector3 dir = playerTransform.position - transform.position;
        body.linearVelocity = new Vector2(dir.x, dir.y).normalized * abilityForce;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(startPos, transform.position) > abilityRange)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.collider.GetComponent<PlayerCombat>().TakeDamage(abilityPower);
        }

        Destroy(gameObject, 0.03f);
    }
}
