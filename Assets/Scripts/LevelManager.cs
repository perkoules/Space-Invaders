using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class LevelManager : MonoBehaviour
{
    public TextMeshProUGUI scoreDisplayText;
    void Start()
    {
        scoreDisplayText.text = PlayerPrefs.GetInt("scorePref").ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
