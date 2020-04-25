using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class GameManager : MonoBehaviour
{

    public AudioClip backgroundSoundForBoss, gameOverSound, spinningWheelSound, gotHitSound, wheelRotationSound;
    public GameObject[] enemies;
    public GameObject gainLifeEffect, whirlOfDoom, losingLifeEffect, bonusRocket;
    public Image[] lives, keysUI;
    public Material temporaryBackground, backgroundForBoss;
    public int score = 0, scoreNew = 0, playersHealth, health = 65, stage = 1;
    public bool secondArmorCollected = false, thirdArmorCollected = false, fourthArmorCollected = false, initializingCompleted = false;

    private AudioSource audioSource;
    private GameObject player;
    private Player playerScript;
    private Skybox backgroundSkybox;
    private TextMeshProUGUI scoreDisplayText, wavesText;
    private bool readyForLosingScene = false;
    private int playerLives = -1, howManyEnemies = 0;
    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        PlayerPrefs.SetInt("scorePref", score);
        StartCoroutine(Initializing());
    }

    IEnumerator Initializing()
    {
        yield return new WaitForSeconds(1.5f);
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<Player>();
        audioSource = GetComponent<AudioSource>();
        backgroundSkybox = Camera.main.GetComponent<Skybox>();
        scoreDisplayText = GameObject.FindGameObjectWithTag("ScoreTextTag").GetComponent<TextMeshProUGUI>();
        wavesText = GameObject.FindGameObjectWithTag("CanvasTag").transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        for (int i = 0; i < 6; i++)
        {
            lives[i] = GameObject.FindGameObjectWithTag("CanvasLivesTag").transform.GetChild(i).GetComponent<Image>();
        }
        for (int i = 0; i < 5; i++)
        {
            keysUI[i] = GameObject.FindGameObjectWithTag("KeysUITag").transform.GetChild(i).GetComponent<Image>();
        }
        initializingCompleted = true;
        StartCoroutine(Spawn(stage));
        InvokeRepeating("CheckIfAllDead", 5, 2);
        InvokeRepeating("SpawnBonusRocket", 15, 15);
        InvokeRepeating("SpawnClock", 60, 60);
    }
    void SpawnClock()
    {
        if (stage >= 6)
        {
            Instantiate(playerScript.extras[1], new Vector3(Random.Range(-50, 50), playerScript.extras[1].transform.position.y, playerScript.extras[1].transform.position.z),
                Quaternion.identity, null);
        }
    }
    void SpawnBonusRocket()
    {
        Vector3 rightmost = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 10));
        GameObject clone = Instantiate(bonusRocket, rightmost - new Vector3(-50, 15, 0), bonusRocket.transform.rotation);

    }
    public IEnumerator PlayerGotHit()
    {
        StartCoroutine(GotHitEffect());
        PlayersHealthChange(1);
        playersHealth = health;
        if (playersHealth <= 0)
        {
            while (readyForLosingScene == false)
            {
                
            }
            readyForLosingScene = false;
            lives[playersHealth].GetComponent<Image>().color = Color.black;
            var loseLifeFX = Instantiate(losingLifeEffect, player.transform.position, Quaternion.identity) as GameObject;
            Destroy(loseLifeFX, 0.75f);
            CheckPlayerSprite();
            Instantiate(playerScript.deathExplosion, player.transform);
            audioSource.Stop();
            audioSource.loop = false;
            audioSource.clip = gameOverSound;
            Destroy(player, 0.5f);
            StartCoroutine(LoseScene());
        }
        else
        {
            lives[playersHealth].GetComponent<Image>().color = Color.black;
            var loseLifeFX = Instantiate(losingLifeEffect, player.transform.position, Quaternion.identity) as GameObject;
            Destroy(loseLifeFX, 0.75f);
            CheckPlayerSprite();
        }
        yield return null;
    }

    IEnumerator LoseScene()
    {
        yield return new WaitForSeconds(0.5f);
        audioSource.volume = 1f;
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length + 0.5f);
        SceneManager.LoadSceneAsync(2);
    }
    public IEnumerator GotHitEffect()
    {
        if (backgroundSkybox.material == backgroundForBoss)
        {
            float start = 0, end = 2;
            while (start < end)
            {
                backgroundSkybox.material = temporaryBackground;
                yield return new WaitForSeconds(0.1f);
                audioSource.PlayOneShot(gotHitSound);
                backgroundSkybox.material = backgroundForBoss;
                yield return new WaitForSeconds(0.1f);
                start += 0.5f;
                yield return null;
            }
        }
        else
        {
            float start = 0, end = 2;
            while (start < end)
            {
                backgroundSkybox.material = temporaryBackground;
                yield return new WaitForSeconds(0.1f);
                audioSource.PlayOneShot(gotHitSound);
                backgroundSkybox.material = null;
                yield return new WaitForSeconds(0.1f);
                start += 0.5f;
                yield return null;
            }
        }
        if (readyForLosingScene == false)
        {

            readyForLosingScene = true;
        }
    }
    public void PlayersHealthChange(int healthNew)
    {
        health -= healthNew;
    }
    public void CheckPlayerSprite()
    {
        if (playerScript.areAllPartsDestoyed == false)
        {
            for (int i = 0; i < 4; i++)
            {
                if (lives[i].GetComponent<Image>().color == Color.black)
                {
                    playerScript.playerParts[i].SetActive(false);
                }
                else if (lives[i].GetComponent<Image>().color == Color.white)
                {
                    playerScript.playerParts[i].SetActive(true);
                }
            }
        }
    }
    public void ScoreChange(int scoreNew)
    {
        score += scoreNew;
        scoreDisplayText.text = score.ToString();
        PlayerPrefs.SetInt("scorePref", score);
    }

    IEnumerator Spawn(int thisStage)
    {
        if (thisStage == 12)
        {
            wavesText.gameObject.SetActive(true);
            wavesText.text = "Boss";
        }
        else if (thisStage < 12)
        {
            wavesText.gameObject.SetActive(true);
            wavesText.text = "Wave " + thisStage;
        }
        yield return new WaitForSeconds(1);
        wavesText.gameObject.SetActive(false);
        switch (thisStage)
        {
            case 1:
                Instantiate(enemies[0], new Vector3(0, enemies[0].transform.position.y, 0), Quaternion.identity, gameObject.transform);
                break;
            case 2:
                Instantiate(enemies[0], new Vector3(0, enemies[0].transform.position.y, 0), Quaternion.identity, gameObject.transform);
                yield return new WaitForSeconds(0.5f);
                Instantiate(enemies[0], new Vector3(0, enemies[0].transform.position.y, 0), Quaternion.identity, gameObject.transform);
                break;
            case 3:
                Instantiate(enemies[0], new Vector3(0, enemies[0].transform.position.y, 0), Quaternion.identity, gameObject.transform);
                yield return new WaitForSeconds(0.5f);
                Instantiate(enemies[0], new Vector3(0, enemies[0].transform.position.y, 0), Quaternion.identity, gameObject.transform);
                yield return new WaitForSeconds(0.5f);
                Instantiate(enemies[0], new Vector3(0, enemies[0].transform.position.y, 0), Quaternion.identity, gameObject.transform);
                break;
            case 4:
                Instantiate(enemies[0], new Vector3(0, enemies[0].transform.position.y, 0), Quaternion.identity, gameObject.transform);
                yield return new WaitForSeconds(0.5f);
                Instantiate(enemies[0], new Vector3(0, enemies[0].transform.position.y, 0), Quaternion.identity, gameObject.transform);
                yield return new WaitForSeconds(0.5f);
                Instantiate(enemies[0], new Vector3(0, enemies[0].transform.position.y, 0), Quaternion.identity, gameObject.transform);
                yield return new WaitForSeconds(0.5f);
                Instantiate(enemies[0], new Vector3(0, enemies[0].transform.position.y, 0), Quaternion.identity, gameObject.transform);
                break;
            case 5:
                Instantiate(enemies[0], new Vector3(0, enemies[0].transform.position.y, 0), Quaternion.identity, gameObject.transform);
                yield return new WaitForSeconds(0.5f);
                Instantiate(enemies[1], new Vector3(0, enemies[1].transform.position.y, 0), Quaternion.identity, gameObject.transform);
                yield return new WaitForSeconds(0.5f);
                Instantiate(enemies[0], new Vector3(0, enemies[0].transform.position.y, 0), Quaternion.identity, gameObject.transform);
                yield return new WaitForSeconds(0.5f);
                Instantiate(enemies[0], new Vector3(0, enemies[0].transform.position.y, 0), Quaternion.identity, gameObject.transform);
                break;
            case 6:
                Instantiate(enemies[0], new Vector3(0, enemies[0].transform.position.y, 0), Quaternion.identity, gameObject.transform);
                yield return new WaitForSeconds(0.5f);
                Instantiate(enemies[1], new Vector3(0, enemies[1].transform.position.y, 0), Quaternion.identity, gameObject.transform);
                yield return new WaitForSeconds(0.5f);
                Instantiate(enemies[1], new Vector3(0, enemies[1].transform.position.y, 0), Quaternion.identity, gameObject.transform);
                break;
            case 7:
                Instantiate(enemies[1], new Vector3(0, enemies[1].transform.position.y, 0), Quaternion.identity, gameObject.transform);
                yield return new WaitForSeconds(0.5f);
                Instantiate(enemies[1], new Vector3(0, enemies[1].transform.position.y, 0), Quaternion.identity, gameObject.transform);
                yield return new WaitForSeconds(0.5f);
                Instantiate(enemies[2], new Vector3(0, enemies[2].transform.position.y, 0), Quaternion.identity, gameObject.transform);
                break;
            case 8:
                Instantiate(enemies[2], new Vector3(0, enemies[2].transform.position.y, 0), Quaternion.identity, gameObject.transform);
                yield return new WaitForSeconds(0.5f);
                Instantiate(enemies[2], new Vector3(0, enemies[2].transform.position.y, 0), Quaternion.identity, gameObject.transform);
                yield return new WaitForSeconds(0.5f);
                Instantiate(enemies[1], new Vector3(0, enemies[1].transform.position.y, 0), Quaternion.identity, gameObject.transform);
                yield return new WaitForSeconds(0.5f);
                Instantiate(enemies[1], new Vector3(0, enemies[1].transform.position.y, 0), Quaternion.identity, gameObject.transform);
                break;
            case 9:
                Instantiate(enemies[2], new Vector3(0, enemies[2].transform.position.y, 0), Quaternion.identity, gameObject.transform);
                yield return new WaitForSeconds(0.5f);
                Instantiate(enemies[2], new Vector3(0, enemies[2].transform.position.y, 0), Quaternion.identity, gameObject.transform);
                yield return new WaitForSeconds(0.5f);
                Instantiate(enemies[3], new Vector3(0, enemies[3].transform.position.y, 0), Quaternion.identity, gameObject.transform);
                break;
            case 10:
                Instantiate(enemies[2], new Vector3(0, enemies[2].transform.position.y, 0), Quaternion.identity, gameObject.transform);
                yield return new WaitForSeconds(0.5f);
                Instantiate(enemies[2], new Vector3(0, enemies[2].transform.position.y, 0), Quaternion.identity, gameObject.transform);
                yield return new WaitForSeconds(0.5f);
                Instantiate(enemies[3], new Vector3(0, enemies[3].transform.position.y, 0), Quaternion.identity, gameObject.transform);
                break;
            case 11:
                Instantiate(enemies[3], new Vector3(0, enemies[3].transform.position.y, 0), Quaternion.identity, gameObject.transform);
                yield return new WaitForSeconds(0.5f);
                Instantiate(enemies[3], new Vector3(0, enemies[3].transform.position.y, 0), Quaternion.identity, gameObject.transform);
                yield return new WaitForSeconds(0.5f);
                Instantiate(enemies[3], new Vector3(0, enemies[3].transform.position.y, 0), Quaternion.identity, gameObject.transform);
                break;
            case 12:
                backgroundSkybox.material = backgroundForBoss;
                audioSource.Stop();
                audioSource.clip = backgroundSoundForBoss;
                audioSource.Play();
                audioSource.volume = 0.5f;
                player.transform.position = new Vector3(12.5f, -65, 0);
                gameObject.transform.position = Vector3.zero;
                Camera.main.orthographicSize = 85;
                GameObject bossClone = Instantiate(enemies[4], enemies[4].transform.position, enemies[4].transform.rotation, gameObject.transform);
                break;
            case 13:
                int tmpScore = Mathf.RoundToInt((600 - Time.timeSinceLevelLoad) * 5);
                ScoreChange(tmpScore);
                SceneManager.LoadSceneAsync(3);
                break;
            default:
                break;
        }
        howManyEnemies = gameObject.transform.childCount;
        yield return new WaitForSeconds(0.5f);
    }
    private void CheckIfAllDead()
    {
        howManyEnemies = gameObject.transform.childCount;
        if (howManyEnemies == 0)
        {
            GainLife();
            stage++;
            StartCoroutine(Spawn(stage));
        }
    }

    private void GainLife()
    {
        if (health < 6)
        {
            playerLives = health;
            PlayersHealthChange(-1);
            lives[playerLives].GetComponent<Image>().color = Color.white;
            CheckPlayerSprite();
            GameObject gainLifeFX = Instantiate(gainLifeEffect, new Vector3(12.5f, 0, 0), Quaternion.identity) as GameObject;
            Destroy(gainLifeFX, 0.8f);
        }
    }

}
