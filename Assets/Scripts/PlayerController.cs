using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Sirenix.OdinInspector;
using Unity.Mathematics;

public class PlayerController : MonoBehaviour
{
    private GameInputs _input;
    private InputAction _moveAction;
    private InputAction _lookAction;
    
    private Vector2 _moveInput;
    private Vector2 _lookInput;
    private Vector3 _lastInput;

    private Vector3 _leftArmOffset, _rightArmOffset, _backpackOffset;

    private float _currentTopSpeed;
    
    [Header("References")]
    public CharacterController charControl;
    public Transform affectedTransform;
    [SerializeField] private Transform leftArm;
    [SerializeField] private Transform rightArm;
    [SerializeField] private Transform backPack;


    [Header("Camera")] 
    [SerializeField] private Transform cameraTarget;

    [PropertyRange(0,360)]
    [SerializeField] private float cameraHorizontalSpeed = 200f;
    [PropertyRange(0,360)]

    [SerializeField] private float cameraVerticalSpeed = 200f;

    [SerializeField] [MinMaxSlider(-89f, 89f)]
    private Vector2 verticalCameraRotationClamp;
    
    private Quaternion _characterTargetRotation;
    private Vector2 _cameraRotation;

    [Header("Mouse")] 
    [ProgressBar(0,5f)]
    [SerializeField] private float mouseCameraSensitivity = 1f;
    
    [Header("Controller")] 
    [ProgressBar(0,5f)]
    [SerializeField] private float controllerCameraSensitivity = 1f;

    [SerializeField] private bool invertY = false;
    [SerializeField] private bool invertX = false;

    [Header("Movement")] 
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float acceleration, runningAcceleration, decceleration;
    [SerializeField] private float topSpeedWalking, topSpeedRunning, topSpeedGliding, topSpeedBackwards;
    [SerializeField] private MovementState moveState;
    [SerializeField] private bool backingUp;
    [SerializeField] private LayerMask groundMask;

    [Header("Interactions")]
    [SerializeField] private Interactable selectedInteractable;
    private enum MovementState
    {
        Walking,
        Running,
        Gliding
    }
    
    private Vector3 _currentVelocity;
    
    void Awake()
    {
        _input = new GameInputs();
        _moveAction = _input.Player.Move;
        _lookAction = _input.Player.Look;
        _input.Player.Sprint.started += (InputAction.CallbackContext _) => { if (moveState == MovementState.Walking)  moveState = MovementState.Running; };
        _input.Player.Sprint.canceled += (InputAction.CallbackContext _) => { if (moveState == MovementState.Running) moveState = MovementState.Walking; };
        _input.Player.ContinueText.started += (InputAction.CallbackContext _) => { DialogUIManager.Instance.ContinueText();};

        _input.Player.Glide.performed += ToggleGlide;

        _input.Player.Interact.performed += Interact;


        _leftArmOffset = affectedTransform.position - leftArm.position;
        _rightArmOffset = affectedTransform.position - rightArm.position;
        _backpackOffset = affectedTransform.position - backPack.position;

        Application.targetFrameRate = -1;

    }

    private void Start()
    {
        GameManager.Instance.onPlayStateChange += ReactToPlaystateChange;
    }

    private void OnEnable()
    {
        _input.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();
    }
    
    private void ReactToPlaystateChange(GameManager.PlayState newState)
    {

        switch (newState)
        {
            case GameManager.PlayState.Game:
                _input.Enable();
                break;
            case GameManager.PlayState.Cutscene:
                //_input.Disable();
                break;
            case GameManager.PlayState.Paused:
                //_input.Disable();
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        if (GameManager.Instance.currentPlayState != GameManager.PlayState.Game)
            return;

        StickToGround();
        ChangeTopSpeed();

        _moveInput = _moveAction.ReadValue<Vector2>();
        _lookInput = _lookAction.ReadValue<Vector2>();

        if (moveState != MovementState.Gliding)
        {
            var angle = AngleToMovement();

            switch (angle)
            {
                case < 30 and > -30:
                    WalkMove(_moveInput, movementSpeed);
                    RotateToInput(angle);
                    backingUp = false;

                    break;
                case < 90 and > -90:
                    WalkMove(_moveInput, movementSpeed / 2);
                    RotateToInput(angle);
                    backingUp = false;

                    break;
                case < 160 and > -160:
                    BreakVelocity();
                    RotateToInput(angle);
                    backingUp = false;

                    break;
                case < 180 and > -180:

                    if (Vector3.Angle(_currentVelocity, affectedTransform.forward) > 160 || _currentVelocity == Vector3.zero)
                    {
                        WalkMove(_moveInput, -movementSpeed / 2);
                        backingUp = true;
                    }
                    else
                    {
                        BreakVelocity();
                    }

                    break;
            }

            
        }
        else
        {
            WalkMove(Vector2.one, movementSpeed);

            var angle = AngleToMovement();

            if (_moveInput != Vector2.zero)
            {
                switch (angle)
                {
                    case > 0:
                        affectedTransform.RotateAround(affectedTransform.position, Vector3.up, -rotationSpeed * Time.deltaTime / 10);

                        break;
                    case < 0:
                        affectedTransform.RotateAround(affectedTransform.position, Vector3.up, rotationSpeed * Time.deltaTime / 10);
                        break;
                }
            }

            


        }

        LeanCharacterToVelocity();
        VelocityCalc();
        MoveAttachments();
        RotateCamera(_lookInput);
    }

    private void MoveAttachments()
    {
        leftArm.position = affectedTransform.position + _leftArmOffset;
        rightArm.position = affectedTransform.position + _rightArmOffset;
        backPack.position = affectedTransform.position + _backpackOffset;

    }

    private float AngleToMovement()
    {
        var movement = WorldDirectionMoveInput(_moveInput);
        var bodyDirection = affectedTransform.forward;
        
        bodyDirection.y = 0;
        bodyDirection.Normalize();
        
        var angle = Vector3.Angle(movement,bodyDirection);
        var direction = Vector3.Cross(movement, bodyDirection).y;

        if (direction > 0) direction = 1;
        if (direction < 0) direction = -1;

        angle *= direction;
        
        return angle; 
    }

    private void ChangeTopSpeed()
    {
        float goalSpeed = moveState == MovementState.Walking ? topSpeedWalking : moveState == MovementState.Running ? topSpeedRunning : topSpeedGliding;

        if (_currentTopSpeed < goalSpeed)
            _currentTopSpeed += acceleration * Time.deltaTime;

        if (_currentTopSpeed > goalSpeed)
            _currentTopSpeed -= decceleration * Time.deltaTime;

        if(Mathf.Abs(_currentTopSpeed - goalSpeed) < 0.5f)
            _currentTopSpeed = goalSpeed;
    }

    private void RotateToInput(float angle)
    {
        if (angle > 4f)
        {
            affectedTransform.RotateAround(affectedTransform.position, Vector3.up, -rotationSpeed * Time.deltaTime);
        }else if (angle < -4f)
        {
            affectedTransform.RotateAround(affectedTransform.position, Vector3.up, rotationSpeed * Time.deltaTime);
        }
    }

    private void LeanCharacterToVelocity()
    {
        var bodyDirection = affectedTransform.forward;
        
        bodyDirection.y = 0;
        bodyDirection.Normalize();
        
        var angle = Vector3.Angle(_currentVelocity, bodyDirection);
        
        var veloForwardMult = Vector3.Angle(_currentVelocity, affectedTransform.right);
        var direction = Vector3.Cross(_currentVelocity, bodyDirection).y;

        if (direction > 0) direction = 0;
        if (direction < 0) direction = -180;

        veloForwardMult += direction;
        veloForwardMult /= 90;
        veloForwardMult = Mathf.Abs(veloForwardMult);
        veloForwardMult -= 2;
        veloForwardMult = Mathf.Abs(veloForwardMult);

        if (veloForwardMult == 2) veloForwardMult = 0;

            veloForwardMult *= 1.5f;
        
        if (angle > 90)
        {
            affectedTransform.rotation = Quaternion.Euler(-_currentVelocity.magnitude * veloForwardMult * 2,affectedTransform.eulerAngles.y,affectedTransform.eulerAngles.z);
        }
        else
        {
            affectedTransform.rotation = Quaternion.Euler(_currentVelocity.magnitude* veloForwardMult * 2,affectedTransform.eulerAngles.y,affectedTransform.eulerAngles.z);
        }
    }

    private void BreakVelocity()
    {
        var decc = moveState == MovementState.Gliding  ? decceleration * 0.1f : decceleration;
        var deccelerationForce = _currentVelocity.normalized * decc * Time.deltaTime;
        if (deccelerationForce.magnitude > _currentVelocity.magnitude) deccelerationForce = _currentVelocity;
        if(_currentVelocity.magnitude > 0.05f) _currentVelocity -= deccelerationForce;
        else _currentVelocity = Vector3.zero;
    }

    private void WalkMove(Vector2 moveInput, float moveSpeed)
    {
        //Input to RelativeInput
        var bodyDirection = affectedTransform.forward;
        
        bodyDirection.y = 0;
        bodyDirection.Normalize();
        //- Input to RelativeInput

        if (moveInput != Vector2.zero)
        {
            _currentVelocity += bodyDirection * (moveState == MovementState.Walking? acceleration : runningAcceleration) * Time.deltaTime * moveSpeed;
        }
        else
        {
            BreakVelocity();
        }

    }

    private void ToggleGlide(InputAction.CallbackContext ctx)
    {
        if(moveState != MovementState.Gliding)
        {
            moveState = MovementState.Gliding;
        }
        else
        {
            moveState = MovementState.Walking;
            _lastInput = Vector2.zero;
        }   
    }

    private void VelocityCalc()
    {
        var velocityCap = backingUp ? topSpeedBackwards : _currentTopSpeed;
        
        if (_currentVelocity.magnitude > velocityCap)
        {
            _currentVelocity.Normalize();
            _currentVelocity *= velocityCap;
        }

        if (_currentVelocity.magnitude > 0) _characterTargetRotation = Quaternion.LookRotation(_currentVelocity, Vector3.up);



        charControl.SimpleMove(_currentVelocity);
    }

    private void StickToGround()
    {
        
        float snapDistance = 1f;
        if (charControl.isGrounded == false)
        {
            RaycastHit hitInfo = new RaycastHit();
            if (Physics.Raycast(new Ray(affectedTransform.position, Vector3.down), out hitInfo, snapDistance))
                charControl.Move(hitInfo.point - affectedTransform.position);
        }
    }

    private Vector3 WorldDirectionMoveInput(Vector2 moveInput)
    {
        Vector3 inputDir = new Vector3(moveInput.x, 0, moveInput.y).normalized;
        Vector3 worldInputDir = cameraTarget.TransformDirection(inputDir);
        worldInputDir.y = 0;
        worldInputDir.Normalize();

        if (worldInputDir != Vector3.zero) _lastInput = worldInputDir;
        Vector3 movement = _lastInput;
        return movement;
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
        if (_lookAction.activeControl == null)
        {
            Debug.LogWarning("No active control scheme detected");
            return false;
        }
        
        bool isMouseInput = _lookAction.activeControl.name == "delta";
        return isMouseInput;
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
        if (selectedInteractable != null)
        {
            selectedInteractable.Interact();
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

        if (interactable == null)
            return;

        if (selectedInteractable != null)
            selectedInteractable.Deselect();

        selectedInteractable = interactable;
        interactable.Select();
    }

    public void EnableInput()
    {
        _input.Enable();
    }

    public void DisableInput()
    {
       // _input.Disable();
    }

    private void TryDeselectInteractable(Collider other)
    {
        Interactable interactable = other.GetComponent<Interactable>();

        if (interactable == null)
            return;

        if (interactable == selectedInteractable)
        {
            selectedInteractable.Deselect();
            selectedInteractable = null;
        }
    }
}
