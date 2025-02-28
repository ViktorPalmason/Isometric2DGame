using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float movementSpeed = 10f;

    Rigidbody2D body;
    InputAction move;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        move = InputSystem.actions.FindAction("Player/Move");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        ProcessMovement();
    }

    void ProcessMovement()
    {
        Vector2 input = move.ReadValue<Vector2>();
        body.AddForce(input * movementSpeed * Time.fixedDeltaTime);       
    }
}
