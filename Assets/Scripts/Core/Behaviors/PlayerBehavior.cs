using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

//Input Keys
public enum InputKeyboard{
    arrows =0, 
    wasd = 1
}
public class PlayerBehavior : AgentBehaviour
{
    public int playerID; // PLayer ID, 1 or 2
    public GameObject GameManager; // Game manager object
    private GameManager gameManager;
    private Zone currentZone; // The current zone the player is in, none, the code transmission zone, or the door zone
    private bool correctCodeEntered; // Whether the correct code has been entered yet or not
    private GameObject codeZone; // Zone where the code is shown
    private GameObject doorZone; // Zone where the code is read
    private List<int> enteredCode; // the code currently entered by the user

    public float linSpeed; // Linear speed to move to the center of a zone
    public float angSpeed; // Angular speed to move to the center of a zone

    public float epsilon; // Precision of position checks
    public InputKeyboard inputKeyboard; 

    // Enum which describes in which zone the player is currently in
    private enum Zone
    {
        None, // No zone
        Code, // Code zone
        Door // Door zone
    }

    // Enum to specify what is returned to the cellulos' scripts
    public enum CodeCheckReturn
    {
        Incomplete,
        Wrong,
        Correct
    }

    void Start()
    {
        agent.SetCasualBackdriveAssistEnabled(true);
        gameManager = GameManager.GetComponent<GameManager>();

    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        if (!gameManager.isWebGame())
        {
            if (collisionInfo.collider.tag == "CodeZone" || collisionInfo.collider == doorZone)
            {
                // Move the cellulo to the center of the zone
                agent.isMoved = false;
                agent.SetCasualBackdriveAssistEnabled(false);
                //Vector3 position = collisionInfo.collider.transform.position;
                //agent._celluloRobot.SetGoalPose(position.x, position.z, 0, linSpeed, angSpeed);
                //StartCoroutine(waitUntilAtSpot(position.x, position.z, 0));
                if (collisionInfo.collider.tag == "CodeZone")
                {
                    currentZone = Zone.Code;
                }
                else if (!correctCodeEntered)
                {
                    currentZone = Zone.Door;
                }
            }
        }
    }

    // Coroutine to wait until the cellulo is at a given position and angle
    private IEnumerator waitUntilAtSpot(float x, float y, float t)
    {
        yield return new WaitUntil(() => Math.Abs(agent._celluloRobot.GetX() - x) <= epsilon && Math.Abs(agent._celluloRobot.GetY() - y) <= epsilon && Math.Abs(agent._celluloRobot.GetTheta() - t) <= epsilon);
    }

    void FixedUpdate()
    {
        /*
        if(currentZone == Zone.None)
        {
            // Code to flee walls
        }
        else
        {
            if(currentZone == Zone.Code)
            {
                showCode();
            }
            else
            {
                readCode();
            }
            agent.isMoved = true; // Allow the cellulo to be moved
            agent.SetCasualBackdriveAssistEnabled(true); // Enable the backdrive assist
            currentZone = Zone.None; // Reset the zone
        }
        */
    }

    // Method to show the code on the cellulo
    public void showCode(int[] code, Color color)
    {
        // TODO: show correct orientation, or move to correct orientation

        for(int i = 0; i < code.Length; ++i)
        {
            agent.SetVisualEffect(VisualEffect.VisualEffectConstAll, Color.black, 0);
            agent.SetVisualEffect(VisualEffect.VisualEffectConstSingle, color, code[i]);
            StartCoroutine(waitForTime());
        }
        agent.SetVisualEffect(VisualEffect.VisualEffectConstAll, Color.black, 0);
    }

    // Coroutine to wait for a given amount of time
    private IEnumerator waitForTime()
    {
        yield return new WaitForSeconds(gameManager.waitTime);
    }

    // Method to read the code entered on the cellulo. Blinks green if the code is correct and red otherwise
    public bool readCode(int[] correctCode, Color color)
    {
        //light up led zero to know orientation of code and color of door
        agent.SetVisualEffect(VisualEffect.VisualEffectConstSingle, color, 0);
        //TODO: play sound

        // get the code the player enters on the cellulo leds
        int length = gameManager.getCodeLength();
        CodeCheckReturn returnCode = CodeCheckReturn.Incomplete;
        for(int i = 0; i < length; ++i)
        {
            bool found = false;
            int currentKey = -1;
            while(!found)
            {
                for(int j = 0; j < Config.CELLULO_KEYS; ++j)
                {
                    if(agent._celluloRobot.GetTouch(j) == Touch.TouchBegan)
                    {
                        found = true;
                        currentKey = j;
                        break;
                    }
                }
            }
            // add digit to enteredCode and check for correctness
            returnCode = enterKeyStroke(currentKey, correctCode);
            StartCoroutine(waitForRelease(currentKey));
        }
        if(returnCode == CodeCheckReturn.Correct)
        {
            correctCodeEntered = true;
            agent.SetVisualEffect(VisualEffect.VisualEffectPulse, Color.green, 0);
            //TODO: play sound;
        }
        else
        {
            correctCodeEntered = false;
            agent.SetVisualEffect(VisualEffect.VisualEffectPulse, Color.red, 0);
            //TODO: play sound;
        }
        StartCoroutine(waitForTime());
        agent.SetVisualEffect(VisualEffect.VisualEffectConstAll, Color.black, 0);
        return correctCodeEntered;
    }

    // Coroutine which waits until a given key is released
    private IEnumerator waitForRelease(int key)
    {
        yield return new WaitUntil(() => agent._celluloRobot.GetTouch(key) == Touch.TouchReleased);
    }

  
    // Method to check if the entered code of the player is correct, assumes the entered code is already long enough
    private bool checkCode(int[] code)
    {
        for(int i = 0; i < gameManager.getCodeLength(); ++i)
        {
            if(enteredCode[i] != code[i])
            {
                return false;
            }
        }
        return true;
    }

 
    // Method to reset the codes entered by a given player
    private void resetEnteredCode()
    {
        enteredCode.Clear();

    }

    // Method to register a key stroke in a given code. This returns Correct if the code is correct, Wrong if the code is wrong (as well
    // as resetting the entered code) and Incomplete if the code has not been fully sent yet
    private CodeCheckReturn enterKeyStroke(int key, int[] code)
    {
        enteredCode.Add(key);
        if (enteredCode.Count == code.Length)
        {
            if (checkCode(code))
            {
                resetEnteredCode();
                return CodeCheckReturn.Correct;
            }
            else
            {
                resetEnteredCode();
                return CodeCheckReturn.Wrong;
            }
        }
        else {
            return CodeCheckReturn.Incomplete;
        }  
    }

    // MOTION 

    public override Steering GetSteering()
    {
        Steering steering = new Steering();

        float horizontal;
        float vertical;

        if(inputKeyboard == InputKeyboard.arrows){
            horizontal=Input.GetAxis("HorizontalArrows");
            vertical=Input.GetAxis("VerticalArrows");
        }
        else{
            horizontal=Input.GetAxis("HorizontalWASD");
            vertical=Input.GetAxis("VerticalWASD");
        }
        

        steering.linear=new Vector3(horizontal, 0,vertical)*  agent.maxAccel;
        steering.linear=this.transform.parent.TransformDirection(Vector3.ClampMagnitude(steering.linear, agent.maxAccel));
        return steering;
    }

}
