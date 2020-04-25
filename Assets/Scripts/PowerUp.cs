using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PowerUp : MonoBehaviour
{
    public AudioClip goodLandingSound;
    public GameObject [] playerProjectiles;
    public SpriteRenderer orange;
    public Transform objectToRotate, rotateAroundThisPoint;
    public float speed;
    public int absorbedPower = 0, damage = 0, playersHealth;
    public bool landedOnRed = false, startSpinning = false, doIHaveAmmo = false;


    private GameObject player, playerGunBarrel;
    private Transform objectToRotateInPlayer, rotateAroundThisPointInPlayer;
    private Player playerScript;
    private GameManager gameManager;
    public AudioSource audioSource;
    public bool initializingFinished = false;

    void Start()
    {
        StartCoroutine(Initializing());
        audioSource = GetComponent<AudioSource>();
    }

    IEnumerator Initializing()
    {
        yield return new WaitForSeconds(2);
        gameManager = GameObject.FindGameObjectWithTag("GameManagerTag").GetComponent<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<Player>();
        playerGunBarrel = GameObject.FindGameObjectWithTag("PlayerGunBarrelTag");
        objectToRotateInPlayer = GameObject.FindGameObjectWithTag("ObjectToRotateInPlayerTag").transform;
        rotateAroundThisPointInPlayer = GameObject.FindGameObjectWithTag("PivotPointCenterTag").transform;
        initializingFinished = true;
    }

    void FixedUpdate()
    {
        if (player != null)
        {
            //Indicator starts spinning
            if (startSpinning == true && initializingFinished == true)
            {
                objectToRotate.RotateAround(rotateAroundThisPoint.transform.position, Vector3.forward, 1);
                objectToRotate.Rotate(Vector3.forward * speed);
                objectToRotateInPlayer.RotateAround(rotateAroundThisPointInPlayer.transform.position, Vector3.forward, 1);
                objectToRotateInPlayer.Rotate(Vector3.forward * speed);
                landedOnRed = false;
            }
            if (doIHaveAmmo == true)
            {
                //gameManager.z.color = Color.white;
                //gameManager.c.color = Color.green;
            }
            else if (doIHaveAmmo == false)
            {
                //gameManager.c.color = Color.white;
            }
        }
    }
    void OnTriggerStay2D (Collider2D other) //Shooting Absorbed
    {
        if ((Input.GetButtonDown("GunControl") || Input.GetAxis("Vertical") > 0) && doIHaveAmmo == true)
        {
            Color colorCollided = other.GetComponent<Image>().color;
            if (colorCollided == Color.green)
            {
                audioSource.PlayOneShot(goodLandingSound);
                damage = absorbedPower * 2;
                Instantiate(playerProjectiles[1], playerGunBarrel.transform.position, Quaternion.identity);
                absorbedPower = 0;
                doIHaveAmmo = false;
                startSpinning = false;
            }
            else if (colorCollided == Color.gray)
            {
                damage = absorbedPower / 2;
                GameObject grayClone = Instantiate(playerProjectiles[0], playerGunBarrel.transform.position, Quaternion.identity) as GameObject;
                GameObject grayClone2 = Instantiate(playerProjectiles[0], playerGunBarrel.transform.position, Quaternion.identity) as GameObject;
                absorbedPower = 0;
                doIHaveAmmo = false;
                startSpinning = false;
            }
            else if (colorCollided == Color.red)
            {
                if (landedOnRed == false)
                {
                    landedOnRed = true;
                    absorbedPower = 0;
                    doIHaveAmmo = false;
                    startSpinning = false;
                    StartCoroutine(gameManager.PlayerGotHit());
                }
            }
            else if (colorCollided == orange.GetComponent<SpriteRenderer>().color)
            {
                damage = absorbedPower * 1;
                Instantiate(playerProjectiles[2], playerGunBarrel.transform.position, Quaternion.identity);//Shoot orange projectile
                absorbedPower = 0;
                doIHaveAmmo = false;
                startSpinning = false;
            }
        }
    }
}
