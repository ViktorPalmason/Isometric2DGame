using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] Camera cam;
    Mouse mouse = Mouse.current;

    private void Awake()
    {
        if (cam == null)
        {
            cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mouseCord = cam.ScreenToWorldPoint(mouse.position.ReadValue());
        Debug.Log(mouseCord);
    }
}
