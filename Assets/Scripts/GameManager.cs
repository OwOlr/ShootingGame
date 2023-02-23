using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public Transform[] spawnPoints;
    public GameObject[] enemyPrefabs;

    public GameObject[] itemPrefebs;

    public float curEnemySpawnDelay;
    public float nextEnemySpawnDelay;

    public float curItemSpawnDelay;
    public float nextItemSpanDelay;

    public GameObject player;

    public Image lifeicon;
    public Image gameOver;

    public int lifeCount;

    public bool isStop = false;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
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
        if (isStop == false)
        {
            curEnemySpawnDelay += Time.deltaTime;
            if (curEnemySpawnDelay > nextEnemySpawnDelay)
            {
                SpawnEnemy();

                nextEnemySpawnDelay = Random.Range(1.0f, 3.0f);
                curEnemySpawnDelay = 0;
            }
        
            curItemSpawnDelay += Time.deltaTime;
            if (curItemSpawnDelay > nextItemSpanDelay)
            {
                SpawnItem();
                nextItemSpanDelay = Random.Range(5.0f, 10.0f);
                curItemSpawnDelay = 0;
            }
        }
    }
    void SpawnItem()
    {
        int randItemType = Random.Range(0, 3);
        int randItemPoint = Random.Range(0, 3);
        GameObject dropItem = Instantiate(itemPrefebs[randItemType], spawnPoints[randItemPoint].position, Quaternion.identity);
    }

    void SpawnEnemy()
    {
        int randType = Random.Range(0, 3);
        int randPoint = Random.Range(0, 7);

        GameObject goEnemy = Instantiate(enemyPrefabs[randType], spawnPoints[randPoint].position, Quaternion.identity);
        Enemy enemyLogic = goEnemy.GetComponent<Enemy>();
        enemyLogic.playerObject = player;
        enemyLogic.Move(randPoint);

    }

    public void GameOver()
    {
            GameObject[] stopEnemies = GameObject.FindGameObjectsWithTag("Enemy");
            GameObject[] stopBullet = GameObject.FindGameObjectsWithTag("EnemyBullet");
            GameObject[] stopItem = GameObject.FindGameObjectsWithTag("Item");

            foreach (var enemies in stopEnemies)
            {
                enemies.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
                enemies.gameObject.GetComponent<Enemy>().isStop = true;
            }
            foreach (var bullets in stopBullet)
            {
                bullets.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            }
            foreach (var items in stopItem)
            {
                items.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            }


            gameOver.gameObject.SetActive(true);
            player.gameObject.GetComponent<PlayerController>().isStop = true;
            isStop = true;
        
    
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
        lifeicon.enabled = false;
        player.transform.position = Vector3.down * 4.5f;
        player.gameObject.GetComponent<PlayerController>().isHit = false;
        player.gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }
}
