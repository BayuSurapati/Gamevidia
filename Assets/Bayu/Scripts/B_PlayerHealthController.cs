using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_PlayerHealthController : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 1;
    private int currentHealth;

    [Header("Death")]
    public float deathDelay = 1f;

    private Animator anim;
    private PlayerScripts playerMovement;
    private bool isDead;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerScripts>();
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        // Matikan gerakan
        playerMovement.canMove = false;

        // Stop physics
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        rb.simulated = false;

        // Animasi mati
        anim.SetTrigger("IsDead");

        Invoke(nameof(ShowRestartUI), deathDelay);
    }

    void ShowRestartUI()
    {
        B_RestartUI.Instance.Show();
    }
}
