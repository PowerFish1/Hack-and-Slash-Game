using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
   
    [SerializeField] private float spawnRate = 0.65f;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private bool canSpawn = true;
    
    // Start function
    private void Start()
    {
        StartCoroutine(Spawner());
    }


    // spawner time loop
    private IEnumerator Spawner()
    {
        // variyng spawnRate
        WaitForSeconds wait = new WaitForSeconds(spawnRate);
        // while spawnable
        while (canSpawn)
        {
            yield return wait;
            GameObject enemyToSpawn = enemyPrefab;
            Instantiate(enemyToSpawn, transform.position, Quaternion.identity);
        }
    }
}
