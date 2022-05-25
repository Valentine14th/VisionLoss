using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Pause : MonoBehaviour
{
    public float originalTimeScale = 0;
    public bool paused = false;

    public Button pauseButton;

    void Start(){
        //pause();
        Debug.Log("paused game at start");
    }
    public void onClick(){
        if(paused){
            resume();
            pauseButton.GetComponentInChildren<TMP_Text>().text = "Pause";
        }
        else{
            pause();
            pauseButton.GetComponentInChildren<TMP_Text>().text = "Resume";
        }
    }

    void pause(){
        originalTimeScale = Time.timeScale;
        Time.timeScale = 0;
        paused = true;
    }

    void resume(){
        Time.timeScale = originalTimeScale;
        paused = false;
    }
}
