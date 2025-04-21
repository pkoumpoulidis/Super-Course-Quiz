using System.Linq;
using AudioSystem;
using Models;
using SaveSystem;
using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(PlayerInputController))]
public class CharacterMovementController : MonoBehaviour
{
    private static readonly int State = Animator.StringToHash("State");
    private static readonly int Vert = Animator.StringToHash("Vert");

    [Header("Movement Settings")]
    [SerializeField] private float walkSpeedThreshold = 3f;
    [SerializeField] private float runSpeedThreshold = 5.5f;
    [SerializeField] private float acceleration = 4f;
    [SerializeField] private float deceleration = 6f;
    [SerializeField] private float rotationSpeed = 15f;

    [Header("Gravity Settings")]
    [SerializeField] private float gravity = 30f;
    [SerializeField] private float airMovementModifier = 0.5f;
    
    [Header("Setup Settings")]
    [SerializeField] private Animal baseCharacter;

    public Animal BaseCharacter => baseCharacter;
    public Transform CharacterBody => _characterBody;
    public CinemachineVirtualCameraBase CharacterCamera => _freeLookCamera;

    // Components
    private CinemachineVirtualCameraBase _freeLookCamera;
    private CharacterController _characterController;
    private PlayerInputController _inputController;
    private Animator _animator;
    private Transform _characterBody;
    
    // Movement Related fields
    private Vector3 _lastMovementDirection;
    private Vector3 _horizontalVelocity;
    private float _verticalVelocity;
    private float _targetHorizontalVelocity;
    
    private void OnDisable()
    {
        if (!_animator) return;
        _animator.SetFloat(Vert,  0f);
        _animator.SetFloat(State, 0f);
    }

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _inputController = GetComponent<PlayerInputController>();
        baseCharacter = GameManager.Instance.Animals.First(animal => animal.AnimalId == GameManager.Instance.Player.LastSelectedAnimalId);
        InitializeCharacter(baseCharacter);
        SoundManager.Instance.SetGameMusic();
    }
    
    private void Update()
    {
        ToggleRun();
        CalculateVerticalVelocity();
        CalculateHorizontalVelocity();
        ApplyMotion();
        ApplyAnimation();
    }

    private void InitializeCharacter(Animal characterInfo)
    {
        _characterBody = Instantiate(characterInfo.Prefab, transform.position, transform.rotation).transform;
        _characterBody.SetParent(transform);
        _freeLookCamera = Instantiate(characterInfo.Camera, transform.position, transform.rotation);
        _freeLookCamera.Follow = _characterBody.transform;
        QuizManager.Instance.InitializeQuiz(this);
        _characterController = _characterBody.GetComponent<CharacterController>();
        _animator = _characterBody.GetComponent<Animator>();
    }

    private void ApplyAnimation()
    {
        // Normalized to (0,1) for the animator's blend tree
        float normalizedSpeed = Mathf.InverseLerp(
            walkSpeedThreshold, 
            runSpeedThreshold, 
            _horizontalVelocity.magnitude
        );
        
        // Set animator's properties
        _animator.SetFloat(Vert,  _inputController.SmoothMagnitude);
        _animator.SetFloat(State, normalizedSpeed);
    }

    private void LateUpdate()
    {
        ApplyRotation();
    }

    private void ApplyMotion()
    {
        // Combine horizontal with vertical acceleration (gravity)
        Vector3 moveVector = _horizontalVelocity + Vector3.up * _verticalVelocity;
        // Apply the movement to CharacterController
        _characterController.Move(moveVector * Time.deltaTime);
    }
    
    private void ApplyRotation()
    {
        if (GetRelativeDirection() != Vector3.zero)
            _lastMovementDirection = GetRelativeDirection();

        // Smoothly rotate toward movement direction
        if (_lastMovementDirection == Vector3.zero)
            return;
        
        // Calculate target rotation
        Quaternion targetRotation = Quaternion.LookRotation(_lastMovementDirection);
        // Apply Rotation
        _characterBody.rotation = Quaternion.Slerp(_characterBody.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void ToggleRun()
    {
        _targetHorizontalVelocity = _inputController.IsRunning ? runSpeedThreshold : walkSpeedThreshold;
    }
    
    private void CalculateVerticalVelocity()
    {
        bool isGrounded = _characterController.isGrounded;

        if (isGrounded)
            _verticalVelocity = Mathf.Lerp(_verticalVelocity, -0.1f, gravity * Time.deltaTime);
        else
            _verticalVelocity -= gravity * Time.deltaTime;
    }

    private void CalculateHorizontalVelocity()
    {
        // Calculate target velocity
        Vector3 targetVelocity = GetRelativeDirection() * _targetHorizontalVelocity;
        
        // Check if player is grounded or not and apply a different force
        float airMovementFactor = _characterController.isGrounded ? 1f : airMovementModifier;
        // Check if player is accelerating or decelerating and apply a different force
        float accelerationFactor = _inputController.IsMoving ? acceleration : deceleration;
        
        // Apply horizontal acceleration/deceleration
        _horizontalVelocity = Vector3.Lerp(_horizontalVelocity, targetVelocity,accelerationFactor * airMovementFactor * Time.deltaTime);
    }
    
    private Vector3 GetRelativeDirection()
    {
        Vector3 movementDirection = _inputController.MovementDirection();
        
        // Look at direction
        Vector3 cameraForward = _freeLookCamera.LookAt.position - _freeLookCamera.transform.position;
        Vector3 cameraRight = Vector3.Cross(Vector3.up, cameraForward);
        
        // Normalize
        cameraForward.y = 0;
        cameraForward.Normalize();
        
        // Return movement direction relative to camera look at position
        return (cameraForward * movementDirection.z + cameraRight * movementDirection.x).normalized;
    }
}