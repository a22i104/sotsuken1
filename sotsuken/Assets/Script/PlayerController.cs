using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [Header("Ground Check")]
    [Header("Key")]
    public KeyCode _jumpKey = KeyCode.Space;
    public KeyCode _DashKey = KeyCode.LeftShift;
    public KeyCode _abilityKey = KeyCode.E;
    public Coroutine SpeedBoostCoroutine;
    public Coroutine JumpBoostCoroutine;
    private float _moveSpeed;
    public float _walkSpeed;
    public float _DashSpeed;
    public float _playerHeight;
    public float _jump;
    public float _jumpCooldown;
    public float _Sdct;
    public float _Sjct;
    public float _count;

    public float _airMultiplier;
    public float _groundDrag;

    public LayerMask whatIsGround;
    public LayerMask whatIsWall;
    public Transform _orientation;

    float _horizontalInput;
    float _verticalInput;
    public bool isTakeing = false;
   

    private bool isJumpReady;
    private bool isGrounded;
    Vector3 moveDirection;
    Rigidbody rb;
    

    public MovementState state;
    public MovementState item_state;

    public enum MovementState
    {
        def,
        Walking,
        Dashing,
        Brinking,
        SuperJumping,
        SuperDashing,
        Interrupting,
        JumpingNow
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        isJumpReady = true;
       
        _Sdct = 1.0f;
        _Sjct = 1.0f;

    }

    void FixedUpdate()
    {
        Move();
    }

    void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, _playerHeight * 0.5f + 0.2f, whatIsGround);

        Debug.DrawRay(transform.position, Vector3.down * (_playerHeight * 0.5f + 0.2f), Color.red);

        rb.drag = isGrounded ? _groundDrag : 0;

        HandleInput();
        LimitSpeed();
        StateHandler();
       
    }

    void HandleInput()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(_jumpKey) && isJumpReady && isGrounded)
        {
            isJumpReady = false;
            Jump();
            Invoke(nameof(ResetJump), _jumpCooldown);
        }
    }

    void StateHandler()
    {
        if (isGrounded && Input.GetKey(_DashKey))
        {
            state = MovementState.Dashing;
            _moveSpeed = _DashSpeed;
        }
        else if (isGrounded)
        {
            state = MovementState.Walking;
            _moveSpeed = _walkSpeed;
        }
        else
        {
            state = MovementState.JumpingNow;
        }
    }

   

    void Move()
    {
        moveDirection = _orientation.forward * _verticalInput + _orientation.right * _horizontalInput;

        if (isGrounded)
        {
            rb.AddForce(moveDirection.normalized * _moveSpeed * 10f, ForceMode.Force);
        }
        else
        {
            rb.AddForce(moveDirection.normalized * _moveSpeed * 10f * _airMultiplier, ForceMode.Force);
        }
    }

    void LimitSpeed()
    {
        Vector3 flat = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if (flat.magnitude > _moveSpeed)
        {
            Vector3 limit = flat.normalized * _moveSpeed;
            rb.velocity = new Vector3(limit.x, rb.velocity.y, limit.z);
        }
    }

    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * _jump, ForceMode.Impulse);
    }

    void ResetJump()
    {
        isJumpReady = true;
    }

    


    
}