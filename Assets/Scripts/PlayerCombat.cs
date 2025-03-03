using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] GameObject projectile;
    [SerializeField] Transform weapon;
    [SerializeField] Transform spawn;

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
        Vector3 mouseCord = cam.ScreenToWorldPoint(mouse.position.ReadValue());
        Vector3 dir = mouseCord - weapon.position;
        float rotz = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        weapon.rotation = Quaternion.Euler(0, 0, rotz);

        if (attack.WasPressedThisFrame())
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Vector3 mouseCord = cam.ScreenToWorldPoint(mouse.position.ReadValue());
        Vector3 dir = spawn.transform.position - mouseCord;
        float rotz = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        Instantiate(projectile, spawn.position, Quaternion.Euler(0, 0, rotz + 90f));
    }
}
