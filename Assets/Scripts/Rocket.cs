using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public GameObject rocketVanishEffect, secondArmor, thirdArmor, fourthArmor;
    private GameObject go;
    private GameManager gameManager;
    private Player pl;
    private readonly float enemySpeed = 35;
    private void Start()
    {
        if(GameObject.FindGameObjectWithTag("Player") == null)
        {
            Destroy(gameObject);
        }
        pl = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        gameManager = GameObject.FindGameObjectWithTag("GameManagerTag").GetComponent<GameManager>();
        if (gameManager.stage == 3 && gameManager.secondArmorCollected == false)
        {
            go = Instantiate(secondArmor, gameObject.transform);
        }
        else if (gameManager.stage == 6 && gameManager.thirdArmorCollected == false)
        {
            go = Instantiate(thirdArmor, gameObject.transform);
        }
        else if (gameManager.stage == 9 && gameManager.fourthArmorCollected == false)
        {
            go = Instantiate(fourthArmor, gameObject.transform);
        }

        Destroy(gameObject, 10f);
    }
    private void Update()
    {
        transform.position += Vector3.left * enemySpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (gameManager.stage == 3)
        {
            if (other.tag == "PlayerProjectileTag" && gameManager.secondArmorCollected == false)
            {
                Player ps = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
                ps.absorbSkin = true;
                Destroy(transform.GetChild(0).gameObject);
                Rigidbody2D rb = go.GetComponent<Rigidbody2D>();
                rb.velocity = Vector3.down * 50;
                transform.DetachChildren();
                Destroy(gameObject);
            }
            else if (other.tag == "PlayerProjectileTag" && gameManager.secondArmorCollected == true)
            {
                RocketVanish();
            }
        }
        else if (gameManager.stage == 6)
        {
            if (other.tag == "PlayerProjectileTag" && gameManager.thirdArmorCollected == false)
            {
                Player ps = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
                ps.absorbSkin = true;
                Destroy(transform.GetChild(0).gameObject);
                Rigidbody2D rb = go.GetComponent<Rigidbody2D>();
                rb.velocity = Vector3.down * 50;
                transform.DetachChildren();
                Destroy(gameObject);
            }
            else if (other.tag == "PlayerProjectileTag" && gameManager.thirdArmorCollected == true)
            {
                RocketVanish();
            }
        }
        else if (gameManager.stage == 9)
        {
            if (other.tag == "PlayerProjectileTag" && gameManager.fourthArmorCollected == false)
            {
                Player ps = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
                ps.absorbSkin = true;
                Destroy(transform.GetChild(0).gameObject);
                Rigidbody2D rb = go.GetComponent<Rigidbody2D>();
                rb.velocity = Vector3.down * 50;
                transform.DetachChildren();
                Destroy(gameObject);
            }
            else if (other.tag == "PlayerProjectileTag" && gameManager.fourthArmorCollected == true)
            {
                RocketVanish();
            }
        }
        else
        {
            if (other.tag == "PlayerProjectileTag" && pl.timesTriggered < 2)
            {
                RocketVanish();
            }
        }
    }

    private void RocketVanish()
    {
        Destroy(transform.GetChild(0).gameObject);
        GameObject go = Instantiate(rocketVanishEffect, gameObject.transform);
        Destroy(go, 1);
        transform.DetachChildren();
        pl.EnableFrozenProjectile(1);
        gameManager.ScoreChange(50);
        Destroy(gameObject);
    }
}
