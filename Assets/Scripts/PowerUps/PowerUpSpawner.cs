using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{

    [SerializeField] private float spawnRate = 13f;
    [SerializeField] private GameObject powerUpPrefab;
    [SerializeField] private bool canSpawn = true;

    // start function
    private void Start()
    {
        StartCoroutine(Spawner());
    }

    // spawner time loop
    private IEnumerator Spawner()
    {
        WaitForSeconds wait = new WaitForSeconds(spawnRate);

        while (canSpawn)
        {
            yield return wait;
            GameObject powerUpToSpawn = powerUpPrefab;
            Instantiate(powerUpToSpawn, transform.position, Quaternion.identity);
        }
    }
}
