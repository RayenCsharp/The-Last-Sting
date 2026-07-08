using UnityEngine;

public class EnemyController : MonoBehaviour
{
    enum EnemyState { Idle, Return, Chase, Attack }

    [Header("Enemy Settings")]
    [SerializeField] private EnemyState enemyState;
    [SerializeField] private float enemySpeed = 7f;
    [SerializeField] private bool Tank;

    [Header("Refrences")]
    [SerializeField] private DetectionZone detectionZone;
    [SerializeField] private DetectionZone attackZone;
    [SerializeField] private Damageable damageable;
    [SerializeField] private Animator animator;
    private Rigidbody _rb;
    private Transform target;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    public bool IsAttacking;

    [SerializeField] private bool canMove = true;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        damageable = GetComponent<Damageable>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyState = EnemyState.Idle;
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (damageable.IsDead || damageable.IsHit)
        {
            canMove = false;
            return;
        }
        else if (!damageable.IsHit)
        {
            canMove = true;
        }
        AttackMode();
        animator.SetBool("Chase", enemyState == EnemyState.Chase || enemyState == EnemyState.Return);
    }

    void FixedUpdate()
    {
        if (!canMove)
        {
            return;
        }
        ChaseMode();
        ReturnMode();
    }

    void ChaseMode()
    {
        if (enemyState == EnemyState.Attack)
            return;

        if (detectionZone.detectedColliders.Count > 0)
        {
            enemyState = EnemyState.Chase;
            if (detectionZone.detectedColliders[0] == null)
            {
                enemyState = EnemyState.Return;
                return;
            }
            target = detectionZone.detectedColliders[0].transform;
            Vector3 targetDirection = target.position - transform.position;
            targetDirection.y = 0;
            
            _rb.MovePosition(transform.position + targetDirection.normalized * enemySpeed * Time.deltaTime);
            if (targetDirection.sqrMagnitude > 0.001f)
            {
                Quaternion lookDirection = Quaternion.LookRotation(targetDirection);
                _rb.MoveRotation(Quaternion.Slerp(transform.rotation, lookDirection, Time.deltaTime * 5f));
            }
            

        }
        else
        {
            enemyState = EnemyState.Idle;
        }
    }

    void AttackMode() 
    {
        if (attackZone.detectedColliders.Count > 0 ) {
            enemyState = EnemyState.Attack;
            if (!IsAttacking)
            {
                animator.SetTrigger("Attack");
                Debug.Log("Enemy Attacking");
            }
        }
        else if (attackZone.detectedColliders.Count == 0 && !IsAttacking)
        {
            if (detectionZone.detectedColliders.Count > 0)
            {
                enemyState = EnemyState.Chase;
            }
            else
            {
                enemyState = EnemyState.Idle;
            }
        }
    }

    void ReturnMode()
    {
        if (enemyState == EnemyState.Attack || enemyState == EnemyState.Chase)
            return;
        if (Vector3.Distance(transform.position, initialPosition) > 0.1f)
        {
            enemyState = EnemyState.Return;
            Vector3 returnDirection = (initialPosition - transform.position).normalized;
            returnDirection.y = 0;
            _rb.MovePosition(transform.position + returnDirection * enemySpeed * Time.deltaTime);
            Quaternion lookDirection = Quaternion.LookRotation(returnDirection);
            lookDirection.x = 0;
            lookDirection.z = 0;
            _rb.MoveRotation(Quaternion.Slerp(transform.rotation, lookDirection, Time.deltaTime * 5f));
            if (Quaternion.Angle(transform.rotation, initialRotation) > 0.1f && Vector3.Distance(transform.position, initialPosition) <= 1f)
            {
                _rb.MoveRotation(Quaternion.Slerp(transform.rotation, initialRotation, Time.deltaTime * 5f));
            }
        }
        else
        {
            enemyState = EnemyState.Idle;
        }
    }
        

}
