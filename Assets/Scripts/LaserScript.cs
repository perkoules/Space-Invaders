using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class LaserScript : MonoBehaviour
{
    public Transform startLaserPosition, endLaserPosition;
    public float widthstart = 5, widthEnd = 5;

    private LineRenderer laserLine;
    private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        laserLine = GetComponent<LineRenderer>();
        laserLine.startWidth = widthstart;
        laserLine.endWidth = widthEnd;
        endLaserPosition = startLaserPosition;
    }

    void Update()
    {
        if (endLaserPosition == null)
        {
            audioSource.Stop();
            endLaserPosition = startLaserPosition;
        }

        if(audioSource.isPlaying == false && endLaserPosition != startLaserPosition)
        {
            audioSource.Play();
        }
        else if(endLaserPosition == startLaserPosition)
        {
            audioSource.Stop();
        }


        laserLine.SetPosition(0, startLaserPosition.position);
        laserLine.SetPosition(1, endLaserPosition.position);
    }
    
}
