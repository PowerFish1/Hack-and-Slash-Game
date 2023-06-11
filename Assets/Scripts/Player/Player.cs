using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : Character, IDamageable
{
    [Header("Input")]
    public KeyCode meleeAttackKey = KeyCode.Mouse0;
    public KeyCode rangedAttackKey = KeyCode.Mouse1;
    public KeyCode jumpKey = KeyCode.Space;
    public string xMoveAxis = "Horizontal";


    [Header("Combat")]
    public Transform meleeAttackOrigin = null;
    public Transform rangedAttackOrigin = null;
    public GameObject projectile = null;
    public float rangedAttackDelay = 0.3f;
    public LayerMask enemyLayer = 8;
    public float playerHealth;
    
    [Header("SFX")]
    public AudioSource jumpSound;
    public AudioSource attackSound;
    public AudioSource rangeShootSound;
    public AudioSource deathSound;
    public AudioSource hurtSound;

    [Header("Game Objects")]
    public GameObject dieText;
    public GameObject fadeOut;


    [SerializeField]
    private Text playerHealthText;
    private float moveIntentionX = 0;
    private bool attemptJump = false;
    private bool attemptRangedAttack = false;
    private float timeUntilRangedReadied = 0;
    private bool isPlayerAlive = true;
    


    // Update is called once per frame
    void Update()
    {
        if (isPlayerAlive)
        {
            GetInput();

            HandleMeleeAttack();
            HandleRangedAttack();
            HandleJump();
            HandleRun();
            HandleAnimations();
        }

       ShowHP();
        
    }

    // function for have more control on physics
    void FixedUpdate()
    {
        HandleRun();   
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

    // Function that checks our inputs
    private void GetInput()
    {
        moveIntentionX = Input.GetAxisRaw(xMoveAxis);
        attemptMeleeAttack = Input.GetKeyDown(meleeAttackKey);
        attemptRangedAttack = Input.GetKeyDown(rangedAttackKey);
        attemptJump = Input.GetKeyDown(jumpKey);
    }

    // Function that let us to move in x axis and turns our character onto right way
    private void HandleRun()
    {
        // turn right
        if (moveIntentionX > 0 && transform.rotation.y == 0)
        {
            transform.rotation = Quaternion.Euler(0, 180f, 0);
        }

        // turn left
        else if (moveIntentionX < 0 && transform.rotation.y != 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        if (moveIntentionX != 0 || !CheckGrounded())
        {
            Rb2D.velocity = new Vector2(moveIntentionX * playerSpeed, Rb2D.velocity.y);
        }
        else if (CheckGrounded())
        {
            Rb2D.velocity = new Vector2(0, Rb2D.velocity.y);
        }
        
    }

    // Function that let us to jump 
    private void HandleJump()
    {
        // if trying to jump
        if (attemptJump && CheckGrounded())
        {
            // add force to jump
            Rb2D.velocity = new Vector2(Rb2D.velocity.x, jumpForce);
            jumpSound.Play();
        }
    }

    // Function that controls our characters melee attack mechanic
    private void HandleMeleeAttack()
    {
        if (attemptMeleeAttack && timeUntilMeleeReadied <= 0)
        {
            Debug.Log("Player: Attempting to Melee Attack");

            // What this does return a list of colliders within a radius
            // storing a list of colliders
            Collider2D[] overlappedColliders = Physics2D.OverlapCircleAll(meleeAttackOrigin.position, meleeAttackRadius, enemyLayer);

            // iterate through our overlapped colliders
            for (int i = 0; i < overlappedColliders.Length; i ++) { 
                IDamageable enemyAttributes = overlappedColliders[i].GetComponent<IDamageable>();
                if (enemyAttributes != null)
                {
                    enemyAttributes.ApplyDamage(playerMeleeDamage);
                }
            }
            // Activating the attack delay
            timeUntilMeleeReadied = meleeAttackDelay;
        }
        else
        {
            // preparing for next ready input
            timeUntilMeleeReadied -= Time.deltaTime;
        }
    }

    // Function that controls our characters range attack mechanic
    private void HandleRangedAttack()
    {
        // if trying to range attack
        if (attemptRangedAttack && timeUntilRangedReadied <= 0) 
        {
            Debug.Log("Player: Attempting to Ranged Attack");
            // shoots attack
            Instantiate(projectile, rangedAttackOrigin.position, rangedAttackOrigin.rotation);

         
            // Activating the attack delay
            timeUntilRangedReadied = rangedAttackDelay;
        }
        else
        {
            // Preparing for next ready input
            timeUntilRangedReadied -= Time.deltaTime;
        }
    }

    private void HandleAnimations()
    {
        // New component bool for the animator
        Animator.SetBool("Grounded", CheckGrounded());

        // if trying to attack
        if (attemptMeleeAttack)
        {
            // if not attacking
            if (!isMeleeAttacking)
            {
                // start time condition for the delay
                StartCoroutine(MeleeAttackAnimDelay());
            }
        }

        // if trying to jump and grounded 
        if (attemptJump && CheckGrounded() || Rb2D.velocity.y > 1f)
        {   
            // if not attacking
            if (!isMeleeAttacking)
            {
                // change animation state to jump
                Animator.SetTrigger("Jump");
                
            }
        }

        // if we are falling or something
        if (Rb2D.velocity.y != 0 && !CheckGrounded())
        {
            // change animation state to jump
            Animator.SetTrigger("Jump");
        }

        // if we are onto ground and we are running
        if (Mathf.Abs(moveIntentionX) > 0.1f && CheckGrounded())
        {
            // change animation state to run
            Animator.SetInteger("AnimState", 2);
        }
        else
        {
            // change animation state to idle
            Animator.SetInteger("AnimState", 0 );
        }

        // if trying to shoot
        if (attemptRangedAttack)
        {
            // change animation state to shoot
            Animator.SetTrigger("Shoot");
            // play sfx
            rangeShootSound.Play();

        }
    }


    // time condition for melee attack
    private IEnumerator MeleeAttackAnimDelay()
    {
        Animator.SetTrigger("Attack");
        attackSound.Play();
        isMeleeAttacking = true;
        yield return new WaitForSeconds(meleeAttackDelay);
        isMeleeAttacking = false;
    }

    // prints hp on the screen
    private void ShowHP()
    {
        playerHealth = CurrentHealth;
        playerHealthText.text = "HP: " + playerHealth;
    }

    // Function of damage control
    public virtual void ApplyDamage(float amount)
    {
        CurrentHealth -= amount;
        if (CurrentHealth > 0)
        {
            Animator.SetTrigger("Hurt");
            hurtSound.Play();
        }
        
        if (CurrentHealth == 0 || CurrentHealth == -1)
        {
            StartCoroutine(PlayerDeath());
        }
    }

    // time loop for player dead
    private IEnumerator PlayerDeath()
    {
        isPlayerAlive = false;
        dieText.SetActive(true);
        Rb2D.bodyType = RigidbodyType2D.Static;
        deathSound.Play();
        Animator.SetTrigger("Death");
        yield return new WaitForSeconds(2.5f);
        fadeOut.SetActive(true);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(1);
    }
}
