using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float power = 0f;

    public bool isRotate;

    // Update is called once per frame
    void Update()
    {
        if (isRotate)
        {
            transform.Rotate(Vector3.forward * 10);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            gameObject.SetActive(false);
        }
    }
}
