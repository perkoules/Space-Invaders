using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public GameObject ammoExistsEffect, deathExplosion, partsExplosion, getHitEffect, frozenProjectileVaultEffect, changeArmorEffect;
    public GameObject[] playerParts, frozenProjectiles, extras;
    public Sprite [] secondSkin, thirdSkin, fourthSkin;
    public List<GameObject> projectilesToMove;
    public Material projectileMaterial, partsMaterial;
    public float speed = 100f;
    public int timesTriggered = 0, ammoEarned = 0;
    public bool areAllPartsDestoyed = false, now = false, sorted = false, absorbSkin = false, canShoot = false;

    private GameObject playersBarrel;
    private GameManager gameManager;
    private LaserScript laserScript;
    private PowerUp powerUp;
    private float xmin, xmax/*, distance, distanceAbs*/;
    //private int absorbedPower;
    private bool initializingFinished = false;
    private void Start()
    {
        playersBarrel = GameObject.FindGameObjectWithTag("PlayerGunBarrelTag");
        float distance = transform.position.z - Camera.main.transform.position.z;
        Vector3 leftmost = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance));
        Vector3 rightmost = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance));
        xmin = leftmost.x + 55;
        xmax = rightmost.x - 5;
        StartCoroutine(Initializing());
    }
    IEnumerator Initializing()
    {
        yield return new WaitForSeconds(2f);
        powerUp = GameObject.FindGameObjectWithTag("IndicatorTag").GetComponent<PowerUp>();
        gameManager = GameObject.FindGameObjectWithTag("GameManagerTag").GetComponent<GameManager>();
        //spriteRenderer = GetComponent<SpriteRenderer>();
        laserScript = GetComponentInChildren<LaserScript>();
        initializingFinished = true;
    }

    void Update()
    {
        if (initializingFinished && gameManager.initializingCompleted)
        {
            if (gameObject != null)
            {
                if (Input.GetButton("Horizontal"))
                {
                    transform.position += Input.GetAxisRaw("Horizontal") * Vector3.right * speed * Time.deltaTime;
                }
                float newX = Mathf.Clamp(transform.position.x, xmin, xmax);
                transform.position = new Vector3(newX, transform.position.y, transform.position.z);
                ammoExistsEffect.SetActive(powerUp.doIHaveAmmo);
                if (powerUp.doIHaveAmmo)
                {
                    gameManager.keysUI[0].color = new Color(0.5f, 1, 0.5f);
                    gameManager.keysUI[1].color = new Color(0.5f, 1, 0.5f);
                }
                else
                {
                    gameManager.keysUI[0].color = Color.white;
                    gameManager.keysUI[1].color = Color.white;
                }
                if (timesTriggered == 2)
                {
                    if (sorted == false)
                    {
                        if(Vector3.Distance(projectilesToMove[0].transform.position, playersBarrel.transform.position) == 0 &&
                            Vector3.Distance(projectilesToMove[1].transform.position, playersBarrel.transform.position) == 0)
                        {
                            sorted = true;
                        }
                        for (int i = 0; i < 2; i++)
                        {
                            projectilesToMove[i].transform.position = Vector2.MoveTowards(projectilesToMove[i].transform.position,playersBarrel.transform.position, 0.2f);
                        }
                    }
                    else if (sorted == true && (Input.GetAxis("Vertical") > 0 || Input.GetButtonDown("GunControl")))
                    {
                        StartCoroutine(ShootFrozenProjectiles());
                    }
                }
            }
        }
    }
    IEnumerator ShootFrozenProjectiles()
    {
        timesTriggered = 0;
        sorted = false;
        GameObject frozProj = Instantiate(frozenProjectiles[4], playersBarrel.transform.position, Quaternion.identity, null);
        frozProj.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        foreach (var item in projectilesToMove)
        {
            Destroy(item);
        }
        projectilesToMove.Clear();
        GameObject frozProj2 = Instantiate(frozenProjectiles[4], playersBarrel.transform.position, Quaternion.identity, null);
        frozProj2.SetActive(true);
    }
    public void EnableFrozenProjectile(int howMany)
    {
        if (timesTriggered < 2)
        {
            timesTriggered += howMany;
            if (timesTriggered == 4)
            {
                projectilesToMove.Add(Instantiate(frozenProjectileVaultEffect, frozenProjectiles[3].transform.position, Quaternion.identity,
                    frozenProjectiles[3].transform.parent));
            }
            else if (timesTriggered == 3)
            {
                projectilesToMove.Add(Instantiate(frozenProjectileVaultEffect, frozenProjectiles[2].transform.position, Quaternion.identity,
                    frozenProjectiles[2].transform.parent));
            }
            else if (timesTriggered == 2)
            {
                projectilesToMove.Add(Instantiate(frozenProjectileVaultEffect, frozenProjectiles[1].transform.position, Quaternion.identity,
                    frozenProjectiles[1].transform.parent));
            }
            else if (timesTriggered == 1)
            {
                projectilesToMove.Add(
                    Instantiate(frozenProjectileVaultEffect,
                    frozenProjectiles[0].transform.position,
                    Quaternion.identity,
                    frozenProjectiles[0].transform.parent));
            }
        }
    }
    void ChangeArmor(string whichArmor)
    {
        GameObject fx = Instantiate(changeArmorEffect, transform);
        Destroy(fx, 0.5f);
        if (whichArmor == "Second")
        {
            //front
            playerParts[3].transform.localPosition = new Vector3(-4.09f, 3.16f, 0);
            playerParts[3].GetComponent<SpriteRenderer>().sprite = secondSkin[0];
            //back
            playerParts[2].transform.localPosition = new Vector3(4.09f, 3.16f, 0);
            playerParts[2].GetComponent<SpriteRenderer>().sprite = secondSkin[1];
            //left
            playerParts[1].transform.localPosition = new Vector3(0.26f, -2.56f, 0);
            playerParts[1].GetComponent<SpriteRenderer>().sprite = secondSkin[2];
            //right
            playerParts[0].transform.localPosition = new Vector3(0, 0, 0);
            playerParts[0].transform.localScale = new Vector3(1.2f, 1.5f, 1);
            playerParts[0].GetComponent<SpriteRenderer>().sprite = secondSkin[3];
        }
        else if (whichArmor == "Third")
        {
            playersBarrel.transform.localPosition = new Vector3(0, -10, 0);
            playerParts[3].transform.localPosition = new Vector3(-4.04f, 1.41f, 0);
            playerParts[3].GetComponent<SpriteRenderer>().sprite = thirdSkin[0];
            playerParts[2].transform.localPosition = new Vector3(4.57f, 1.41f, 0);
            playerParts[2].GetComponent<SpriteRenderer>().sprite = thirdSkin[1];
            playerParts[1].transform.localPosition = new Vector3(0.7f, -4.13f, 0);
            playerParts[1].GetComponent<SpriteRenderer>().sprite = thirdSkin[2];
            playerParts[0].transform.localPosition = new Vector3(0, 0, 0);
            playerParts[0].GetComponent<SpriteRenderer>().sprite = thirdSkin[3];
        }
        else if (whichArmor == "Fourth")
        {
            playersBarrel.transform.localPosition = new Vector3(0, -12, 0);
            playerParts[3].transform.localPosition = new Vector3(-6.6f, -0.61f, 0);
            playerParts[3].GetComponent<SpriteRenderer>().sprite = fourthSkin[0];
            playerParts[2].transform.localPosition = new Vector3(6.69f, -0.61f, 0);
            playerParts[2].GetComponent<SpriteRenderer>().sprite = fourthSkin[1];
            playerParts[1].transform.localPosition = new Vector3(0f, -7.19f, 0);
            playerParts[1].GetComponent<SpriteRenderer>().sprite = fourthSkin[2];
            playerParts[0].transform.localPosition = new Vector3(0, 0, 0);
            playerParts[0].GetComponent<SpriteRenderer>().sprite = fourthSkin[3];
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        LineRenderer lr = GetComponentInChildren<LineRenderer>();
        LaserScript ls = GetComponentInChildren<LaserScript>();
        if (other.tag == "EnemyProjectileTag" && powerUp.doIHaveAmmo == false)
        {
            gameManager.keysUI[0].color = Color.green;
            gameManager.keysUI[2].color = Color.green;
            lr.startColor = Color.red;
            lr.endColor = Color.red;
            ls.endLaserPosition = other.transform;
        }
        else if (other.tag == "DamagedPartsTag" && other.GetComponentInChildren<PartsSeparator>().enabled == true)
        {
            gameManager.keysUI[0].color = Color.green;
            gameManager.keysUI[2].color = Color.green;
            lr.startColor = Color.green;
            lr.endColor = Color.green;
            ls.endLaserPosition = other.transform;
        }
        else if (other.tag == "BonusTag")
        {
            gameManager.keysUI[0].color = Color.green;
            gameManager.keysUI[2].color = Color.green;
            lr.startColor = Color.green;
            lr.endColor = Color.green;
            ls.endLaserPosition = other.transform;
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        LineRenderer lr = GetComponentInChildren<LineRenderer>();
        LaserScript ls = GetComponentInChildren<LaserScript>();
        if (other.tag == "EnemyProjectileTag" && powerUp.doIHaveAmmo == false)
        {
            gameManager.keysUI[0].color = Color.green;
            gameManager.keysUI[2].color = Color.green;
            lr.startColor = Color.red;
            lr.endColor = Color.red;
            ls.endLaserPosition = other.transform;
        }
        else if (other.tag == "DamagedPartsTag" && other.GetComponentInChildren<PartsSeparator>().enabled == true)
        {
            gameManager.keysUI[0].color = Color.green;
            gameManager.keysUI[2].color = Color.green;
            lr.startColor = Color.green;
            lr.endColor = Color.green;
            ls.endLaserPosition = other.transform;
            if ((Input.GetButtonDown("GunControl") || Input.GetAxis("Vertical") < 0) /*&& powerUp.doIHaveAmmo == false*/)
            {
                GameManager gameManager = GameObject.FindGameObjectWithTag("GameManagerTag").GetComponent<GameManager>();
                gameManager.ScoreChange(20);
                Destroy(other.gameObject);
            }
        }
        else if (other.tag == "BonusTag")
        {
            gameManager.keysUI[0].color = Color.green;
            gameManager.keysUI[2].color = Color.green;
            lr.startColor = Color.green;
            lr.endColor = Color.green;
            ls.endLaserPosition = other.transform;
            if ((Input.GetButtonDown("GunControl") || Input.GetAxis("Vertical") < 0))
            {
                if (other.gameObject.name.Contains("Clock"))
                {
                    Clock cl = FindObjectOfType<Clock>();
                    cl.timePaused = true;
                    cl.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                    ls.endLaserPosition = ls.startLaserPosition;
                }
                if (other.gameObject.name.Contains("Second") && absorbSkin == true)
                {
                    absorbSkin = false;
                    ChangeArmor("Second");
                    GameManager gameManager = GameObject.FindGameObjectWithTag("GameManagerTag").GetComponent<GameManager>();
                    gameManager.secondArmorCollected = true;
                    extras[0].SetActive(true);
                    Destroy(other.gameObject);
                }
                else if (other.gameObject.name.Contains("Third") && absorbSkin == true)
                {
                    absorbSkin = false;
                    ChangeArmor("Third");
                    GameManager gameManager = GameObject.FindGameObjectWithTag("GameManagerTag").GetComponent<GameManager>();
                    gameManager.thirdArmorCollected = true;
                    if(extras[0].activeSelf == false)
                    {
                        extras[0].SetActive(true);
                    }
                    Destroy(other.gameObject);
                }
                else if (other.gameObject.name.Contains("Fourth") && absorbSkin == true)
                {
                    absorbSkin = false;
                    ChangeArmor("Fourth");
                    GameManager gameManager = GameObject.FindGameObjectWithTag("GameManagerTag").GetComponent<GameManager>();
                    gameManager.fourthArmorCollected = true;
                    if (extras[0].activeSelf == false)
                    {
                        extras[0].SetActive(true);
                    }
                    Destroy(other.gameObject);
                }
            }
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        gameManager.keysUI[0].color = Color.white;
        gameManager.keysUI[2].color = Color.white;
        LineRenderer lr = GetComponentInChildren<LineRenderer>();
        LaserScript ls = GetComponentInChildren<LaserScript>();
        if (other.tag == "EnemyProjectileTag" && powerUp.doIHaveAmmo == false)
        {
            //gameManager.z.color = Color.white;
            lr.startColor = Color.red;
            lr.endColor = Color.red;
            ls.endLaserPosition = ls.startLaserPosition;
        }
        else if (other.tag == "DamagedPartsTag" && other.GetComponentInChildren<PartsSeparator>().enabled == true)
        {
            //gameManager.z.color = Color.white;
            lr.startColor = Color.green;
            lr.endColor = Color.green;
            ls.endLaserPosition = ls.startLaserPosition;
        }
        else if (other.tag == "BonusTag")
        {
            lr.startColor = Color.green;
            lr.endColor = Color.green;
            ls.endLaserPosition = ls.startLaserPosition;
        }
    }
}
