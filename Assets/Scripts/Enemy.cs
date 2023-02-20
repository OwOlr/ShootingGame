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

    ///Start보다 이전에 실행되며, 스크립트가 비활성화가 되어있어도 실행된다.
    ///반대로 Start는 스크립트가 활성화가 되어야만 실행이된다.
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
        if (nPoint == 3 || nPoint ==4)  ///오른쪽 스폰 포인트 배열 인덱스 값
        {
            rd.velocity = new Vector2(speed, -1);
        }
       else if (nPoint == 5 || nPoint == 6)  ///왼쪽 스폰 포인트 배열 인덱스 값
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
