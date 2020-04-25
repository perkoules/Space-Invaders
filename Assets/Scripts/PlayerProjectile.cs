using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    private GameManager gameManager;
    private AudioSource audioSource;
    private Rigidbody2D rb;
    private Player pl;

    private void Awake()
    {
        if (gameObject.name == "PlayerProjectileGray(Clone)")
        {
            gameObject.tag = "GrayTag";
        }
    }
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.PlayOneShot(audioSource.clip);
        pl = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        gameManager = GameObject.FindGameObjectWithTag("GameManagerTag").GetComponent<GameManager>();
        rb = GetComponent<Rigidbody2D>();
        if (gameObject.name == "PlayerProjectileGreen(Clone)")
        {
            rb.velocity = Vector2.up * 50;
        }
        else if (gameObject.name == "PlayerProjectileGray(Clone)")
        {
            GameObject[] tempGray = GameObject.FindGameObjectsWithTag("GrayTag");
            if(tempGray[0] == this.gameObject)
            {
                rb.velocity = Vector2.up * 300;
            }
            else
            {
                rb.velocity = Vector2.up * 300;
            }
        }
        else
        {
            rb.velocity = Vector2.up * 300;
        }
    }

    private void Update()
    {
        if (gameObject.name == "PlayerProjectileGreen(Clone)")
        {
            transform.position = Vector3.MoveTowards(transform.position, GameObject.FindGameObjectWithTag("Enemy").transform.position, 5);
        }
        else if (gameObject.name == "PlayerProjectileGray(Clone)")
        {
            GameObject[] tempGray = GameObject.FindGameObjectsWithTag("GrayTag");
            if (tempGray[0] == this.gameObject)
            {
                transform.position += Vector3.left * 50 * Time.deltaTime;
            }
            else
            {
                transform.position += Vector3.right * 50 * Time.deltaTime;
            }
        }
    }

    
}
