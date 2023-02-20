using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float health;

    public Sprite[] sprites;

    SpriteRenderer spriteRender;

    Rigidbody2D rd;

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
       
       rd = GetComponent<Rigidbody2D>();

       spriteRender = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move(int nPoint)
    {
        if (nPoint == 3 || nPoint ==4)  ///������ ���� ����Ʈ �迭 �ε��� ��
        {
            rd.velocity = new Vector2(speed, -1);
        }
       else if (nPoint == 5 || nPoint == 6)  ///���� ���� ����Ʈ �迭 �ε��� ��
        {
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
    void OnHit(float bulletPower)
    {
        health -= bulletPower;
        spriteRender.sprite = sprites[1];
        Invoke("ReturnSprite", 0.1f);

        if (health <= 0)
        {
            Destroy(gameObject);
        }

    }
    void ReturnSprite()
    {
        spriteRender.sprite = sprites[0];
    }
}