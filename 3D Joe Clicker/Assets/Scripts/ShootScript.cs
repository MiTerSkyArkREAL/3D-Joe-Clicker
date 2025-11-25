using UnityEngine;
using System.Collections;

public class ShootScript : MonoBehaviour
{
    [Header("Force Settings")]
    public float forceAmount = 20f;
    public float maxDistance = 300f;
    public Camera playerCamera;

    [Header("Recoil Settings")]
    public float recoilAmount = 8f;
    public float recoilReturnSpeed = 12f;

    [Header("Zoom Settings")]
    public float normalFOV = 60f;         // default FOV
    public float zoomFOV = 40f;           // zoomed in FOV
    public float zoomSpeed = 12f;         // how fast it zooms

    private float currentRecoil = 0f;
    private float recoilVelocity = 0f;

    private Quaternion originalRotation;

    void Start()
    {
        originalRotation = transform.localRotation;

        // ensure camera starts at normal FOV
        if (playerCamera != null)
            playerCamera.fieldOfView = normalFOV;
    }

    void Update()
    {
        // --- SHOOTING ---
        if (Input.GetButtonDown("Fire1"))
        {
            ShootForce();
            ApplyRecoil();
        }

        // --- RECOIL ---
        currentRecoil = Mathf.SmoothDamp(
            currentRecoil,
            0f,
            ref recoilVelocity,
            1f / recoilReturnSpeed
        );

        transform.localRotation =
            originalRotation * Quaternion.Euler(0f, 0f, currentRecoil);

        // --- ZOOM ---
        HandleZoom();
    }

    void HandleZoom()
    {
        // Right click held -> zoom
        bool zooming = Input.GetButton("Fire2");

        float targetFOV = zooming ? zoomFOV : normalFOV;

        playerCamera.fieldOfView =
            Mathf.Lerp(playerCamera.fieldOfView, targetFOV, Time.deltaTime * zoomSpeed);
    }

    void ShootForce()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            Rigidbody rb = hit.collider.attachedRigidbody;

            if (rb != null)
            {
                rb.AddForce(ray.direction * forceAmount, ForceMode.Impulse);
            }
        }
    }

    void ApplyRecoil()
    {
        currentRecoil += recoilAmount;
    }
}
