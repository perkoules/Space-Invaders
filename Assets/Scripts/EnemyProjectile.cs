using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class EnemyProjectile : MonoBehaviour
{
    public bool isWheelReady = false;

    private Enemy enemy;
    private PowerUp powerUp;
    private GameObject player;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Collider2D col;
    private Color orange = new Color(255, 165, 0);
    private int absorbedPower;
    private float distance, distanceAbs = 0, enemyProjectileSpeed;
    private AudioSource audioSource;
    private LaserScript laserScript;

    private void Start()
    {

        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            laserScript = player.GetComponentInChildren<LaserScript>();
        }
        powerUp = GameObject.FindGameObjectWithTag("IndicatorTag").GetComponent<PowerUp>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        audioSource = gameObject.GetComponent<AudioSource>();
        enemyProjectileSpeed = enemy.enemyProjectileSpeed;
        rb.velocity = Vector2.down * enemyProjectileSpeed;
        audioSource.PlayOneShot(audioSource.clip);
        
    }

    private void Update()
    {
        if (player != null)
        {
            distance = gameObject.transform.position.y - (player.transform.position.y + 5.5f);
            distanceAbs = Mathf.Abs(distance);
            if (distance < 0)
            {
                distanceAbs = 100;
            }
            if ((Input.GetButtonDown("GunControl") || Input.GetAxis("Vertical") < 0) && powerUp.doIHaveAmmo == false)
            {
                if (Mathf.Abs(gameObject.transform.position.x - player.transform.position.x) < 8.3f)
                {
                    if (transform.GetChild(1).tag == "Dissolve")
                    {
                        rb.velocity = Vector2.zero;
                        spriteRenderer.sprite = null;
                        transform.GetChild(0).gameObject.SetActive(false);
                        transform.GetChild(1).gameObject.SetActive(true);
                        Destroy(gameObject, 1);
                    }
                    laserScript.endLaserPosition = laserScript.startLaserPosition;
                    absorbedPower = Mathf.Abs(Mathf.RoundToInt(distanceAbs) - 100);
                    powerUp.absorbedPower = absorbedPower;
                    powerUp.startSpinning = true;
                    powerUp.doIHaveAmmo = true;
                }
            }
            //StartCoroutine(Absorb());
        }
        else
        {
            audioSource.Stop();
            Destroy(gameObject);
        }
    }
    /*IEnumerator Absorb()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Mathf.Abs(gameObject.transform.position.x - player.transform.position.x) < 8.3f && powerUp.doIHaveAmmo == false)
            {
                StopCoroutine(Absorb());
                if (transform.GetChild(1).tag == "Dissolve")
                {
                    rb.velocity = Vector2.zero;
                    spriteRenderer.sprite = null;
                    transform.GetChild(0).gameObject.SetActive(false);
                    transform.GetChild(1).gameObject.SetActive(true);
                    Destroy(gameObject, 1);
                }
                laserScript.endLaserPosition = laserScript.startLaserPosition;
                absorbedPower = Mathf.Abs(Mathf.RoundToInt(distanceAbs) - 100);
                powerUp.absorbedPower = absorbedPower;
                powerUp.startSpinning = true;
                powerUp.doIHaveAmmo = true;
            }
        }
        yield return null;
    }*/
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Destroy(gameObject);
            GameManager gameManager = GameObject.FindGameObjectWithTag("GameManagerTag").GetComponent<GameManager>();
            StartCoroutine(gameManager.PlayerGotHit());
        }
    }
}
