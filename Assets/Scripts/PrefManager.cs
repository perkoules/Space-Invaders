using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class PrefManager : MonoBehaviour
{
    public Texture2D pointerTexture;
    public AudioClip[] clips;
    private AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 0:
                audioSource.clip = clips[0];
                break;
            case 2:
                audioSource.clip = clips[1];
                break;
            case 3:
                audioSource.clip = clips[2];
                break;
            default:
                break;
        }
        audioSource.Play();
        if(SceneManager.GetActiveScene().buildIndex != 1)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Cursor.SetCursor(pointerTexture, Vector2.zero, CursorMode.Auto);
        }
    }
    public void LoadScene(int sceneNumber)
    {
        if (sceneNumber >= 0)
        {
            SceneManager.LoadSceneAsync(sceneNumber);
        }
        else
        {
            Application.Quit();
        }
    }
}