using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuddyScript : MonoBehaviour
{
    public bool readyToStick = false;
    private GameManager gameManager;
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManagerTag").GetComponent<GameManager>();
    }


    void Update()
    {
        if (gameManager.stage >= 3 && gameManager.stage < 9)
        {
            if (readyToStick == true)
            {
                transform.position = Vector3.MoveTowards(transform.position, GameObject.FindGameObjectWithTag("Enemy").transform.position, 5);
            }
            else
            {
                gameObject.transform.RotateAround(transform.parent.transform.position, Vector3.forward, 5);
                gameObject.transform.Rotate(Vector3.forward * 60);
            }
        }
        else if (gameManager.stage >= 9)
        {
            if (readyToStick == true)
            {
                transform.position = Vector3.MoveTowards(transform.position, GameObject.FindGameObjectWithTag("Enemy").transform.position, 5);
            }
            else
            {
                gameObject.transform.RotateAround(transform.parent.transform.position, Vector3.forward, 15);
                gameObject.transform.Rotate(Vector3.forward * 60);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "EnemyProjectileTag" && gameManager.stage >= 9)
        {
            Destroy(other.gameObject);
        }
    }
}
