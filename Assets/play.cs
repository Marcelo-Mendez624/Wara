using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    public TransitionManager transitionManager;

    public void OnPlayButtonClicked()
    {
        transitionManager.LoadScene("GameScene");
    }
}
