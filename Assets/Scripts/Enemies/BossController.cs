using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("Boss Settings")]
    [SerializeField] private float moveSpeed = 6.5f;
    [SerializeField] private float stopDistance = 5f;
    [SerializeField] float rotationSpeed = 120f;
    [Header("References")]
    [SerializeField] private DetectionZone detectionZone;
    [SerializeField] private DetectionZone attackZoneBite;
    [SerializeField] private DetectionZone attackZoneHead;
    [SerializeField] private DetectionZone attackZoneTail;
    [SerializeField] private Damageable damageable;
    [SerializeField] private Animator animator;
    [SerializeField] private UiManeger uiManager;

    enum BossState { Idle, Chase, Attack }
    enum AttackType { None, Bite, Head, Tail }
    [Header("State")]
    [SerializeField] private BossState bossState;
    [SerializeField] private AttackType attackType;
    [SerializeField] private bool canMove;
    public bool IsAttacking;

    [Header("Attack Cooldowns")]
    [SerializeField] private float biteCooldown = 3f;
    [SerializeField] private float headCooldown = 2f;
    [SerializeField] private float tailCooldown = 5f;

    [SerializeField] private bool canUseBite = true;
    [SerializeField] private bool canUseHead = true;
    [SerializeField] private bool canUseTail = true;


    private Transform target;
    private Rigidbody _rb;



    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        damageable = GetComponent<Damageable>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bossState = BossState.Idle;
        attackType = AttackType.None;
    }

    // Update is called once per frame
    void Update()
    {
        if (bossState == BossState.Chase)
        {
            uiManager.BossHp(damageable.Health, 300f);
        }
        if (damageable.IsDead || IsAttacking)
        {
            canMove = false;
            return;
        }else
        {
            canMove = true;
        }
        Attack();
        animator.SetBool("Chase", bossState == BossState.Chase);
    }

    void FixedUpdate()
    {
        if (!canMove)
        {
            return;
        }
        Chase();
    }


    void Chase() 
    {
        if (bossState == BossState.Attack)
            return;

        if (detectionZone.detectedColliders.Count > 0)
        {
            bossState = BossState.Chase;
            if (detectionZone.detectedColliders[0] == null)
            {
                bossState = BossState.Idle;
                return;
            }
            target = detectionZone.detectedColliders[0].transform;
            
            Vector3 targetDirection = target.position - transform.position;
            targetDirection.y = 0;

            _rb.MovePosition(transform.position + targetDirection.normalized * moveSpeed * Time.deltaTime);
            
            float distance = Vector3.Distance(transform.position, target.position);

            if (distance > stopDistance)
            {
                _rb.MovePosition(transform.position + targetDirection.normalized * moveSpeed * Time.deltaTime);

                Quaternion lookDirection = Quaternion.LookRotation(targetDirection);

                Quaternion newRotation = Quaternion.RotateTowards(
                    transform.rotation,
                    lookDirection,
                    rotationSpeed * Time.deltaTime
                );

                float angle = Vector3.Angle(transform.forward, targetDirection);

                if (angle > 10f)
                {
                    _rb.MoveRotation(newRotation);
                }
            }
        }
        else
        {
            bossState = BossState.Idle;
        }
    }

    void Attack()
    {
        if (attackZoneBite.detectedColliders.Count > 0 && canUseBite)
        {

            bossState = BossState.Attack;
            if (!IsAttacking)
            {
                attackType = AttackType.Bite;
                animator.SetTrigger("Bite");
                canUseBite = false;
                StartCoroutine(AttackCooldown(biteCooldown, AttackType.Bite));
            }
        }
        else if (attackZoneHead.detectedColliders.Count > 0 && canUseHead)
        {
            bossState = BossState.Attack;
            if (!IsAttacking)
            {
                attackType = AttackType.Head;
                animator.SetTrigger("Head");
                canUseHead = false;
                StartCoroutine(AttackCooldown(headCooldown, AttackType.Head));
            }
        }
        else if (attackZoneTail.detectedColliders.Count > 0 && canUseTail)
        {
            bossState = BossState.Attack;
            if (!IsAttacking)
            {
                attackType = AttackType.Tail;
                animator.SetTrigger("Tail");
                canUseTail = false;
                StartCoroutine(AttackCooldown(tailCooldown, AttackType.Tail));
            }
        }
        else if (detectionZone.detectedColliders.Count > 0 && !IsAttacking)
        {
            bossState = BossState.Chase;
            attackType = AttackType.None;
        }
        else
        {
            bossState = BossState.Idle;
            attackType= AttackType.None;
        }
    }

    IEnumerator AttackCooldown(float cooldown, AttackType type)
    {
        yield return new WaitForSeconds(cooldown);
        switch (type)
        {
            case AttackType.Bite:
                canUseBite = true;
                break;
            case AttackType.Head:
                canUseHead = true;
                break;
            case AttackType.Tail:
                canUseTail = true;
                break;
        }
    }
}



