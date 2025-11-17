using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private Transform orientation;

    private Vector3 moveDir;

    private Rigidbody rb;

    private float horizontalInput, verticalInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 movementInput = context.ReadValue<Vector2>();
        horizontalInput = movementInput.x;
        verticalInput = movementInput.y;
    }

    private void MovePlayer()
    {
        moveDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        rb.AddForce(moveDir.normalized * moveSpeed * 10f);
    }
}
