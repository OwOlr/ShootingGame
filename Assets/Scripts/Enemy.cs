using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
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

    ///Start���� ������ ����Ǹ�, ��ũ��Ʈ�� ��Ȱ��ȭ�� �Ǿ��־ ����ȴ�.
    ///�ݴ�� Start�� ��ũ��Ʈ�� Ȱ��ȭ�� �Ǿ�߸� �����̵ȴ�.
    private void Awake()
    {
       rd = GetComponent<Rigidbody2D>();

        spriteRender = GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
       //rd = GetComponent<Rigidbody2D>();
       //spriteRender = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isStop == false)
        {
            Fire();
            RelodadBullet();
        }
        

    }

    public void Move(int nPoint)
    {
        if (nPoint == 3 || nPoint ==4)  ///������ ���� ����Ʈ �迭 �ε��� ��
        {
            transform.Rotate(Vector3.forward * 90);
            rd.velocity = new Vector2(speed, -1);
        }
       else if (nPoint == 5 || nPoint == 6)  ///���� ���� ����Ʈ �迭 �ε��� ��
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
        if (collision.gameObject.tag == "Border")
        {
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "Bullet")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            
            OnHit(bullet.power);
            
            
            Destroy(collision.gameObject);
        }
            

        
    }
    public void OnHit(float bulletPower)
    {
        health -= bulletPower;
        spriteRender.sprite = sprites[1];
        Invoke("ReturnSprite", 0.1f);

        if (health <= 0)
        {
            PlayerController playerLogic = playerObject.GetComponent<PlayerController>();
            playerLogic.nScore += enemyScore;
            Destroy(gameObject);
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
        GameObject bulletObj = Instantiate(bulletPrefeb, transform.position, Quaternion.identity);
        Rigidbody2D rigidBullet = bulletObj.GetComponent<Rigidbody2D>();



        Vector3 dirVec = playerObject.transform.position - transform.position;
        rigidBullet.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
    }

    void RelodadBullet()
    {
        curBulletDelay += Time.deltaTime;   //deltaTime = �����Ӱ��� ����
    }
}
