using System.Collections;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private ParticleSystem poisonParticles;
    [SerializeField] private GameManeger gameManeger;
    [SerializeField] private UiManeger uiManeger;

    enum holders { Player, Ant, LadyBug, Rhinoceros, Boss }
    [SerializeField] private holders holder;
    [SerializeField] private int scoreValue;

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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // initialize current health
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (holder == holders.Player)
        {
            uiManeger.UpdateHealth(Health, maxHealth);
        }
    }

    public void TakeDamage(float damage)
    {
        if (!IsDead)
        {
            Health -= damage;
            Debug.Log($"{gameObject.name} took {damage} damage. Current health: {Health}");

            if (Health <= 0)
            {
                Die();
            }
            if (holder != holders.Boss)
            {
                animator.SetTrigger("Hit");
            }
        }
    }

    public void PoisenDamage(float damage, float duration)
    {
        if (!IsDead)
        {
            StartCoroutine(CoroutinePoisenDamage(damage, duration));
        }
        else
        {
            StopCoroutine(CoroutinePoisenDamage(damage, duration));
        }
    }

    IEnumerator CoroutinePoisenDamage(float damage, float duration)
    {
        poisonParticles.Play();
        float elapsedTime = 0f;
        while (elapsedTime < duration && !IsDead)
        {
            TakeDamage(damage);
            yield return new WaitForSeconds(1f);
            elapsedTime += 1f;
        }
        poisonParticles.Stop();
    }

    public void Heal(float amount)
    {
        if (!IsDead)
        {
            Health += amount;
            Health = Mathf.Min(Health, maxHealth);
            Debug.Log($"{gameObject.name} healed {amount}. Current health: {Health}");
        }
    }

    void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        // perform Death animation
        animator.SetTrigger("Death");
        if (holder == holders.Player || holder == holders.Boss)
        {
            gameManeger.GameOver();
        }
        else
        {
            gameManeger.AddScore(scoreValue);
        }
    }
    
}
