using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterObject : MonoBehaviour
{
    public GameObject player;
    public GameObject gameManager;

    void Start()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        GameObject playerClone = Instantiate(player, player.transform.position, player.transform.rotation, null);
        yield return new WaitForSeconds(1);
        GameObject gameManagerClone = Instantiate(gameManager, player.transform.position, player.transform.rotation, null);
    }
}
