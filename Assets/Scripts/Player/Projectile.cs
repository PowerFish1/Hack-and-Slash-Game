using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Rigidbody2D rb2D = null;
    public float speed = 15f;
    public float damage = 5f;
    public float delaySeconds = 3f;

    private WaitForSeconds cullDelay = null;

    // Start is called before the first frame update
    void Start()
    {
        // coroutine for time loop
        cullDelay = new WaitForSeconds(delaySeconds);
        StartCoroutine(DelayedCull());

        rb2D.velocity = transform.right * speed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // if the gameobject collides with something
    void OnTriggerEnter2D(Collider2D collider)
    {
        // if collides to enemy
        if (collider.gameObject.layer == 8)
        {
            // apply damage
            IDamageable enemyAttributes = collider.GetComponent<IDamageable>();
            if (enemyAttributes != null)
            {
                enemyAttributes.ApplyDamage(damage);
            }

            // destroy gameobject
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    // creating a time loop 
    private IEnumerator DelayedCull()
    {
        yield return cullDelay;
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
