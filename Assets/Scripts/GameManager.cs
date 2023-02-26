using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public Transform[] spawnPoints;
    public string[] enemyPrefabs;

    public string[] itemPrefebs;

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

    public ObjectManager objectManager;

    public List<Spawn> spawnList;
    public int spawnIndex;      //���� ���� �ε���
    public bool spawnEnd;   //��� �������� ������ �� �ݾ��ִ� flag

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
        spawnList = new List<Spawn>();
        enemyPrefabs = new string[] { "EnemyS", "EnemyM", "EnemyL", "EnemyB" };
        itemPrefebs = new string[] { "ItemBoom","ItemCoin", "ItemPower" };
        ReadSpawnFile();
    }

    void ReadSpawnFile()
    {
        //1. ���� �ʱ�ȭ
        spawnList.Clear();
        spawnIndex = 0;
        spawnEnd = false;

        //2. ������ ���� �б�.
        //<�ؽ�Ʈ ���� ���� Ŭ����>
        //as = ������ ������ �����ϴ� �Լ�. ������ ���� ���� ��� nulló�� ��.
        TextAsset textFile = Resources.Load("Stage0") as TextAsset;
        //System.IO���� ���� Ŭ���� - ���� ���� ���ڿ� ������ �б� Ŭ����. 
        StringReader stringReader = new StringReader(textFile.text);

        //���� ���� ������� �Ѵ�.
        while (stringReader != null)   
        {
            string line = stringReader.ReadLine();  //�� �پ� �б�
            

            if(line == null)
            {
                break;
            }
            //3. ������ ������ ����
            Spawn spawnData = new Spawn();
            spawnData.delay = float.Parse(line.Split(',')[0]);      //Split() - ������ ���� ���ڷ� ���ڿ��� ������ �Լ�.
            spawnData.type = line.Split(',')[1];
            spawnData.point = int.Parse(line.Split(',')[2]);

            //spawnList�� spawnDatas �ֱ�
            spawnList.Add(spawnData);
        }

        //StringReader�� ����� ������ �۾��� ���� �� �� �ݱ�
        stringReader.Close();

        //ù��° ���� ������ ����.
        nextEnemySpawnDelay = spawnList[0].delay;

    }

    // Update is called once per frame
    void Update()
    {
        if (isStop == false)
        {
            curEnemySpawnDelay += Time.deltaTime;
            if (curEnemySpawnDelay > nextEnemySpawnDelay && !spawnEnd)
            {
                SpawnEnemy();
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
        scoreText.text = string.Format("{0:n0}",playerLogic.nScore);    // {0:n0} => �� �ڸ����� ������ �����ִ� ���� ���
    }
    void SpawnItem()
    {
        int randItemType = Random.Range(0, 3);
        int randItemPoint = Random.Range(0, 3);
        GameObject dropItem = objectManager.MakeObj(itemPrefebs[randItemType]);
        dropItem.transform.position = spawnPoints[randItemPoint].position;
        dropItem.transform.rotation = Quaternion.identity;

        //Instantiate(itemPrefebs[randItemType], spawnPoints[randItemPoint].position, Quaternion.identity);
    }

    void SpawnEnemy()
    {
        int enemyindex = 0;
        switch (spawnList[spawnIndex].type)
        {
            case "S":
                enemyindex = 0;
                break;
            case "M":
                enemyindex = 1;
                break;
            case "L":
                enemyindex = 2;
                break;
            case "B":
                enemyindex = 3;
                break;
        }
        int enemyPoint = spawnList[spawnIndex].point;

        GameObject goEnemy = objectManager.MakeObj(enemyPrefabs[enemyindex]);
        goEnemy.transform.position = spawnPoints[enemyPoint].position;
        goEnemy.transform.rotation = Quaternion.identity;

        //Instantiate(enemyPrefabs[randType], spawnPoints[randPoint].position, Quaternion.identity);
        Enemy enemyLogic = goEnemy.GetComponent<Enemy>();
        enemyLogic.playerObject = player;
        enemyLogic.objectManager = objectManager;
        enemyLogic.Move(enemyPoint);

        //������ �ε��� ���� - spawnList ���� �˸�
        spawnIndex++;
        if (spawnIndex == spawnList.Count)
        {
            spawnEnd = true;
            return;
        }
        //���� ������ ������ ����
        nextEnemySpawnDelay = spawnList[spawnIndex].delay;

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
            item.SetActive(false);
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
        // Life�������� ��������ŭ �� ������ (Disable)
        for (int i = 0; i < 3 ; i++)
        {
            lifeicons[i].color = new Color(1, 1, 1, 0);
        }
        // Life�������� ��������ŭ ��� �Ҵ�. (active)
        for (int i = 0; i < life; i++)
        {
            lifeicons[i].color = new Color(1, 1, 1, 1);
        }
    }
    public void UpdateBoomIcon(int boom)
    {
        // Boom �������� ��������ŭ �� ������ (Disable)
        for (int i = 0; i < 3; i++)
        {
            boomicons[i].color = new Color(1, 1, 1, 0);
        }
        // Boom �������� ��������ŭ ��� �Ҵ�. (active)
        for (int i = 0; i < boom; i++)
        {
            boomicons[i].color = new Color(1, 1, 1, 1);
        }
    }
}
