using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Coin,
    Power,
    Boom
}

public class Item : MonoBehaviour
{
    public ItemType type;
    Rigidbody2D itemRigid;
    // Start is called before the first frame update
    void Awake()
    {
        itemRigid = GetComponent<Rigidbody2D>();
        
    }

    private void OnEnable()
    {
        itemRigid.velocity = Vector2.down * 1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            gameObject.SetActive(false);
        }
    }
}
