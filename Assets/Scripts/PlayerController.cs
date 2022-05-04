using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Sirenix.OdinInspector;
using Unity.Mathematics;

public class PlayerController : MonoBehaviour
{
    
    
    private Vector2 _moveInput;
    private Vector2 _lookInput;
    private Vector3 _lastInput;

    private quaternion _characterTargetRotation;
    private Vector2 _cameraRotation;
    
    private GameInputs _input;
    private InputAction _moveAction;
    private InputAction _lookAction;
    
    private CharacterController _charControl;

    private Interactable _selectedInteractable;

    [Header("Camera")] 
    [SerializeField] private Transform cameraTarget;

    [PropertyRange(0,360)]
    [SerializeField] private float cameraHorizontalSpeed = 200f;
    [PropertyRange(0,360)]

    [SerializeField] private float cameraVerticalSpeed = 200f;

    [SerializeField] [MinMaxSlider(-89f, 89f)]
    private Vector2 verticalCameraRotationClamp;

    [Header("Mouse")] 
    [ProgressBar(0,5f)]
    [SerializeField] private float mouseCameraSensitivity = 1f;
    
    [Header("Controller")] 
    [ProgressBar(0,5f)]
    [SerializeField] private float controllerCameraSensitivity = 1f;

    [SerializeField] private bool invertY = false;
    [SerializeField] private bool invertX = false;

    [Header(("Movement"))] 
    [SerializeField][MinValue(0)] private float moveSpeed = 3f;
    [SerializeField][MinValue(0)] private float rotationSpeed = 3f;
    [SerializeField] private AnimationCurve MovementCurve;
    [SerializeField] private float timeTillMaxMoveCurve;
    private float currentMoveCurve;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private float accelerationMult;
    [SerializeField] private float decelerationMult;
    
    private void Awake()
    {
        _input = new GameInputs();
        _moveAction = _input.Player.Move;
        _lookAction = _input.Player.Look;

        _input.Player.Interact.performed += Interact;
        
        _charControl = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        _input.Enable();
    }

    private void Update()
    {
        _moveInput = _moveAction.ReadValue<Vector2>();
        _lookInput = _lookAction.ReadValue<Vector2>();
        Move(_moveInput);
        playerAnimator.SetBool("Grounded", _charControl.isGrounded);

    }

    private void FixedUpdate()
    {
    }

    private void LateUpdate()
    {
        RotateCamera(_lookInput);
    }

    private void OnDisable()
    {
        _input.Disable();
    }

    private void OnDestroy()
    {
        
    }

    private void RotateCamera(Vector2 lookInput)
    {
        if (lookInput != Vector2.zero)
        {

            var isMouseInput = IsMouseLook();

            float deltaTimeMultiplier = isMouseInput ? 1.0f : Time.deltaTime;
            float sensitivity = isMouseInput ? mouseCameraSensitivity : controllerCameraSensitivity;
            
            lookInput *= deltaTimeMultiplier * sensitivity;

            _cameraRotation.x += lookInput.y * cameraVerticalSpeed * (invertY && !isMouseInput ? -1f : 1f);
            _cameraRotation.y += lookInput.x * cameraHorizontalSpeed * (invertX && !isMouseInput ? -1f : 1f);


            
            _cameraRotation.x = NormalizeAngle(_cameraRotation.x);
            _cameraRotation.y = NormalizeAngle(_cameraRotation.y);


            _cameraRotation.x = Mathf.Clamp(_cameraRotation.x, verticalCameraRotationClamp.x, verticalCameraRotationClamp.y);
        }
        

        cameraTarget.rotation = Quaternion.Euler(_cameraRotation.x, _cameraRotation.y, 0f);

    }

    private bool IsMouseLook()
    {
        bool isMouseInput = _lookAction.activeControl.name == "delta";
        return isMouseInput;
    }

    private void Move(Vector2 moveInput)
    {
        playerAnimator.SetFloat("Speed",MovementCurve.Evaluate(currentMoveCurve / timeTillMaxMoveCurve));
        
        Vector3 inputDir = new Vector3(moveInput.x, 0, moveInput.y).normalized;
        Vector3 worldInputDir = cameraTarget.TransformDirection(inputDir);
        worldInputDir.y = 0;
        worldInputDir.Normalize();

        if (worldInputDir != Vector3.zero)
            _lastInput = worldInputDir;
        Vector3 movement = _lastInput * moveSpeed * MovementCurve.Evaluate(currentMoveCurve / timeTillMaxMoveCurve);

        _characterTargetRotation = Quaternion.LookRotation(_lastInput,Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, _characterTargetRotation, Time.deltaTime * rotationSpeed);
        
        if (moveInput != Vector2.zero)
        {
            
            currentMoveCurve += accelerationMult * Time.deltaTime;

            if (currentMoveCurve > timeTillMaxMoveCurve)
                currentMoveCurve = timeTillMaxMoveCurve;
        }
        else
        {
            currentMoveCurve -= Time.deltaTime * decelerationMult;

            if (currentMoveCurve < 0)
                currentMoveCurve = 0;
        }
        
        
        _charControl.SimpleMove(movement);
    }

    private float NormalizeAngle(float angle)
    {
       
        angle %= 360f;

        if (angle < 0)
        {
            angle += 360f;
        }

        if (angle > 180f)
        {
            angle -= 360f;
        }

        return angle;
    }

    private void Interact(InputAction.CallbackContext ctx)
    {
        if (_selectedInteractable != null)
        {
            _selectedInteractable.Interact();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        TrySelectInteractable(other);
    }

    private void OnTriggerExit(Collider other)
    {
        TryDeselectInteractable(other);
    }

    private void TrySelectInteractable(Collider other)
    {
        Interactable interactable = other.GetComponent<Interactable>();
        
        if(interactable == null)
            return;
        
        if(_selectedInteractable != null)
            _selectedInteractable.Deselect();

        _selectedInteractable = interactable;
        interactable.Select();
    }
    
    public void EnableInput()
    {
        _input.Enable();
    }
    
    public void DisableInput()
    {
        _input.Disable();
    }

    private void TryDeselectInteractable(Collider other)
    {
        Interactable interactable = other.GetComponent<Interactable>();
        
        if(interactable == null)
            return;

        if (interactable == _selectedInteractable)
        {
            _selectedInteractable.Deselect();
            _selectedInteractable = null;
        }
    }
}
