using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float maxSpeed = 10f;
    [Range(1, 2)]
    [SerializeField] float deAcceleration = 1.2f;
    [SerializeField] Rigidbody2D body;

    Animator anim;
    InputAction move;

    // The direction the players across the x axis
    float xDir = Mathf.Sin(Mathf.Deg2Rad * 105f);
    float yDir = Mathf.Sin(Mathf.Deg2Rad * 150f);

    Vector2 NorthEast;
    Vector2 NorthWest;
    Vector2 SouthWest;
    Vector2 SouthEast;

    private void Awake()
    {
        if(body == null)
        {
            body = GetComponentInChildren<Rigidbody2D>();
        }

        anim = GetComponent<Animator>();
        move = InputSystem.actions.FindAction("Player/Move");
        NorthEast = new Vector2(xDir, yDir).normalized;
        NorthWest = new Vector2(-xDir, yDir).normalized;
        SouthWest = new Vector2(-xDir, -yDir).normalized;
        SouthEast = new Vector2(xDir, -yDir).normalized;
    }

    private void FixedUpdate()
    {
        ProcessMovement();
    }

    void ProcessMovement()
    {
        Vector2 input = move.ReadValue<Vector2>();
        if(move.IsPressed())
        {
            anim.SetBool("IsMoving", true);
        }

        if (input.y > 0)
        {
            body.linearVelocity = NorthEast * maxSpeed;
        } else if(input.x < 0)
        {
            body.linearVelocity = NorthWest * maxSpeed;
        }
        else if (input.y < 0)
        {
            body.linearVelocity = SouthWest * maxSpeed;
        }
        else if (input.x > 0)
        {
            body.linearVelocity = SouthEast * maxSpeed;
        }
        {            
            if (body.linearVelocity.magnitude < 1f)
            {
                anim.SetBool("IsMoving", false);
                body.linearVelocity = Vector2.zero;
            }
            body.linearVelocity /= deAcceleration;
        }
    }
}
