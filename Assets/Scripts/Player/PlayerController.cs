using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    [Header("Refrences")]
    [SerializeField] private InputHandler playerInput;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Collider Feet;
    [SerializeField] private Animator animator;
    [SerializeField] private Damageable Damageable;
    private Rigidbody _rb;

    [Header("Player Sittings")]
    
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float sprintSpeed = 10f;
    [SerializeField] private float jumpForce = 5f;
    private float _currentSpeed => playerInput.SprintTriggered ? sprintSpeed : walkSpeed;

    [SerializeField] private float maxPoisen = 100f; // maximum poisen value (exposed in the Inspector)
    private float poisen;
    public float Poisen // property for current poisen, with a getter and a private setter
    {
        get => poisen;
        private set => poisen = Mathf.Max(0f, value);
    }


    [SerializeField]private bool isGrounded;
    [SerializeField]public bool isAttacking;

    private Vector3 _moveVelocity;
    private float turnSmooth;
    [SerializeField] private float animationSpeed;

    [Header("Attack Settings")]
    [SerializeField] private float attackCooldown = 1.5f;
    enum AttackState { None, Attack1, Attack2, StingAttack }

    private AttackState attackState;
    private float attackTimer;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        Damageable = GetComponent<Damageable>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        animationSpeed = playerInput.MovementInput.magnitude * _currentSpeed;
        animator.SetFloat("Speed", animationSpeed);
        animator.SetBool("Grounded", isGrounded);
        animator.SetFloat("Velocity", _rb.linearVelocity.y);
        animator.SetBool("IsDead", Damageable.IsDead);

        CheckGrounded();
        HandleAttackTimer();
        if (isGrounded && playerInput.stingAttackTriggered && Poisen > 0f) 
        {
            Debug.Log("Sting Attack Triggered");
            playerInput.ConsumeStingAttack();
            StingAttack();
        }
        else if (isGrounded && playerInput.stingAttackTriggered && Poisen <= 0f)
        {
            Debug.Log("Not enough poisen for sting attack");
            playerInput.ConsumeStingAttack();
        }
        else
        {
            playerInput.ConsumeStingAttack();
        }
        if (isGrounded && playerInput.attackTriggered)
        {
            playerInput.ConsumeAttack();
            Attack();
        }
        else
        {
            playerInput.ConsumeAttack();
        }


    }


    void FixedUpdate()
    {
        MovePlayer();
        Jump();
    }

    void MovePlayer()
    {
        if (isAttacking || Damageable.IsDead || !isGrounded) {
            return;
        }
        Vector2 moveInput = playerInput.MovementInput;

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 direction = forward * moveInput.y + right * moveInput.x;
        
        if (moveInput != Vector2.zero)
        {
            direction.Normalize();
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmooth, 0.1f);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            _moveVelocity = direction * _currentSpeed;
            _rb.linearVelocity = _moveVelocity + new Vector3(0f, _rb.linearVelocity.y, 0f);
        }
        else
        {
            _rb.linearVelocity = new Vector3(0f, _rb.linearVelocity.y, 0f);
        }
    }

    void Jump()
    {
        if (playerInput.JumpTriggered && isGrounded)
        {
            animator.SetTrigger("Jump");
            StartCoroutine(OnJumpAnimationEventDelay());
        }
    }

    IEnumerator OnJumpAnimationEventDelay()
    {
        yield return new WaitForSeconds(0.1f);
        _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        if (_rb.linearVelocity.y < 0f)
        {
            _rb.linearVelocity += Vector3.up * Physics.gravity.y * 4 * Time.fixedDeltaTime;
        }
    }

    void Attack()
    {
        if (attackState == AttackState.None)
        {
            Debug.Log("Attack 1");
            animator.SetTrigger("NormalAttack");
            attackState = AttackState.Attack1;
            attackTimer = 0f;
        }
        else if (attackState == AttackState.Attack1)
        {
            Debug.Log("Attack 2");
            animator.SetTrigger("SpinAttack");
            attackState = AttackState.Attack2;
            attackTimer = 0f;
        }
        
    }

    void StingAttack()
    {
        if (attackState == AttackState.None)
        {
            Debug.Log("Sting Attack");
            animator.SetTrigger("StingAttack");
            attackState = AttackState.StingAttack;
            attackTimer = 0f;
            poisen -= 10f; // reduce poisen by 10 for each sting attack
        }
    }

    void HandleAttackTimer()
    {
        if (attackState != AttackState.None)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackCooldown)
            {
                attackState = AttackState.None;
                attackTimer = 0f;
            }
        }
    }

    void CheckGrounded()
    {
        Vector3 origin = new Vector3(Feet.bounds.center.x,Feet.bounds.min.y + 0.1f,Feet.bounds.center.z);
        isGrounded = Physics.BoxCast(origin,Feet.bounds.extents,Vector3.down,Quaternion.identity,0.2f,LayerMask.GetMask("Ground"));
        if (isGrounded)
        {
            _rb.linearVelocity = Vector3.zero;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PoisenPickUp"))
        {
            poisen += 50f;
            poisen = Mathf.Min(poisen, maxPoisen); // clamp poisen to maximum value
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("HpPickUp"))
        {
            Damageable.Heal(20f);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("LegendaryPickUp"))
        {
            Damageable.Heal(100f);
            Destroy(other.gameObject);
        }
    }
}
