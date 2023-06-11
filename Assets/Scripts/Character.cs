using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Attributes")]
    public float healthPool = 10f;

    [Header("Player Movement")]
    public float playerSpeed = 5f;

     [Header("Player Combat")]
    public float playerMeleeDamage = 10f;

   
    [Header("Enemy Movement")]
    public float enemySpeed = 2.5f;
    public float jumpForce = 6f;
    public float groundedLeeway = 0.1f;

    [Header("Enemy Combat")]
    public float enemyMeleeDamage = 2f;
    public float enemyMeleeAttackRadius = 0.6f;
    public float enemyMeleeAttackDelay = 1.1f;
    public float enemyAttackDelay = 0.7f;
    public float enemyApplyDamageDelay = 0.42f;

    [Header("General Combat")]
    public float meleeAttackRadius = 0.6f;
    public float meleeAttackDelay = 1.1f;
    public bool attemptMeleeAttack = false;
    public float timeUntilMeleeReadied = 0;
    public bool isMeleeAttacking = false;
    

    private Rigidbody2D rb2D = null;
    private Animator animator = null;
    private float currentHealth = 10f;

    // acces to characters rigidbody (getter - setter)
    public Rigidbody2D Rb2D
    {
        get { return rb2D; }
        protected set { rb2D = value; }
    }

    // value a characters current health (getter - setter)
    public float CurrentHealth
    {
        get { return currentHealth; }
        set { currentHealth = value; }
    }

    // acces to characters animator (getter - setter)
    public Animator Animator
    {
        get { return animator; }
        protected set { animator = value; }
    }

    // Start is called before the first frame update
    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        currentHealth = healthPool;
    }


    // Function that entitys die mechanic
    protected virtual void Die()
    {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }

    // Check if player is grounded
    protected bool CheckGrounded()
    {
        return Physics2D.Raycast(transform.position, -Vector2.up, groundedLeeway);
    }
}
