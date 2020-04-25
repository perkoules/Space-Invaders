using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public EnemyTypes enemyTypes;
    public GameObject enemyGunBarrel, damagedPartEffect;
    public GameObject[] enemyParts;
    public float enemyProjectileSpeed, enemyHealth, enemySpeed;

    private PowerUp powerUp;
    private SpriteRenderer spriteRenderer;
    private GameManager gameManager;
    private GameObject cloneProjectile, player, enemyProjectile, whirl, enemyExplosion, cloneWhirl, deathWhirl, explodedEnemy;
    private AudioSource audioSource;
    private Animator anim;
    private float enemyShootingFrequency, xmin, xmax, newX;
    private bool isMovingLeft, isMovinRight, areAllPartsDestoyed = false;
    private int enemyDamage;
    private void Awake()
    {
        gameObject.name = enemyTypes.enemyType;
        enemySpeed = enemyTypes.enemySpeed;
        enemyHealth = enemyTypes.health;
        enemyDamage = enemyTypes.damage;
        enemyProjectileSpeed = enemyTypes.projectileSpeed;
        enemyShootingFrequency = enemyTypes.shootingFrequency;
        enemyProjectile = enemyTypes.enemyProjectile;
        whirl = enemyTypes.chargingEffect;
        enemyExplosion = enemyTypes.deathExplosion;
    }

    private void Start()
    {
        if(Random.value > 0.5f)
        {
            isMovingLeft = true;
            isMovinRight = false;
        }
        else
        {
            isMovingLeft = false;
            isMovinRight = true;
        }
        float distance = transform.position.z - Camera.main.transform.position.z;
        Vector3 leftmost = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance));
        Vector3 rightmost = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance));
        xmin = leftmost.x + 50;
        xmax = rightmost.x - 5;

        anim = GetComponent<Animator>();
        powerUp = GameObject.FindGameObjectWithTag("IndicatorTag").GetComponent<PowerUp>();
        gameManager = GameObject.FindGameObjectWithTag("GameManagerTag").GetComponent<GameManager>();
        audioSource = gameObject.GetComponentInChildren<AudioSource>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
        InvokeRepeating("Whirl", 0, enemyShootingFrequency);
        InvokeRepeating("Projectiles", 2f, enemyShootingFrequency);
    }

    private void Update()
    {
        if (player != null)
        {
            Movement(isMovingLeft, isMovinRight);
        }
        else
        {
            CancelInvoke();
            audioSource.Stop();
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(25,45,0), enemySpeed * Time.deltaTime);
        }
    }
    
    public void Movement(bool left, bool right)
    {
        isMovingLeft = left;
        isMovinRight = right;
        if (isMovingLeft == true)
        {
            transform.position += Vector3.left * enemySpeed * Time.deltaTime;
            if (transform.position.x <= xmin)
            {
                if (Random.value > 0.5f)
                {
                    if (gameObject.name.Contains("Boss") || gameObject.name.Contains("Hard"))
                    {

                    }
                    else
                    {
                        StartCoroutine(Down());
                    }
                    isMovingLeft = false;
                    isMovinRight = true;
                }
                else
                {
                    isMovingLeft = false;
                    isMovinRight = true;
                }

            }
        }
        else if (isMovinRight == true)
        {
            transform.position += Vector3.right * enemySpeed * Time.deltaTime;
            if (transform.position.x >= xmax)
            {
                if (Random.value > 0.5f)
                {
                    if (gameObject.name.Contains("Boss") || gameObject.name.Contains("Hard"))
                    {

                    }
                    else
                    {
                        StartCoroutine(Up());
                    }
                    isMovinRight = false;
                    isMovingLeft = true;
                }
                else
                {
                    isMovinRight = false;
                    isMovingLeft = true;
                }
            }
        }
        else if (isMovingLeft == false && isMovinRight == false)
        {

            transform.position = Vector3.MoveTowards(transform.position, deathWhirl.transform.position, enemySpeed * Time.deltaTime);
            if (Vector3.Distance(gameObject.transform.position, deathWhirl.transform.position) <= 0)
            {
                GameObject eff = gameObject.transform.GetChild(2).transform.gameObject;
                eff.transform.parent = null;
                Destroy(gameObject);
                Destroy(eff, 1.5f);
                Destroy(deathWhirl, 1.5f);
            }
            //transform.position += Vector3.down * enemySpeed * Time.deltaTime;
        }
        //Restrict Enemy to the gamespace
        newX = Mathf.Clamp(transform.position.x, xmin, xmax);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }
    IEnumerator Down()
    {
        while (transform.position.y >= 35)
        {
            transform.position += Vector3.down * enemySpeed * Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator Up()
    {
        while (transform.position.y <= 55.5f)
        {
            transform.position += Vector3.up * enemySpeed * Time.deltaTime;
            yield return null;
        }
    }
    public void Projectiles()
    {
        //audioSource.Stop();
        cloneProjectile = Instantiate(enemyProjectile, enemyGunBarrel.transform.position, Quaternion.identity);
    }

    public void Whirl()
    {
        //audioSource.Play();
        cloneWhirl = Instantiate(whirl,enemyGunBarrel.transform);
    }


    IEnumerator Shrinking()
    {
        ParticleSystem m_System = GetComponentInChildren<ParticleSystem>();
        ParticleSystem.MainModule main = m_System.main;
        main.startSize = 10;
        while (enemyShootingFrequency > 0)
        {
            enemyShootingFrequency -= Time.deltaTime;
            main.startSize = (main.startSize.constant*enemyShootingFrequency)/ 3;
            yield return null;
        }
    }
    private void CheckEnemyDamageSprite()
    {
        if (areAllPartsDestoyed == false)
        {
            if (enemyHealth < (enemyTypes.health / 4) * 1)
            {
                //Loseback
                if (gameObject.name == "Hard" || gameObject.name == "Boss")
                {
                    enemySpeed = enemySpeed * 1.5f;
                }
                enemyParts[2].transform.parent = null;
                enemyParts[2].GetComponent<PartsSeparator>().enabled = true;
                Instantiate(damagedPartEffect, enemyParts[2].transform);
                Destroy(enemyParts[2], 5);
                if (enemyParts[1] != null)
                {
                    enemyParts[1].transform.parent = null;
                    enemyParts[1].GetComponent<PartsSeparator>().enabled = true;
                    Instantiate(damagedPartEffect, enemyParts[1].transform);
                    Destroy(enemyParts[1], 5);
                    if (enemyParts[0] != null)
                    {
                        enemyParts[0].transform.parent = null;
                        enemyParts[0].GetComponent<PartsSeparator>().enabled = true;
                        Instantiate(damagedPartEffect, enemyParts[0].transform);
                        Destroy(enemyParts[0], 5);
                    }
                }
                else if (enemyParts[0] != null)
                {
                    enemyParts[0].transform.parent = null;
                    enemyParts[0].GetComponent<PartsSeparator>().enabled = true;
                    Instantiate(damagedPartEffect, enemyParts[0].transform);
                    Destroy(enemyParts[0], 5);
                }
                areAllPartsDestoyed = true;
            }
            else if (enemyHealth < (enemyTypes.health / 4) * 2)
            {
                //LoseRight
                if (gameObject.name == "Hard" || gameObject.name == "Boss")
                {
                    enemySpeed = enemySpeed * 1.25f;
                }
                enemyParts[1].transform.parent = null;
                enemyParts[1].GetComponent<PartsSeparator>().enabled = true;
                Instantiate(damagedPartEffect, enemyParts[1].transform);
                Destroy(enemyParts[1], 5);
                if (enemyParts[0] != null)
                {
                    enemyParts[0].transform.parent = null;
                    enemyParts[0].GetComponent<PartsSeparator>().enabled = true;
                    Instantiate(damagedPartEffect, enemyParts[0].transform);
                    Destroy(enemyParts[0], 5);
                }
            }
            else if (enemyHealth < (enemyTypes.health / 4) * 3)
            {
                //Lose Left
                if (enemyParts[0] != null)
                {
                    enemyParts[0].transform.parent = null;
                    enemyParts[0].GetComponent<PartsSeparator>().enabled = true;
                    Instantiate(damagedPartEffect, enemyParts[0].transform);
                    Destroy(enemyParts[0], 5);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "PlayerProjectileTag")// If player projectile hits enemy
        {
            gameManager.ScoreChange(powerUp.damage);
            Destroy(other.gameObject);
            enemyHealth -= powerUp.damage;
            CheckEnemyDamageSprite();
            CheckEnemysHealth();
            if (player.GetComponent<Player>().extras[0].activeSelf == true)
            {
                if (powerUp.damage >= 85 && gameManager.stage >= 3 && gameManager.stage < 9)
                {
                    BuddyScript bs = GameObject.FindGameObjectWithTag("BuddyTag").GetComponent<BuddyScript>();
                    bs.readyToStick = true;
                }
                else if (powerUp.damage >= 75 && gameManager.stage >= 9)
                {
                    BuddyScript bs = GameObject.FindGameObjectWithTag("BuddyTag").GetComponent<BuddyScript>();
                    bs.readyToStick = true;
                }
            }
        }
        else if (other.tag == "FrozenProjectileTag")
        {
            gameManager.ScoreChange(powerUp.damage);
            Destroy(other.gameObject);
            enemyHealth -= powerUp.damage;
            CheckEnemyDamageSprite();
            CheckEnemysHealth();
        }
        else if (other.tag == "BuddyTag")
        {
            gameManager.ScoreChange(powerUp.damage);
            BuddyScript bs = GameObject.FindGameObjectWithTag("BuddyTag").GetComponent<BuddyScript>();
            bs.readyToStick = false;
            other.transform.localPosition = new Vector3(20, 0, 0);
            enemyHealth -= (powerUp.damage / 2);
            CheckEnemyDamageSprite();
            CheckEnemysHealth();
        }
    }
    
    private void CheckEnemysHealth()
    {
        if (enemyHealth <= 0)
        {
            Destroy(gameObject, 8);
            deathWhirl = Instantiate(gameManager.whirlOfDoom, gameManager.whirlOfDoom.transform.position, Quaternion.identity, null);
            explodedEnemy = Instantiate(enemyExplosion, gameObject.transform);
            Destroy(deathWhirl, 8);
            CancelInvoke("Projectiles");
            CancelInvoke("Whirl");
            isMovingLeft = false;
            isMovinRight = false;
            enemySpeed = 100;
        }
    }
}
