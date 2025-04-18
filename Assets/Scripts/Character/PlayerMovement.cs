using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public RandomEncounter randomEncounter;

    private Rigidbody2D rb;
    private Vector2 movement;
    private PlayerControls controls;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        randomEncounter=this.gameObject.GetComponent<RandomEncounter>();
    }

    private void Awake()
    {
        controls = new PlayerControls();

        controls.Player.Move.performed += ctx => movement = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => movement = Vector2.zero;
    }

    private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    void Update()
    {
        Vector2 input = controls.Player.Move.ReadValue<Vector2>();

        if (input != Vector2.zero)
        {
            animator.SetBool("isRunning", true);
            randomEncounter.SetWalking(true);
        }

        else
        {
            animator.SetBool("isRunning", false);
            randomEncounter.SetWalking(false);
        }

        if (input.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);   

        }
        else if (input.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);  

        }

    }
}
