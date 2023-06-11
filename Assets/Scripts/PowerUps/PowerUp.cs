using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private Player player = null;

    public Player Player
    {
        get { return player; }
    }

    // if there is a collision
    void OnTriggerEnter2D(Collider2D collision)
    {
        // if the collision to player
        if (collision.CompareTag("Player"))
        {
            if (collision.GetComponent<Player>())
            {
                player = collision.GetComponent<Player>();
            }

            Activate();

        }
    }

    // function that activates destroy
    public virtual void Activate()
    {
        Destroy(gameObject);
    }
}
