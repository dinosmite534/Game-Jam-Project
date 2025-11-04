using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class FirstPersonController : MonoBehaviour
{
    [Header("Movement Speeds")]
    [SerializeField] private float walkSpeed = 3.0f;

    [Header("Jump Parameters")]
    [SerializeField] private float jumpForce = 5.0f;
    [SerializeField] private float gravityMultiplier = 1.0f;

    [Header("Camera Parameters")]
    [SerializeField] private float mouseSensitivity = 0.1f;
    [SerializeField] private float upDownLookRange = 80f;

    [Header("References")]
    [SerializeField] private CharacterController characterController;
    [SerializeReference] private Camera mainCamera;
    [SerializeField] private PlayerInputHandler playerInputHandler;

    private Vector3 currentMovement;
    private float verticalRotation;
    private float currentSpeed;

    [Header("Interaction")]
    [SerializeField] private float distance = 3;
    [SerializeField] private LayerMask mask;
    [SerializeField] private float interactPositionY;   //player eye level otherwise it would be in the floor
    private PlayerUi playerUI;

    [Header("Pause")]
    [SerializeField] private GameObject pauseMenu;
    private bool paused;

    //if i add a sprint
    // private float currentSpeed => walkSpeed * (playerInputHandler.SprintTriggered ? sprintMultiplier : 1);


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        paused = false;
        currentSpeed = walkSpeed;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerUI = GetComponent<PlayerUi>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale != 0)
        {
            HandleMovement();
            HandleRotation();


            //interact
            playerUI.UpdateText(string.Empty);
            Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
            Debug.DrawRay(ray.origin, ray.direction * distance);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, distance, mask))
            {
                if (hitInfo.collider.GetComponent<Interactable>() != null)
                {
                    Interactable interactable = hitInfo.collider.gameObject.GetComponent<Interactable>();
                    playerUI.UpdateText(interactable.promptMessage);

                    if (playerInputHandler.interactTriggered)
                    {
                        interactable.BaseInteract();
                    }
                }
            }
            if (playerInputHandler.pauseTriggered)
            {
                PauseMenu();
            }
        }

    }

    private Vector3 CalculateWorldDirection()
    {
        Vector3 inputDirection = new Vector3(playerInputHandler.MovementInput.x, 0f, playerInputHandler.MovementInput.y);
        Vector3 worldDirection = transform.TransformDirection(inputDirection);
        return worldDirection.normalized;
    }
    private void HandleMovement()
    {
        Vector3 worldDirection = CalculateWorldDirection();
        currentMovement.x = worldDirection.x * currentSpeed;
        currentMovement.z = worldDirection.z * currentSpeed;

        HandleJumping();

        characterController.Move(currentMovement * Time.deltaTime);
    }
    private void ApplyHorizontalRotation(float rotationAmount)
    {
        transform.Rotate(0,rotationAmount,0);
    }
    private void ApplyVerticalRotation(float rotationAmount)
    {
        verticalRotation = Mathf.Clamp(verticalRotation - rotationAmount, -upDownLookRange, upDownLookRange);
        mainCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }
    
    private void HandleRotation()
    {
        float mouseXRotation = playerInputHandler.RotationInput.x * mouseSensitivity;
        float mouseYRotation = playerInputHandler.RotationInput.y * mouseSensitivity;

        ApplyHorizontalRotation(mouseXRotation);
        ApplyVerticalRotation(mouseYRotation);
    }

    private void PauseMenu()
    {
        if (Time.timeScale == 0)
        {
            // Unpause
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            // Pause
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    private void HandleJumping()
    {
        if (characterController.isGrounded)
        {
            currentMovement.y = -0.5f;
            if (playerInputHandler.jumpTriggered)
            {
                currentMovement.y = jumpForce;

            }
        }
        else
        {
            currentMovement.y += Physics.gravity.y * gravityMultiplier * Time.deltaTime;
        }
    }
}
