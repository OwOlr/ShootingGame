using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float power = 0f;
    public float life = 3;
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

    public GameObject gameMObj;

    SpriteRenderer spRenderer;
    Animator anim;


    private void Start()
    {
        anim = GetComponent<Animator>();
        spRenderer = GetComponent<SpriteRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isStop == false)
        {
            Move();
        }
        
        Fire();
        RelodadBullet();


       

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

        anim.SetInteger("Input" , (int)h);
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



    void Power()
    {
        switch (power)
        {
            case 1:
                {
                    GameObject bullet = Instantiate(bulletPrefebA,
                      transform.position,
                      Quaternion.identity);
                    Rigidbody2D rd = bullet.GetComponent<Rigidbody2D>();

                    rd.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                }
                break;

            case 2:
                {  GameObject bulletR = Instantiate(bulletPrefebA, 
                    transform.position + Vector3.right * 0.1f, 
                    Quaternion.identity);
                Rigidbody2D rdR = bulletR.GetComponent<Rigidbody2D>();
                
                rdR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

                GameObject bulletL = Instantiate(bulletPrefebA, 
                    transform.position + Vector3.left * 0.1f, 
                    Quaternion.identity);
                Rigidbody2D rdL = bulletL.GetComponent<Rigidbody2D>();
                
                rdL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                }
                break;

            case 3:
                { 
                GameObject bulletC = Instantiate(bulletPrefebB,
                    transform.position + Vector3.up * 0.1f,
                    Quaternion.identity);
                Rigidbody2D rdC = bulletC.GetComponent<Rigidbody2D>();

                rdC.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

                GameObject bulletR = Instantiate(bulletPrefebA,
                    transform.position + Vector3.right * 0.2f,
                    Quaternion.identity);
                Rigidbody2D rdR = bulletR.GetComponent<Rigidbody2D>();

                rdR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

                GameObject bulletL = Instantiate(bulletPrefebA,
                    transform.position + Vector3.left * 0.2f,
                    Quaternion.identity);
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
        if(collision.gameObject.tag == "EnemyBullet")
        {

            GameManager gameLogic = gameMObj.GetComponent<GameManager>();
            if (life == 0)
            {
                gameLogic.GameOver();
            }
            else
            {
                isHit = true;
                isdelCol = true;
                life--;
               
                gameObject.GetComponent<PolygonCollider2D>().enabled = false;
                isStop = true;
                gameLogic.ResPawnPlayer();

                

            }
        }
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
