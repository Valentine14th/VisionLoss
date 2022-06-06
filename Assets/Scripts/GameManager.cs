using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{

    // ------- VARIABLES FOR CODE MANAGEMENT ------- //

    private int[,] codes;               // Code for each door
    private int[] player1EnteredCode;   // The code the first player currently entered
    private int[] player2EnteredCode;   // The code the second player currently entered
    private GameObject[] codeZones;     // the set of code zones
    private int codeLength;             // Length of the codes
    private int nbOfDoors;              // nb of doors and thus codes in the level
    private bool webGame;               // whether we are playing the webbased game
    public float waitTime;              // time between code display pulses
    private static Color[] colors = { Color.blue, Color.green, Color.red, Color.magenta, Color.yellow, Color.cyan, Color.white };

    private int startTime = 0;
    private int currentTime = 0;
    private bool isTimerRunning = false;
    public TMP_Text timerText;

    public GameObject winDisplay;


    // ------- END OF VARIABLES FOR CODE MANAGEMENT ------- //


    // Start is called before the first frame update
    void Start()
    {
        setCodeLength(3);
        generateCodes();
        nbOfDoors = codeZones.Length;
        Debug.Log("nb of doors is: " + nbOfDoors);
        webGame = true; // TODO: make it possible to choose
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isTimerRunning)
        {
            currentTime = (int)Time.time - startTime;
            
            string text = string.Format("{0:00}:{1:00}", currentTime / 60, currentTime % 60);
            timerText.SetText("" + text);
        }
    }

    public void StartTimer(){
        isTimerRunning = true;
    }

    public void StopTimer(){
        isTimerRunning = false;
    }

    public void ResetTimer(){
        startTime = (int)Time.time;
    }

    public void Win(){
        StopTimer();
        winDisplay.SetActive(true);
    }

    public bool isWebGame()
    {
        return webGame;
    }

    // ------- METHODS FOR CODE MANAGEMENT ------- //

    public void setNbOfDoors(int nb)
    {
        nbOfDoors = nb;
    }

    public int getNbOfDoors()
    {
        return nbOfDoors;
    }

    // Method to set a new code length
    public void setCodeLength(int length)
    {
        codeLength = length;
    }


    // Method to generate random codes of the given length and bind them to code zones 
    private void generateCodes()
    {
        codeZones = GameObject.FindGameObjectsWithTag("CodeZone");
        setNbOfDoors(codeZones.Length);

        codes = new int[nbOfDoors, codeLength];
        for (int j = 0; j < nbOfDoors; ++j)
        {
            for (int i = 0; i < codeLength; ++i)
            {
                codes[j, i] = Random.Range(0, Config.CELLULO_KEYS);
            }
        }

        // assign them to code zones and doors
        if(nbOfDoors > colors.Length)
        {
            Debug.LogWarning("not enough colors");
            return;
        }
        for (int i=0; i < nbOfDoors; ++i)
        {
            CodeZoneBehavior zone = codeZones[i].GetComponent<CodeZoneBehavior>();

            zone.setCode(getCode(i));
            zone.setColor(colors[i]);
        }

    }

    // Method to return the code from a code zone
    public int[] getCode(int zone)
    {
        int[] ret = new int[codeLength];
        for (int i = 0; i < codeLength; ++i)
        {
            ret[i] = codes[zone, i];
        }
        return ret;
    }

    // Method to get the code length
    public int getCodeLength()
    {
        return codeLength;
    }


    // ------- END OF METHODS FOR CODE MANAGEMENT ------- //
}
