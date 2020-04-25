using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartsSeparator : MonoBehaviour
{
    //private bool scoreGained = false;
    private GameObject player;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void Update()
    {
        transform.position += Vector3.down * 40 * Time.deltaTime;
        /*if (Input.GetKeyDown(KeyCode.Space) && Mathf.Abs(gameObject.transform.position.x - player.transform.position.x) < 8.3f && scoreGained == false)
        {
            scoreGained = true;
            GameManager gameManager = GameObject.FindGameObjectWithTag("GameManagerTag").GetComponent<GameManager>();
            gameManager.ScoreChange(20);
            Destroy(gameObject);
        }*/
    }
}
