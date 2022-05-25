using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Mute : MonoBehaviour
{
    public float originalVolume = 0;
    public bool muted = false;

    public Button muteButton;

    void Start()
    {
        mute();
    }

    public void onClick(){
        if(muted){
            unMute();
            muted = false;
            muteButton.GetComponentInChildren<TMP_Text>().text = "Mute";

        }
        else{
            mute();
            muted = true;
            muteButton.GetComponentInChildren<TMP_Text>().text = "Unmute";
        }
    }

    void mute(){
        originalVolume = AudioListener.volume;
        AudioListener.volume = 0;
    }

    public void unMute(){
        AudioListener.volume = originalVolume;
    }
}
