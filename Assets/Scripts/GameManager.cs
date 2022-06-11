using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{

    // ------- VARIABLES FOR CODE MANAGEMENT ------- //

    private static Color[] colors = { Color.blue, Color.green, Color.red, Color.magenta, Color.yellow, Color.cyan, new Color(1, 0.5f, 0.5f, 1)};
    private int[,] codes;              // Code for each door
    private GameObject[] codeZones;    // the set of code zones
    public int codeLength;            // Length of the codes
    private int nbOfDoors;             // nb of doors and thus codes in the level
    private bool[] won;                   // whether each player has reached the end of the maze
    public bool webGame;               // whether we are playing the webbased/screenbased game
    public float waitTime;             // time between code display pulses
    
    private int startTime = 0;
    private int currentTime = 0;
    private bool isTimerRunning = false;
    public TMP_Text timerText;

    public GameObject winDisplay;
    public AudioSource reachExit;
    public AudioSource winSound;


    // ------- END OF VARIABLES FOR CODE MANAGEMENT ------- //


    // Start is called before the first frame update
    void Start()
    {
        won = new bool[2];
        won[0] = false;
        won[1] = false;
        //setCodeLength(3);
        generateCodes();
        nbOfDoors = codeZones.Length;
        Debug.Log("nb of doors is: " + nbOfDoors);
        StartTimer();
        //webGame = true; // TODO: make it possible to choose
        
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

    public void setWebGame(bool set)
    {
        webGame = set;
    }

    public void setNbOfDoors(int nb)
    {
        nbOfDoors = nb;
    }

    public int getNbOfDoors()
    {
        return nbOfDoors;
    }

    public void playerWon(int id)
    {
        if (!won[id - 1])
        {
            won[id - 1] = true;
            reachExit.Play();
        }
        if(won[0] && won[1])
        {
            winSound.Play();
            Win();
        }
    }

    // ------- METHODS FOR CODE MANAGEMENT ------- //


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
