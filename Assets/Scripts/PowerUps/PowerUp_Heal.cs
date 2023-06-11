using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp_Heal : PowerUp
{
    [Header("Settings")]
    public float amount = 5f;
    public AudioSource pickUpSound;

    // activate method which override from PowerUp
    public override void Activate()
    {
        pickUpSound.Play();
        // if we have enough health for maxing it 
        if (Player.CurrentHealth + amount > Player.healthPool)
        {
            Player.CurrentHealth = Player.healthPool;
        }
        // if we have not enough health
        else
        {
            Player.CurrentHealth += amount;
        }

        base.Activate();
    }
}
