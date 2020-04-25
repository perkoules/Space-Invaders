using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrozenProjectile : MonoBehaviour
{
    private Rigidbody2D rb;
    private GameObject[] enemies;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.up * 50;
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, GameObject.FindGameObjectWithTag("Enemy").transform.position, 5);
    }
}
