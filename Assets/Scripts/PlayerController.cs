using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _rollSpeed;
    [SerializeField] private float _rollDuration;
    private float _rollTime;
    private bool _isRolling;
    private bool _isInvincible = false; 

    [Header("Cooldown Settings")]
    [SerializeField] private float _jumpCooldown;
    [SerializeField] private float _rollCooldown;
    private float _jumpCooldownTimer = 0f;
    private float _rollCooldownTimer = 0f;

    [Header("Ground Checker")]
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _groundCheckRadius;
    [SerializeField] private LayerMask _groundLayer;
    private bool _isGrounded;

    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private PauseMenu _pauseMenu;

    public bool IsInvincible => _isInvincible;

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (_isRolling)
        {
            HandleRoll();
            return;
        }

        _jumpCooldownTimer -= Time.deltaTime;
        _rollCooldownTimer -= Time.deltaTime;

        Move();
        Jump();
        Roll();
        UpdateAnimator();
        UpdateFacingDirection();
    }

    private void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        Vector2 velocity = new Vector2(horizontal * _moveSpeed, _rigidbody2D.velocity.y);
        _rigidbody2D.velocity = velocity;

        if (Mathf.Abs(horizontal) > 0.1f)
            transform.localScale = new Vector3(Mathf.Sign(horizontal), 1, 1);
    }

    private void Jump()
    {
        _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _groundLayer);

        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded && _jumpCooldownTimer <= 0f)
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _jumpForce);
            _jumpCooldownTimer = _jumpCooldown;
        }
    }

    private void Roll()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && _isGrounded && _rollCooldownTimer <= 0f && _rigidbody2D.velocity.x != 0)
        {
            _isRolling = true;
            _isInvincible = true;
            _animator.SetBool("IsRolling", _isRolling);

            _rollTime = _rollDuration;
            _rollCooldownTimer = _rollCooldown;

            float direction = transform.localScale.x;
            _rigidbody2D.velocity = new Vector2(direction * _rollSpeed, _rigidbody2D.velocity.y);
        }
    }

    private void HandleRoll()
    {
        _rollTime -= Time.deltaTime;
        if (_rollTime <= 0f)
        {
            _isRolling = false;
            _isInvincible = false;
            _animator.SetBool("IsRolling", _isRolling);

            _rigidbody2D.velocity = new Vector2(0, _rigidbody2D.velocity.y);
        }
    }

    private void UpdateAnimator()
    {
        float horizontalSpeed = Mathf.Abs(_rigidbody2D.velocity.x);
        _animator.SetFloat("Speed", horizontalSpeed);
        _animator.SetBool("IsGrounded", _isGrounded);
        _animator.SetFloat("VerticalSpeed", _rigidbody2D.velocity.y);
    }

    private void UpdateFacingDirection()
    {
        if (Mathf.Abs(_rigidbody2D.velocity.x) < 0.1f)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (mousePosition.x > transform.position.x)
                transform.localScale = new Vector3(1, 1, 1);
            else if (mousePosition.x < transform.position.x)
                transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void OnDrawGizmos()
    {
        if (_groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);
        }
    }
}