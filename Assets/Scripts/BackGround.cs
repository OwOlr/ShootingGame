 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    public float speed;
    public int startIndex;
    public int endIndex;
    public Transform[] sprites;

    float viewHeight;

    private void Awake()
    {
        viewHeight = Camera.main.orthographicSize * 2;
    }
    // Update is called once per frame
    void Update()
    {
        Move();
        Scrolling();

    }

    void Move()
    {
        Vector2 curPos = transform.position;
        Vector2 nextPos = Vector2.down * speed * Time.deltaTime;
        transform.position = curPos + nextPos;
    }
    void Scrolling()
    {
        //������ ��������Ʈ�� ī�޶� �þ߿��� ����� �� ����.
        if (sprites[endIndex].position.y < viewHeight * (-1))
        {
            //Sprite ReUse
            Vector2 backSpritePos = sprites[startIndex].localPosition;
            Vector2 frontSpritePos = sprites[endIndex].localPosition;
            sprites[endIndex].transform.localPosition = backSpritePos + Vector2.up * viewHeight;

            //Cursor Index Change
            int startIndexSave = startIndex;
            startIndex = endIndex;
            endIndex = (startIndexSave -1 == -1) ? sprites.Length-1 : startIndexSave -1;
        }
    }
}
