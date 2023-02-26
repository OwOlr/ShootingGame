using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public float power = 0f;
    public float curBulletDelay = 0f;
    public float maxBulletDelay = 1f;

    public ObjectManager objectManager;

    public Vector2 followPos;   //팔로워의 위치
    public int followDelay;     //Follower 생성 딜레이 시간
    public Transform parent;    //부모 위치
    public Queue<Vector2> parentPos;    //부모 위치를 실시간으로 리스트에 저장

    private void Awake()
    {
        parentPos = new Queue<Vector2>();
    }

    void Update()
    {
        Watch();
        Follow();
        Fire();
        RelodadBullet();

    }
    void Watch()
    {
        //Queue = FIFO(First Input First OUT) - "먼저 들어간 사람이 먼저 나간다!"
        //1. Input Parent Pos
        if (!parentPos.Contains(parent.position))
        {
            parentPos.Enqueue(parent.position);
        }

        //2. OutPut Pos
        if (parentPos.Count > followDelay)
        {
            followPos = parentPos.Dequeue();    //리스트에서 제거당하면서 값을 반환.
        }
        
        else if(parentPos.Count < followDelay)
        {
            followPos = parent.position;
        }
        
    }
    void Follow()
    {
        transform.position = followPos;
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
    void Power()
    {
        switch (power)
        {
            case 1:
                {
                    GameObject bullet = objectManager.MakeObj("FollowerBullet");
                    bullet.transform.position = transform.position;
                    bullet.transform.rotation = Quaternion.identity;

                    //Instantiate(bulletPrefebA,transform.position,Quaternion.identity);
                    Rigidbody2D rd = bullet.GetComponent<Rigidbody2D>();

                    rd.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                }
                break;

        }

    }

    void RelodadBullet()
    {
        curBulletDelay += Time.deltaTime;   //deltaTime = 프레임간의 간격
    }
}
