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
    public int spawnIndex;      //다음 적의 인덱스
    public bool spawnEnd;   //모든 리스폰이 끝났을 때 닫아주는 flag

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
        //1. 변수 초기화
        spawnList.Clear();
        spawnIndex = 0;
        spawnEnd = false;

        //2. 리스폰 파일 읽기.
        //<텍스트 파일 에셋 클래스>
        //as = 파일의 유형을 검증하는 함수. 유형이 맞지 않을 경우 null처리 들어감.
        TextAsset textFile = Resources.Load("Stage0") as TextAsset;
        //System.IO에서 나온 클래스 - 파일 내의 문자열 데이터 읽기 클래스. 
        StringReader stringReader = new StringReader(textFile.text);

        //파일 끝을 정해줘야 한다.
        while (stringReader != null)   
        {
            string line = stringReader.ReadLine();  //한 줄씩 읽기
            

            if(line == null)
            {
                break;
            }
            //3. 리스폰 데이터 생성
            Spawn spawnData = new Spawn();
            spawnData.delay = float.Parse(line.Split(',')[0]);      //Split() - 지정한 구분 문자로 문자열을 나누는 함수.
            spawnData.type = line.Split(',')[1];
            spawnData.point = int.Parse(line.Split(',')[2]);

            //spawnList에 spawnDatas 넣기
            spawnList.Add(spawnData);
        }

        //StringReader로 열어둔 파일은 작업이 끝난 후 꼭 닫기
        stringReader.Close();

        //첫번째 스폰 딜레이 적용.
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
        scoreText.text = string.Format("{0:n0}",playerLogic.nScore);    // {0:n0} => 세 자리마다 쉬묘로 나눠주는 숫자 양식
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

        //리스폰 인덱스 증가 - spawnList 끝을 알림
        spawnIndex++;
        if (spawnIndex == spawnList.Count)
        {
            spawnEnd = true;
            return;
        }
        //다음 리스폰 딜레이 갱신
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
