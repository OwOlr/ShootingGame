using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager sgameM = null;

    public Transform[] spawnPoints;
    public GameObject[] enemyPrefabs;

    public float curEnemySpawnDelay;
    public float nextEnemySpawnDelay;

    public GameObject player;


    private void Awake()
    {
        if (sgameM == null)
        {
            sgameM = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        curEnemySpawnDelay += Time.deltaTime;
        if (curEnemySpawnDelay > nextEnemySpawnDelay)
        {
            SpawnEnemy();

            nextEnemySpawnDelay = Random.Range(0.5f, 3.0f);
            curEnemySpawnDelay = 0;
        }
    }

    void SpawnEnemy()
    {
        int randType = Random.Range(0, 6);
        int randPoint = Random.Range(0, 6);

        GameObject goEnemy = Instantiate(enemyPrefabs[randType], spawnPoints[randPoint].position, Quaternion.identity);
        Enemy enemyLogic = goEnemy.GetComponent<Enemy>();
        enemyLogic.playerObject = player;
        enemyLogic.Move(randPoint);
    }

    public void GameOver()
    {
        
    }

    public void ResPawnPlayer()
    {
        Invoke("AlivePlayer", 1.0f);
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("EnemyBullet");
        foreach (var item in bullets)
        {
            Destroy(item);
        }
    }
    void AlivePlayer()
    {
        player.transform.position = Vector3.down * 4.5f;
        player.gameObject.GetComponent<PlayerController>().isHit = false;
        player.gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }
}
