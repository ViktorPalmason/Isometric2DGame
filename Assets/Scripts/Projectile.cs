using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class Projectile : MonoBehaviour
{
    [SerializeField] float speed = 20f;
    [SerializeField] Camera mainCam;
    [SerializeField] float distance = 10f;
    [SerializeField] int attackPower = 5;
    
    Rigidbody2D body;

    Vector3 startPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        body = GetComponent<Rigidbody2D>();
        startPos = transform.position;
        Vector3 mousePos = mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector3 dir = mousePos - transform.position;
        body.linearVelocity = new Vector2(dir.x, dir.y).normalized * speed;
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(startPos, transform.position) > distance)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            collision.collider.GetComponent<EnemyController>().TakeDamage(attackPower);
        }

        Destroy(gameObject, 0.03f);
    }
}
