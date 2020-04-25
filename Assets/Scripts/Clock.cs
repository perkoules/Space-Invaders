using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    public bool timePaused = false;
    private bool enterred = false;
    private GameManager gameManager;
    private AudioSource audioSource;
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManagerTag").GetComponent<GameManager>();
        audioSource = gameManager.GetComponent<AudioSource>();
        GetComponent<Rigidbody2D>().velocity = Vector3.down * 50;
    }

    // Update is called once per frame
    void Update()
    {
        if (timePaused && enterred == false)
        {
            AudioSource[] suorces = FindObjectsOfType<AudioSource>();
            foreach (var item in suorces)
            {
                item.Pause();
            }
            gameObject.GetComponent<AudioSource>().UnPause();
            enterred = true;
            StartCoroutine(PauseTime());
        }
    }

    IEnumerator PauseTime()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (var en in enemies)
        {
            en.enemySpeed = 0;
        }
        if(gameManager.stage < 6)
        {
            yield return new WaitForSeconds(5);
        }
        else
        {
            yield return new WaitForSeconds(9);
        }
        
        foreach (var enn in enemies)
        {
            enn.enemySpeed = enn.enemyTypes.enemySpeed;
        }
        AudioSource[] suorces = FindObjectsOfType<AudioSource>();
        foreach (var item in suorces)
        {
            item.UnPause();
        }
        //audioSource.UnPause();
        timePaused = false;
        enterred = false;
        Destroy(gameObject);
        yield return null;
    }

}
