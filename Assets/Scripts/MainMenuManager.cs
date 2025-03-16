using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
   
    public AudioManager audioManager;


    public void PlayStartButtonHoverAudio()
    {
        audioManager.Play("ButtonHover");
    }

    public void PlayStartButtonClickedAudio()
    {
        audioManager.Play("ButtonPressed");
    }
}
