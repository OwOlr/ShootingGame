using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public GameObject enemyLPrefeb;
    public GameObject enemyMPrefeb;
    public GameObject enemySPrefeb;
    public GameObject enemyBPrefeb;

    public GameObject itemCoinPrefeb;
    public GameObject itemPowerPrefeb;
    public GameObject itemBoomPrefeb;

    public GameObject bulletPlayerAPrefeb;
    public GameObject bulletPlayerBPrefeb;

    public GameObject bulletFollowerPreFeb;

    public GameObject bulletEnemyAPrefeb;
    public GameObject bulletEnemyBPrefeb;

    public GameObject bulletBossAPrefeb;
    public GameObject bulletBossBPrefeb;



    GameObject[] enemyL;
    GameObject[] enemyM;
    GameObject[] enemyS;
    GameObject[] enemyB;

    GameObject[] itemCoin;
    GameObject[] itemPower;
    GameObject[] itemBoom;

    GameObject[] bulletPlayerA;
    GameObject[] bulletPlayerB;

    GameObject[] bulletFollower;

    GameObject[] bulletEnemyA;
    GameObject[] bulletEnemyB;

    GameObject[] bulletBossA;
    GameObject[] bulletBossB;


    GameObject[] targetPool;

    private void Awake()
    {

        enemyL = new GameObject[10];
        enemyM = new GameObject[10];
        enemyS = new GameObject[20];
        enemyB = new GameObject[1];

        itemCoin = new GameObject[20];
        itemPower = new GameObject[10];
        itemBoom = new GameObject[10];

        bulletPlayerA = new GameObject[100];
        bulletPlayerB = new GameObject[100];

        bulletFollower = new GameObject[100];

        bulletEnemyA = new GameObject[100];
        bulletEnemyB = new GameObject[100];

        bulletBossA = new GameObject[100];
        bulletBossB = new GameObject[100];

        Generate();
    }

    void Generate()
    {
        for (int index = 0; index < enemyB.Length; index++)
        {
            enemyB[index] = Instantiate(enemyBPrefeb);
            enemyB[index].SetActive(false);
        }
        //배열 10개 (적 L, M or 아이템 파워, 폭발)
        for (int index = 0; index < enemyL.Length; index++)
        {
            enemyL[index] = Instantiate(enemyLPrefeb);
            enemyL[index].SetActive(false);

            enemyM[index] = Instantiate(enemyMPrefeb);
            enemyM[index].SetActive(false);

            itemPower[index] = Instantiate(itemPowerPrefeb);
            itemPower[index].SetActive(false);

            itemBoom[index] = Instantiate(itemBoomPrefeb);
            itemBoom[index].SetActive(false);

        }

        //배열 20개 (적S , 아이템 코인)
        for (int index = 0; index < enemyS.Length; index++)
        {
            enemyS[index] = Instantiate(enemySPrefeb);
            enemyS[index].SetActive(false);

            itemCoin[index] = Instantiate(itemCoinPrefeb);
            itemCoin[index].SetActive(false);
        }

        //배열 100개 (플레이어 총알 A,B or 적 총알 A,B)
        for (int index = 0; index < bulletPlayerA.Length; index++)
        {
            bulletPlayerA[index] = Instantiate(bulletPlayerAPrefeb);
            bulletPlayerA[index].SetActive(false);

            bulletPlayerB[index] = Instantiate(bulletPlayerBPrefeb);
            bulletPlayerB[index].SetActive(false);

            bulletFollower[index] = Instantiate(bulletFollowerPreFeb);
            bulletFollower[index].SetActive(false);

            bulletEnemyA[index] = Instantiate(bulletEnemyAPrefeb);
            bulletEnemyA[index].SetActive(false);

            bulletEnemyB[index] = Instantiate(bulletEnemyBPrefeb);
            bulletEnemyB[index].SetActive(false);

            bulletBossA[index] = Instantiate(bulletBossAPrefeb);
            bulletBossA[index].SetActive(false);

            bulletBossB[index] = Instantiate(bulletBossBPrefeb);
            bulletBossB[index].SetActive(false);
        }

    }

    public GameObject MakeObj(string type)
    {

        switch (type)
        {
            case "EnemyL":
                targetPool = enemyL;
                break;
            case "EnemyM":
                targetPool = enemyM;
                break;
            case "EnemyS":
                targetPool = enemyS;
                break;
            case "EnemyB":
                targetPool = enemyB;
                break;

            case "ItemCoin":
                targetPool = itemCoin;
                break;
            case "ItemPower":
                targetPool = itemPower;
                break;
            case "ItemBoom":
                targetPool = itemBoom;
                break;

            case "BulletPlayerA":
                targetPool = bulletPlayerA;
                break;
            case "BulletPlayerB":
                targetPool = bulletPlayerB;
                break;

            case "FollowerBullet":
                targetPool = bulletFollower;
                break;

            case "BulletEnemyA":
                targetPool = bulletEnemyA;
                break;
            case "BulletEnemyB":
                targetPool = bulletEnemyB;
                break;

            case "BulletBossA":
                targetPool = bulletBossA;
                break;
            case "BulletBossB":
                targetPool = bulletBossB;
                break;

        }

        for (int index = 0; index < targetPool.Length; index++)
        {
            if (!targetPool[index].activeSelf)
            {
                targetPool[index].SetActive(true);
                return targetPool[index];
            }
        }

        return null;
    }

    public GameObject[] GetPool(string type)
    {
        switch (type)
        {
            case "EnemyL":
                targetPool = enemyL;
                break;
            case "EnemyM":
                targetPool = enemyM;
                break;
            case "EnemyS":
                targetPool = enemyS;
                break;

            case "EnemyB":
                targetPool = enemyB;
                break;

            case "ItemCoin":
                targetPool = itemCoin;
                break;
            case "ItemPower":
                targetPool = itemPower;
                break;
            case "ItemBoom":
                targetPool = itemPower;
                break;

            case "BulletPlayerA":
                targetPool = bulletPlayerA;
                break;
            case "BulletPlayerB":
                targetPool = bulletPlayerB;
                break;

            case "FollowerBullet":
                targetPool = bulletFollower;
                break;

            case "BulletEnemyA":
                targetPool = bulletEnemyA;
                break;
            case "BulletEnemyB":
                targetPool = bulletEnemyB;
                break;

            case "BulletBossA":
                targetPool = bulletBossA;
                break;
            case "BulletBossB":
                targetPool = bulletBossB;
                break;

        }
        return targetPool;
    }
}


