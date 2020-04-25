using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eraser : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag != "BuddyTag")
        {
            Destroy(other.gameObject);
        }
    }

}
