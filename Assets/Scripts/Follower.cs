using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public float power = 0f;
    public float curBulletDelay = 0f;
    public float maxBulletDelay = 1f;

    public ObjectManager objectManager;

    public Vector2 followPos;   //�ȷο��� ��ġ
    public int followDelay;     //Follower ���� ������ �ð�
    public Transform parent;    //�θ� ��ġ
    public Queue<Vector2> parentPos;    //�θ� ��ġ�� �ǽð����� ����Ʈ�� ����

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
        //Queue = FIFO(First Input First OUT) - "���� �� ����� ���� ������!"
        //1. Input Parent Pos
        if (!parentPos.Contains(parent.position))
        {
            parentPos.Enqueue(parent.position);
        }

        //2. OutPut Pos
        if (parentPos.Count > followDelay)
        {
            followPos = parentPos.Dequeue();    //����Ʈ���� ���Ŵ��ϸ鼭 ���� ��ȯ.
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
        curBulletDelay += Time.deltaTime;   //deltaTime = �����Ӱ��� ����
    }
}
