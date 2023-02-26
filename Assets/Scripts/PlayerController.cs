using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float power = 0f;
    public int life = 3;
    public int boom;
    public bool isBoomTime;

    public bool isHit = false;
    public bool isdelCol = false;

    public bool isTouchTop = false;
    public bool isTouchBottom = false;
    public bool isTouchLeft = false;
    public bool isTouchRight = false;
    public bool isStop = false;

    public GameObject bulletPrefebA;
    public GameObject bulletPrefebB;
    public float curBulletDelay = 0f;
    public float maxBulletDelay = 1f;

    public float curColDelay = 0f;
    public float maxColDelay = 1f;

    public int nScore;
    public GameObject boomEffectObj;
    Animator anim;

    public ObjectManager objectManager;

    public GameObject[] followers;


    private void Start()
    {
        anim = GetComponent<Animator>();


    }

    // Update is called once per frame
    void Update()
    {
        if (isStop == false)
        {
            Move();
            Fire();
            Boom();

            RelodadBullet();
        }


    }

    private void FixedUpdate()
    {
        if (isdelCol)
        {
            curColDelay += Time.deltaTime;

        }
        if (curColDelay > maxColDelay)
        {

            isdelCol = false;
            isStop = false;
            gameObject.GetComponent<PolygonCollider2D>().enabled = true;
            curColDelay = 0;
        }

        if (isHit)
        {

            float val = Mathf.Sin(Time.time * 50);

            if (val > 0)
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }
            return;

        }

    }

    public void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        anim.SetInteger("Input", (int)h);
        if ((isTouchRight && h == 1) || (isTouchLeft && h == -1))
        {
            h = 0;
        }
        if ((isTouchTop && v == 1) || (isTouchBottom && v == -1))
        {
            v = 0;
        }

        Vector3 curPos = transform.position;
        Vector3 nextPos = new Vector3(h, v, 0) * speed * Time.deltaTime;

        transform.position = curPos + nextPos;
    }

    void Fire()
    {
        if (!Input.GetButton("Fire1"))
        {
            return;
        }
        if (curBulletDelay < maxBulletDelay)
        {
            return;
        }

        Power();


        curBulletDelay = 0;
    }
    void RelodadBullet()
    {
        curBulletDelay += Time.deltaTime;   //deltaTime = 프레임간의 간격
    }

    public void Boom()
    {
        if (!Input.GetButton("Fire2"))
            return;

        if (isBoomTime)
            return;

        if (boom == 0)
            return;

        boom--;
        isBoomTime = true;
        GameManager.instance.UpdateBoomIcon(boom);

        // 1. Effect Visible!
        boomEffectObj.SetActive(true);
        // Final. Effect Visible Off!
        Invoke("OffBoomEffect", 2.0f);

        // 2. Remove Enemy - 오브젝트 풀링 교체
        GameObject[] enemiesL = objectManager.GetPool("EnemyL");
        GameObject[] enemiesM = objectManager.GetPool("EnemyM");
        GameObject[] enemiesS = objectManager.GetPool("EnemyS");
        
        foreach (var delenemyL in enemiesL)
        {
            if (delenemyL.activeSelf)
            {
                Enemy enemyLogic = delenemyL.GetComponent<Enemy>();
                enemyLogic.OnHit(1000);
                delenemyL.SetActive(false);
            }
        }
        foreach (var delenemyM in enemiesM)
        {
            if (delenemyM.activeSelf)
            {
                Enemy enemyLogic = delenemyM.GetComponent<Enemy>();
                enemyLogic.OnHit(1000);
                delenemyM.SetActive(false);
            }
        }
        foreach (var delenemyS in enemiesS)
        {
            if (delenemyS.activeSelf)
            {
                Enemy enemyLogic = delenemyS.GetComponent<Enemy>();
                enemyLogic.OnHit(1000);
                delenemyS.SetActive(false);
            }
        }

        GameObject[] enemyBulletsA = objectManager.GetPool("BulletEnemyA");
        GameObject[] enemyBulletsB = objectManager.GetPool("BulletEnemyB");
        foreach (var delBulletsA in enemyBulletsA)
        {
            if (delBulletsA.activeSelf)
                delBulletsA.SetActive(false);
        }
        foreach (var delBulletsB in enemyBulletsB)
        {
            if (delBulletsB.activeSelf)
                delBulletsB.SetActive(false);
        }
    }


    void Power()
    {
        switch (power)
        {
            case 1:
                {
                    GameObject bullet = objectManager.MakeObj("BulletPlayerA");
                    bullet.transform.position = transform.position;
                    bullet.transform.rotation = Quaternion.identity;

                    //Instantiate(bulletPrefebA,transform.position,Quaternion.identity);
                    Rigidbody2D rd = bullet.GetComponent<Rigidbody2D>();

                    rd.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                }
                break;

            case 2:
                {
                    GameObject bulletR = objectManager.MakeObj("BulletPlayerA");
                    bulletR.transform.position = transform.position + Vector3.right * 0.1f;
                    bulletR.transform.rotation = Quaternion.identity;
                    //Instantiate(bulletPrefebA, transform.position + Vector3.right * 0.1f, Quaternion.identity);
                    Rigidbody2D rdR = bulletR.GetComponent<Rigidbody2D>();
                    rdR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

                    GameObject bulletL = objectManager.MakeObj("BulletPlayerA");
                    bulletL.transform.position = transform.position + Vector3.left * 0.1f;
                    bulletL.transform.rotation = Quaternion.identity;
                    //Instantiate(bulletPrefebA,transform.position + Vector3.left * 0.1f,Quaternion.identity);
                    Rigidbody2D rdL = bulletL.GetComponent<Rigidbody2D>();
                    rdL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                }
                break;

            default:
                {
                    GameObject bulletC = objectManager.MakeObj("BulletPlayerB");
                    bulletC.transform.position = transform.position + Vector3.up * 0.1f;
                    bulletC.transform.rotation = Quaternion.identity;
                    //Instantiate(bulletPrefebB,transform.position + Vector3.up * 0.1f,Quaternion.identity);
                    Rigidbody2D rdC = bulletC.GetComponent<Rigidbody2D>();
                    rdC.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

                    GameObject bulletR = objectManager.MakeObj("BulletPlayerA");
                    bulletR.transform.position = transform.position + Vector3.right * 0.2f;
                    bulletR.transform.rotation = Quaternion.identity;
                    //Instantiate(bulletPrefebA,transform.position + Vector3.right * 0.2f,Quaternion.identity);
                    Rigidbody2D rdR = bulletR.GetComponent<Rigidbody2D>();
                    rdR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

                    GameObject bulletL = objectManager.MakeObj("BulletPlayerA");
                    bulletL.transform.position = transform.position + Vector3.left * 0.2f;
                    bulletL.transform.rotation = Quaternion.identity;
                    //Instantiate(bulletPrefebA,transform.position + Vector3.left * 0.2f,Quaternion.identity);
                    Rigidbody2D rdL = bulletL.GetComponent<Rigidbody2D>();
                    rdL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                }
                break;

        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerBorder")
        {

            switch (collision.gameObject.name)
            {
                case "Top":
                    isTouchTop = true;
                    break;
                case "Bottom":
                    isTouchBottom = true;
                    break;
                case "Right":
                    isTouchRight = true;
                    break;
                case "Left":
                    isTouchLeft = true;
                    break;
                default:
                    break;
            }

        }
        if (collision.gameObject.tag == "EnemyBullet")
        {
            isHit = true;
            isdelCol = true;
            life--;
            power = 1;
            gameObject.GetComponent<PolygonCollider2D>().enabled = false;
            isStop = true;

            GameManager.instance.UpdateLifeIcon(life);

            if (life == 0)
            {
                GameManager.instance.GameOver();
                
            }
            else
            {
                GameManager.instance.ResPawnPlayer();
            }

        }

        if (collision.gameObject.tag == "Item")
        {
            Item item = collision.gameObject.GetComponent<Item>();

            switch (item.type)
            {
                case ItemType.Coin:
                    nScore += 1000;
                    break;
                case ItemType.Power:
                    
                    if (power > 6)
                    {
                        power = 6;
                        nScore += 500;
                    }
                    power++;
                    AddFollower();
                    break;
                case ItemType.Boom:
                    {
                        boom++;
                        GameManager.instance.UpdateBoomIcon(boom);
                        if (boom >= 3)
                        {
                            power = 3;
                            nScore += 500;
                        }
                    }
                    break;
            }

            collision.gameObject.SetActive(false);

        }
    }

    void AddFollower()
    {
        if (power == 4)
            followers[0].SetActive(true);
        else if (power == 5)
            followers[1].SetActive(true);
        else if (power == 6)
            followers[2].SetActive(true);

    }

    void OffBoomEffect()
    {
        boomEffectObj.SetActive(false);
        isBoomTime = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerBorder")
        {
            switch (collision.gameObject.name)
            {
                case "Top":
                    isTouchTop = false;
                    break;
                case "Bottom":
                    isTouchBottom = false;
                    break;
                case "Right":
                    isTouchRight = false;
                    break;
                case "Left":
                    isTouchLeft = false;
                    break;
                default:
                    break;
            }

        }
    }
}
