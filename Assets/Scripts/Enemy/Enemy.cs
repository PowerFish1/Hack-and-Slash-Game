using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character, IDamageable
{
    [Header("Combat")]
    public Transform player;
    public Transform meleeAttackOrigin = null;
    public Transform rangedAttackOrigin = null;
    public float distanceToPlayer = 0;


    IDamageable playerAttributes;
    private Collider2D collider;

    public AudioSource hurtSound;
    public LayerMask playerLayer = 7;

    // start function
    void Start()
    {
        collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerAttributes();
        HandleMeleeAttack();
        EnemyAnimations();

    }

    // function for have more control on physics
    void FixedUpdate()
    {
        EnemyMovement();
    }

    // Function of damage control
    public virtual void ApplyDamage(float amount)
    {
        CurrentHealth -= amount;
        hurtSound.Play();
        Animator.SetTrigger("Hurt");
        if (CurrentHealth <= 0)
        {
            StartCoroutine(EnemyDie());
            // counting how many enemy did died
            GameplayController.instance.EnemyKilled();  
        }
    }

    void EnemyMovement()
    {
        // calculate the distance between player
        distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        // To follow the player where he is we need direction
        Vector2 direction = player.transform.position - transform.position;

        // Move to player
        transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, enemySpeed * Time.deltaTime);

        // Change character position to right way
        if (player.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (player.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }

    // Function that shows where is our raycast is pointing
    void OnDrawGizmosSelected()
    {
        Debug.DrawRay(transform.position, -Vector2.up * groundedLeeway, Color.green);

        if (meleeAttackOrigin != null)
        {
            Gizmos.DrawWireSphere(meleeAttackOrigin.position, meleeAttackRadius);
        }
    }

    // function that controls the animations
    void EnemyAnimations()
    {
        // New component bool for the animator
        Animator.SetBool("Grounded", CheckGrounded());

        // if we are onto ground and we are running
        if (Mathf.Abs(distanceToPlayer) > 0.1f)
        {
            Animator.SetInteger("AnimState", 2 );
        }


        // if the player is in range 
        if (distanceToPlayer <= meleeAttackRadius)
        {
            // if not attacking
            if (!isMeleeAttacking)
            {
                // start time condition for the delay and attack
                StartCoroutine(MeleeAttackAnimDelay());
            }
        }
    }
    // Function that controls our characters melee attack mechanic
    private void HandleMeleeAttack()
    {   
        // if the player is in range
        if (distanceToPlayer <= meleeAttackRadius && timeUntilMeleeReadied <= 0)
        {
            // start time loop for delayeddamage
            StartCoroutine(DelayedDamage());
            // Activating the attack delay
            timeUntilMeleeReadied = meleeAttackDelay;
        }
        else
        {
            // preparing for next ready input
            timeUntilMeleeReadied -= Time.deltaTime;
        }
    }

    // Do damage when the animation fits
    private IEnumerator DelayedDamage()
    {
        // if there is a player object in the target
        if (playerAttributes != null)
        {
            // wait for the animation fits
            yield return new WaitForSeconds(enemyApplyDamageDelay);

            // if th enemey didnt die while animation fits
            if (CurrentHealth > 0)
            {
                // apply damage
                playerAttributes.ApplyDamage(enemyMeleeDamage);
            }
        }
    }

    // enemy die time loop
    IEnumerator EnemyDie()
    {
        enemySpeed = 0;
        collider.isTrigger = true;
        Animator.SetTrigger("Death");
        yield return new WaitForSeconds(0.4f);
        Die();
    }

    // function that controls enemys attackable
    private void PlayerAttributes()
    {
        // What this does return a list of colliders within a radius
        // storing a list of colliders
        Collider2D[] overlappedColliders = Physics2D.OverlapCircleAll(meleeAttackOrigin.position, meleeAttackRadius, playerLayer);

        // iterate through our overlapped colliders
        for (int i = 0; i < overlappedColliders.Length; i++)
        {
            playerAttributes = overlappedColliders[i].GetComponent<IDamageable>();
        }
    }

    // time condition for melee attack
    private IEnumerator MeleeAttackAnimDelay()
    {
        Animator.SetTrigger("Attack");
        isMeleeAttacking = true;
        enemySpeed = 0;
        yield return new WaitForSeconds(enemyAttackDelay);
        isMeleeAttacking = false;
        enemySpeed = 1.5f;
    }
}
