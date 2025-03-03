using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float maxSpeed = 10f;
    [Range(1, 2)]
    [SerializeField] float deAcceleration = 1.2f;

    Rigidbody2D body;

    // The direction the players across the x axis
    float xDir = Mathf.Sin(Mathf.Deg2Rad * 105f);
    float yDir = Mathf.Sin(Mathf.Deg2Rad * 150f);

    Vector2 NorthEast;
    Vector2 NorthWest;
    Vector2 SouthWest;
    Vector2 SouthEast;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();

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
        if(Keyboard.current.wKey.isPressed)
        {
            body.linearVelocity = NorthEast * maxSpeed;
        } else if(Keyboard.current.aKey.isPressed)
        {
            body.linearVelocity = NorthWest * maxSpeed;
        }
        else if (Keyboard.current.sKey.isPressed)
        {
            body.linearVelocity = SouthWest * maxSpeed;
        }
        else if (Keyboard.current.dKey.isPressed)
        {
            body.linearVelocity = SouthEast * maxSpeed;
        }
        {
            if(body.linearVelocity.magnitude < 1f)
            {
                body.linearVelocity = Vector2.zero;
            }
            body.linearVelocity /= deAcceleration;
        }
    }
}
