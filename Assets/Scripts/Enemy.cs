using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string enemyName;
    public float heathSave;
    public float speed;
    public float health;
    public int enemyScore;

    public float curBulletDelay = 0f;
    public float maxBulletDelay = 1f;

    public GameObject playerObject;

    public Sprite[] sprites;
    public GameObject bulletPrefeb;
    SpriteRenderer spriteRender;

    public bool isStop = false;

    public Rigidbody2D rd;

    public ObjectManager objectManager;

    Animator anim;

    public int patternIndex;
    public int curPatternCount;     //현재 패턴 사용 횟수
    public int[] maxPatternCount;   //패턴 사용 횟수

    ///Start보다 이전에 실행되며, 스크립트가 비활성화가 되어있어도 실행된다.
    ///반대로 Start는 스크립트가 활성화가 되어야만 실행이된다.
    private void Awake()
    {
        rd = GetComponent<Rigidbody2D>();

        spriteRender = GetComponent<SpriteRenderer>();
        heathSave = health;
        if (enemyName == "B")
        {
            anim = GetComponent<Animator>();

        }
    }
    private void OnEnable()
    {
        health = heathSave;
        if (enemyName == "B")
        {
            Invoke("Stop", 2);
        }
    }

    void Stop()
    {
        if (!gameObject.activeSelf)
            return;
        Rigidbody2D rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = Vector2.zero;

        Invoke("Think", 2);
    }

    void Think()
    {
        patternIndex = patternIndex == 3 ? 0 : patternIndex + 1;
        curPatternCount = 0;
        switch (patternIndex)
        {
            case 0:
                FireFoward();
                break;
            case 1:
                FireShot();
                break;
            case 2:
                FireArc();
                break;
            case 3:
                FireAround();
                break;
        }
    }

    void FireFoward()
    {

        //플레이어 방향으로 4발 발사.
        GameObject bulletR = objectManager.MakeObj("BulletBossA");
        bulletR.transform.position = transform.position + Vector3.right * 0.3f;
        GameObject bulletRR = objectManager.MakeObj("BulletBossA");
        bulletRR.transform.position = transform.position + Vector3.right * 0.45f;

        GameObject bulletL = objectManager.MakeObj("BulletBossA");
        bulletL.transform.position = transform.position + Vector3.left * 0.3f;
        GameObject bulletLL = objectManager.MakeObj("BulletBossA");
        bulletLL.transform.position = transform.position + Vector3.left * 0.45f;

        Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidRR = bulletRR.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidLL = bulletLL.GetComponent<Rigidbody2D>();


        rigidR.AddForce(Vector2.down * 8, ForceMode2D.Impulse);
        rigidRR.AddForce(Vector2.down * 8, ForceMode2D.Impulse);
        rigidL.AddForce(Vector2.down * 8, ForceMode2D.Impulse);
        rigidLL.AddForce(Vector2.down * 8, ForceMode2D.Impulse);

        //패턴 횟수 세기
        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
            Invoke("FireFoward", 2);

        else
            Invoke("Think", 3);

    }

    void FireShot()
    {
        //플레이어 방향으로 랜덤하게 샷건 4발 발사.
        for (int index = 0; index < 5; index++)
        {
            GameObject bullet = objectManager.MakeObj("BulletEnemyB");
            bullet.transform.position = transform.position;

            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            Vector2 dirVec = playerObject.transform.position - transform.position;
            Vector2 ranVec = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(0f, 2f));
            dirVec += ranVec;

            rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
        }


        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
            Invoke("FireShot", 3.5f);

        else
            Invoke("Think", 3);
    }
    void FireArc()
    {
        //부채모양으로 왔다 갔다  발사

        GameObject bullet = objectManager.MakeObj("BulletEnemyA");
        bullet.transform.position = transform.position;
        bullet.transform.rotation = Quaternion.identity;

        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        Vector2 dirVec = new Vector2(Mathf.Sin(Mathf.PI * 10 * curPatternCount / maxPatternCount[patternIndex]), -1);

        rigid.AddForce(dirVec.normalized * 5, ForceMode2D.Impulse);

        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
            Invoke("FireArc", 0.15f);

        else
            Invoke("Think", 3);
    }
    void FireAround()
    {
        //전체 공격 - 원 형태로 발사
        int roundNumA = 50;
        int roundNumB = 40;
        int roundNum = curPatternCount % 2 == 0 ? roundNumA : roundNumB;
        for (int index = 0; index < roundNum; index++)
        {
            GameObject bullet = objectManager.MakeObj("BulletBossB");
            bullet.transform.position = transform.position;
            bullet.transform.rotation = Quaternion.identity;

            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            Vector2 dirVec = new Vector2(Mathf.Cos(Mathf.PI * 2 * index / roundNum),
                                        Mathf.Sin(Mathf.PI * 2 * index / roundNum));

            rigid.AddForce(dirVec.normalized * 2, ForceMode2D.Impulse);

            Vector3 rotVec = Vector3.forward * 360 * index / roundNum + Vector3.forward * 90;
            bullet.transform.Rotate(rotVec);
        }

        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
            Invoke("FireAround", 0.7f);

        else
            Invoke("Think", 3);

    }

    // Update is called once per frame
    void Update()
    {
        if (isStop == false)
        {
            if (enemyName == "B")
            {
                return;
            }
            Fire();
            RelodadBullet();
        }


    }

    public void Move(int nPoint)
    {
        if (nPoint == 3 || nPoint == 4)  ///오른쪽 스폰 포인트 배열 인덱스 값
        {
            transform.Rotate(Vector3.forward * 90);
            rd.velocity = new Vector2(speed, -1);
        }
        else if (nPoint == 5 || nPoint == 6)  ///왼쪽 스폰 포인트 배열 인덱스 값
        {
            transform.Rotate(Vector3.back * 90);
            rd.velocity = new Vector2(speed * (-1), -1);
        }
        else
        {
            rd.velocity = Vector3.down * speed;
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border" && enemyName != "B")
        {
            gameObject.SetActive(false);
            transform.rotation = Quaternion.identity;
        }
        else if (collision.gameObject.tag == "Bullet")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();

            OnHit(bullet.power);

        }



    }
    public void OnHit(float bulletPower)
    {
        health -= bulletPower;
        if (enemyName == "B")
        {
            anim.SetTrigger("OnHit");
        }
        else
        {
            spriteRender.sprite = sprites[1];
            Invoke("ReturnSprite", 0.1f);

        }

        if (health <= 0)
        {
            PlayerController playerLogic = playerObject.GetComponent<PlayerController>();
            playerLogic.nScore += enemyScore;
            //transform.rotation = Quaternion.identity;
            gameObject.SetActive(false);
        }

    }
    void ReturnSprite()
    {
        spriteRender.sprite = sprites[0];
    }

    void Fire()
    {

        if (curBulletDelay > maxBulletDelay)
        {
            Power();

            curBulletDelay = 0;
        }

    }

    void Power()
    {
        GameObject bulletObj = objectManager.MakeObj("BulletEnemyA");
        bulletObj.transform.position = transform.position;
        //Instantiate(bulletPrefeb, transform.position, Quaternion.identity);
        Rigidbody2D rigidBullet = bulletObj.GetComponent<Rigidbody2D>();


        Vector3 dirVec = playerObject.transform.position - transform.position;
        rigidBullet.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
    }

    void RelodadBullet()
    {
        curBulletDelay += Time.deltaTime;   //deltaTime = 프레임간의 간격
    }
}
