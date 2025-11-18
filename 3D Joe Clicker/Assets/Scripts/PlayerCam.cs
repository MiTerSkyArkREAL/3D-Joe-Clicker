using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCam : MonoBehaviour
{
    [SerializeField]
    private float sensX = 30;
    private float sensY = 30;

    [SerializeField]
    private Transform orientation;

    private float xRotation, yRotation;

    private float mouseX, mouseY;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
    }

    public void OnMouseX(InputAction.CallbackContext context)
    {
        mouseX = context.ReadValue<float>() * Time.deltaTime * sensX;
    }

    public void OnMouseY(InputAction.CallbackContext context)
    {
        mouseY = context.ReadValue<float>() * Time.deltaTime * sensY;
    }
}
