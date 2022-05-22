using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    // ------- VARIABLES FOR CODE MANAGEMENT ------- //

    private int[,] codes;               // Code for each door
    private int[] player1EnteredCode;   // The code the first player currently entered
    private int[] player2EnteredCode;   // The code the second player currently entered
    private GameObject[] codeZones;     // the set of code zones
    private int codeLength;             // Length of the codes
    public int nbOfDoors;               // nb of doors and thus codes in the level
    private bool webGame;               // whether we are playing the webbased game

    // Enum to specify what is returned to the cellulos' scripts
    public enum CodeCheckReturn
    {
        Incomplete,
        Wrong,
        Correct
    }

    // ------- END OF VARIABLES FOR CODE MANAGEMENT ------- //



    // Start is called before the first frame update
    void Start()
    {
        setNbOfDoor(2);
        setCodeLength(6);
        generateCodes();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    // ------- METHODS FOR CODE MANAGEMENT ------- //

    public void setNbOfDoor(int nb)
    {
        nbOfDoors = nb;
    }

    // Method to set a new code length
    public void setCodeLength(int length)
    {
        codeLength = length;
    }

    public bool isWebGame()
    {
        return webGame;
    }

    // Method to generate random codes of the given length and bind them to code zones 
    public void generateCodes()
    {
        codes = new int[nbOfDoors, codeLength];
        for (int j = 0; j < nbOfDoors; ++j)
        {
            for (int i = 0; i < codeLength; ++i)
            {
                codes[j, i] = Random.Range(0, Config.CELLULO_KEYS);
            }
        }


        // TODO assign them to code zones and doors
        codeZones = GameObject.FindGameObjectsWithTag("CodeZone");
        for(int i=0; i < nbOfDoors; ++i)
        {

            codeZones[i].GetComponent<CodeZoneBehavior>().setCode(getCode(i));
        }

    }

    /*
    // Method to reset the codes entered by a given player
    public void resetEnteredCode(int player)
    {
        for(int i = 0; i < codeLength; ++i)
        {
            if(player == 1)
            {
                player1EnteredCode[i] = 0;
            }
            else
            {
                player2EnteredCode[i] = 0;
            }
        }
    }
    */

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

    /*
    // Method to check if the entered code of the player is correct
    public bool checkCode(int player)
    {
        for(int i = 0; i < codeLength; ++i)
        {
            if(player == 1)
            {
                if(player1Code[i] != player1EnteredCode[i])
                {
                    return false;
                }
            }
            else
            {
                if(player2Code[i] != player2EnteredCode[i])
                {
                    return false;
                }
            }
        }
        return true;
    }
    */

    /*
    // Method to register a key stroke in a given code. This returns Correct if the code is correct, Wrong if the code is wrong (as well
    // as resetting the entered code) and Incomplete if the code has not been fully sent yet
    public CodeCheckReturn enterKeyStroke(int player, int key)
    {
        for(int i = 0; i < codeLength; ++i)
        {
            if(player == 1)
            {
                if(player1EnteredCode[i] == 0)
                {
                    player1EnteredCode[i] = key;
                    if(i == codeLength - 1)
                    {
                        if(checkCode(1))
                        {
                            onCorrectCode(1);
                            return CodeCheckReturn.Correct;
                        }
                        else
                        {
                            resetEnteredCode(1);
                            return CodeCheckReturn.Wrong;
                        }
                    }
                    else
                    {
                        return CodeCheckReturn.Incomplete;
                    }
                }
                else if(i == codeLength - 1)
                {
                    return CodeCheckReturn.Correct;
                }
            }
            else
            {
                if(player2EnteredCode[i] == 0)
                {
                    player2EnteredCode[i] = key;
                    if(i == codeLength - 1)
                    {
                        if(checkCode(2))
                        {
                            onCorrectCode(2);
                            return CodeCheckReturn.Correct;
                        }
                        else
                        {
                            resetEnteredCode(2);
                            return CodeCheckReturn.Wrong;
                        }
                    }
                    else
                    {
                        return CodeCheckReturn.Incomplete;
                    }
                }
                else if(i == codeLength - 1)
                {
                    return CodeCheckReturn.Correct;
                }
            }
        }
        return CodeCheckReturn.Incomplete;
        
    }
    */

    // Method called when a given player has entered the correct code
    public void onCorrectCode(int player)
    {
        if(player == 1)
        {
            // ######## TODO
        }
        else
        {
            // ######## TODO
        }
    }

    // ------- END OF METHODS FOR CODE MANAGEMENT ------- //
}
