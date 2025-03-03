using UnityEngine;
using UnityEngine.InputSystem;

public class Projectile : MonoBehaviour
{
    [SerializeField] Camera cam;
    Mouse mouse = Mouse.current;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(mouse.position.ReadValue());
        Vector3 mouseCord = cam.ScreenToWorldPoint(mouse.position.ReadValue());
        Debug.Log(mouseCord);
    }
}
