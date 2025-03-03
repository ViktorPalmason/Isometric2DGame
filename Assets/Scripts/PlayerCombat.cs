using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] GameObject projectile;

    Mouse mouse = Mouse.current;
    InputAction attack;
    private void Awake()
    {
        if (cam == null)
        {
            cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }

        attack = InputSystem.actions.FindAction("Player/Attack");
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(attack.WasPressedThisFrame())
        {
            Shoot();
        }
    }

    void Shoot()
    {
        //Vector3 mouseCord = cam.ScreenToWorldPoint(mouse.position.ReadValue());
        //Debug.Log(mouseCord);
        Instantiate(projectile, transform.position + Vector3.right, Quaternion.identity);
    }
}
