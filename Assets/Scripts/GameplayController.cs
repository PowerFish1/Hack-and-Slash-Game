using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayController : MonoBehaviour
{
    public static GameplayController instance;

    [SerializeField]
    private Text enemyKillCountText;

    private int enemyKillCount = 0;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


    public void EnemyKilled()
    {
        enemyKillCount++;
        enemyKillCountText.text = "Total Kills: " + enemyKillCount;
    }
}
