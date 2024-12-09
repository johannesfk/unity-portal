using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class CameraMove : MonoBehaviour
{

    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction lookAction;
    private Vector2 moveInput;
    private Vector2 lookInput;

    private const float moveSpeed = 7.5f;
    private const float cameraSpeed = 0.5f;

    public Quaternion TargetRotation { private set; get; }
    private Vector3 targetEuler;
    private Vector3 moveVector = Vector3.zero;
    private float moveY = 0.0f;

    private new Rigidbody rigidbody;


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
        Gizmos.DrawRay(transform.position, transform.forward * 2);
    }

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        if (playerInput != null)
        {
            moveAction = playerInput.actions["Move"];
            lookAction = playerInput.actions["Look"];
        }

        Cursor.lockState = CursorLockMode.Locked;
        TargetRotation = transform.rotation;
    }

    private void OnEnable()
    {
        moveAction?.Enable();
        lookAction?.Enable();

        if (moveAction != null)
        {
            moveAction.performed += HandleMove;
            moveAction.canceled += HandleMove;
        }

        if (lookAction != null)
        {
            lookAction.performed += HandleLook;
            lookAction.canceled += HandleLook;
        }
    }

    private void OnDisable()
    {
        moveAction?.Disable();
        lookAction?.Disable();

        if (moveAction != null)
        {
            moveAction.performed -= HandleMove;
            moveAction.canceled -= HandleMove;
        }

        if (lookAction != null)
        {
            lookAction.performed -= HandleLook;
            lookAction.canceled -= HandleLook;
        }
    }

    private void HandleMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        moveVector = new Vector3(moveInput.x, 0.0f, moveInput.y) * moveSpeed;
    }

    private void HandleLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
        var rotation = new Vector2(-lookInput.y, lookInput.x);
        targetEuler = TargetRotation.eulerAngles + (Vector3)rotation * cameraSpeed;
        TargetRotation = Quaternion.Euler(targetEuler);
    }

    private void Update()
    {
        // Apply smooth camera rotation
        if (targetEuler.x > 180.0f)
        {
            targetEuler.x -= 360.0f;
        }
        targetEuler.x = Mathf.Clamp(targetEuler.x, -75.0f, 75.0f);

        transform.rotation = Quaternion.Slerp(transform.rotation, TargetRotation,
            Time.deltaTime * 15.0f);
    }

    /* private void Update()
    {
        // Rotate the camera.
        var rotation = new Vector2(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"));
        var targetEuler = TargetRotation.eulerAngles + (Vector3)rotation * cameraSpeed;
        if (targetEuler.x > 180.0f)
        {
            targetEuler.x -= 360.0f;
        }
        targetEuler.x = Mathf.Clamp(targetEuler.x, -75.0f, 75.0f);
        TargetRotation = Quaternion.Euler(targetEuler);

        transform.rotation = Quaternion.Slerp(transform.rotation, TargetRotation,
            Time.deltaTime * 15.0f);

        // Move the camera.
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        moveVector = new Vector3(x, 0.0f, z) * moveSpeed;

        moveY = Input.GetAxis("Elevation");
    }
 */
    private void FixedUpdate()
    {
        Vector3 newVelocity = transform.TransformDirection(moveVector);
        newVelocity.y += moveY * moveSpeed;
        rigidbody.linearVelocity = newVelocity;
    }

    public void ResetTargetRotation()
    {
        TargetRotation = Quaternion.LookRotation(transform.forward, Vector3.up);
    }
}
