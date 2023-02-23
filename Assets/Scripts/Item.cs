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
    void Start()
    {
        itemRigid = GetComponent<Rigidbody2D>();
        itemRigid.velocity = Vector2.down * 3;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
