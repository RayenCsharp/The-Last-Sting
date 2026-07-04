using UnityEngine;

public class Damageable : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;

    // backing field for current health (not exposed in the Inspector)
    private float health;
    public float Health
    {
        get => health;
        private set => health = Mathf.Max(0f, value);
    }

    public bool IsDead => Health <= 0f;
    public bool IsHit;

    [SerializeField] private Animator animator;
    [SerializeField] private bool Player;
    [SerializeField] private bool Tank;
    [SerializeField] private bool Boss;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // initialize current health
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        if (!IsDead)
        {
            health -= damage;
            Debug.Log($"{gameObject.name} took {damage} damage. Current health: {Health}");

            if (Health <= 0)
            {
                Die();
            }
            animator.SetTrigger("Hit");
        }
    }

    void Die()
    {
        // Handle death logic here (e.g., play animation, disable object, etc.)
        Debug.Log($"{gameObject.name} has died.");
        // perform Death animation
        animator.SetTrigger("Death");
        //Destroy(gameObject);
        // disable the object
    }
}
