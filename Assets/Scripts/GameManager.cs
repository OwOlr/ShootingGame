using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    public Image[] lifeicons;
    public Image[] boomicons;
    public Image gameOver;
    public Text scoreText;

    public int lifeCount;

    public bool isStop = false;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
           //DontDestroyOnLoad(gameObject);
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

                nextEnemySpawnDelay = Random.Range(1.0f, 2.0f);
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

        PlayerController playerLogic = player.GetComponent<PlayerController>();
        scoreText.text = string.Format("{0:n0}",playerLogic.nScore);    // {0:n0} => 세 자리마다 쉬묘로 나눠주는 숫자 양식
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
        player.transform.position = Vector3.down * 4.5f;
        player.gameObject.GetComponent<PlayerController>().isHit = false;
        player.gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }

    public void RetryGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void UpdateLifeIcon(int life)
    {
        // Life아이콘을 라이프만큼 다 꺼버림 (Disable)
        for (int i = 0; i < 3 ; i++)
        {
            lifeicons[i].color = new Color(1, 1, 1, 0);
        }
        // Life아이콘을 라이프만큼 모두 켠다. (active)
        for (int i = 0; i < life; i++)
        {
            lifeicons[i].color = new Color(1, 1, 1, 1);
        }
    }
    public void UpdateBoomIcon(int boom)
    {
        // Boom 아이콘을 라이프만큼 다 꺼버림 (Disable)
        for (int i = 0; i < 3; i++)
        {
            boomicons[i].color = new Color(1, 1, 1, 0);
        }
        // Boom 아이콘을 라이프만큼 모두 켠다. (active)
        for (int i = 0; i < boom; i++)
        {
            boomicons[i].color = new Color(1, 1, 1, 1);
        }
    }
}
