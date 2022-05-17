using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : AgentBehaviour
{
    public int playerID; // PLayer ID, 1 or 2
    public GameObject gameManager; // Game manager object
    private Zone currentZone; // The current zone the player is in, none, the code transmission zone, or the door zone
    public float waitTime; // The waiting time between code pulses
    private bool correctCodeEntered; // Whether the correct code has been entered yet or not

    // Enum which describes in which zone the player is currently in
    private enum Zone
    {
        None,
        Code,
        Door
    }

    void Start()
    {
        agent.SetCasualBackdriveAssistEnabled(true);
    }

    void FixedUpdate()
    {

    }

    // Method to show the code on the cellulo
    private void showCode()
    {
        Color color = (playerID == 1) ? Color.red : Color.blue;
        int[] code = gameManager.GetComponent<GameManager>().getCode(playerID);
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
        yield return new WaitForSeconds(waitTime);
    }

    // Method to read the code entered on the cellulo. Blinks green if the code is correct and red otherwise
    private void readCode()
    {
        int length = gameManager.GetComponent<GameManager>().getCodeLength();
        GameManager.CodeCheckReturn returnCode = GameManager.CodeCheckReturn.Incomplete;
        for(int i = 0; i < length; ++i)
        {
            bool found = false;
            int currentKey = -1;
            while(!found)
            {
                for(int j = 0; j < 6; ++j)
                {
                    if(agent._celluloRobot.GetTouch(j) == Touch.TouchBegan)
                    {
                        found = true;
                        currentKey = j;
                        break;
                    }
                }
            }
            returnCode = gameManager.GetComponent<GameManager>().enterKeyStroke(playerID, currentKey);
            StartCoroutine(waitForRelease(currentKey));
        }
        if(returnCode == GameManager.CodeCheckReturn.Correct)
        {
            correctCodeEntered = true;
            agent.SetVisualEffect(VisualEffect.VisualEffectPulse, Color.green, 0);
        }
        else
        {
            correctCodeEntered = false;
            agent.SetVisualEffect(VisualEffect.VisualEffectPulse, Color.red, 0);
        }
        StartCoroutine(waitForTime());
        agent.SetVisualEffect(VisualEffect.VisualEffectConstAll, Color.black, 0);
    }

    // Coroutine which waits until a given key is released
    private IEnumerator waitForRelease(int key)
    {
        yield return new WaitUntil(() => agent._celluloRobot.GetTouch(key) == Touch.TouchReleased);
    }

}
