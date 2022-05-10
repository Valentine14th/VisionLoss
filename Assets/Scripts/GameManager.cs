using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    // ------- VARIABLES FOR CODE MANAGEMENT ------- //

    private int[] player1Code;          // Code for the first player
    private int[] player1EnteredCode;   // The code the first player currently entered
    private int[] player2Code;          // Code for the second player
    private int[] player2EnteredCode;   // The code the second player currently entered
    private int codeLength;             // Length of the codes

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    // ------- METHODS FOR CODE MANAGEMENT ------- //

    // Method to set a new code length
    public void setCodeLength(int length)
    {
        codeLength = length;
    }

    // Method to generate random codes of the given length
    public void generateCodes()
    {
        player1Code = new int[codeLength];
        player2Code = new int[codeLength];
        for(int i = 0; i < codeLength; ++i)
        {
            player1Code[i] = Random.Range(0, 6);
            player2Code[i] = Random.Range(0, 6);
        }
    }

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

    // Method to return the code from a particular player
    public int[] getCode(int player)
    {
        if(player == 1)
        {
            return player1Code;
        }
        else
        {
            return player2Code;
        }
    }

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
