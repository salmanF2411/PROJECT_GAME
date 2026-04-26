using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementController : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 currentInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rb.velocity = currentInput * moveSpeed;
    }

    // dipanggil otomatis oleh PlayerInput
    void OnMove(InputValue value)
    {
        currentInput = value.Get<Vector2>();
    }
}