using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using UnityEngine.UI;
public class Timer : MonoBehaviour
{
    public float timeRemaining = 60; // Tiempo en segundos
    public bool timerIsRunning = false;
    public TMP_Text timerText; // Referencia al texto UI que muestra el tiempo
    public WaraPC characterController;

    private void Start()
    {
        timerIsRunning = true;
    }

    private void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
                DisplayTime(timeRemaining);
                characterController.Dead(DeathState.Time);
            }
        }
    }

    private void DisplayTime(float timeToDisplay)
    {
        // Muestra el tiempo restante en formato MM:SS
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
