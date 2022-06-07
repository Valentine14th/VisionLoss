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

    public float linSpeed; // Linear speed to move to the center of a zone
    public float angSpeed; // Angular speed to move to the center of a zone

    public float epsilon; // Precision of position checks
    public InputKeyboard inputKeyboard; 
    private bool atCorrectAngle; // Whether the cellulo is already at the correct angle
    public float wallRange; // Range at which walls are detected

    // Enum which describes in which zone the player is currently in
    private enum Zone
    {
        None, // No zone
        Code, // Code zone
        Door // Door zone
    }

    void Start()
    {
        agent.SetCasualBackdriveAssistEnabled(true); // TODO remove this?
        gameManager = GameManager.GetComponent<GameManager>();

        agent.ActivateDirectionalHapticFeedback(); // TODO test this, should work with walls directly, otherwise activate normal wall response mode

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

    // Coroutine to wait for a short amount of time
    private IEnumerator waitForShort()
    {
        yield return new WaitForSeconds(0.1f);
    }


    // Goal angle reached
    public void OnGoalPoseReached()
    {
        atCorrectAngle = true;
    }

    // Method to read the code entered on the cellulo. Blinks green if the code is correct and red otherwise
    public bool readCode(int[] correctCode, Color color)
    {
        // Turn cellulo to correct orientation
        if(Math.Abs(agent._celluloRobot.GetTheta()) > epsilon)
        {
            agent._celluloRobot.SetGoalOrientation(0, 50);
            while(!atCorrectAngle)
            {
                StartCoroutine(waitForShort());
            }
            atCorrectAngle = false;
        }


        //light up led zero to know orientation of code and color of door
        agent.SetVisualEffect(VisualEffect.VisualEffectConstSingle, color, 0);
        //TODO: play sound

        // get the code the player enters on the cellulo leds
        int length = correctCode.Length;
        int[] enteredCode = new int[length];
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
            enteredCode[i] = currentKey;
            StartCoroutine(waitForRelease(currentKey));
        }
        bool codeIsCorrect = true;
        for(int i = 0; i < length; ++i)
        {
            if(enteredCode[i] != correctCode[i])
            {
                codeIsCorrect = false;
                break;
            }
        }
        agent.SetVisualEffect(VisualEffect.VisualEffectPulse, (codeIsCorrect) ? Color.green : Color.red, 0);
        // TODO: play sound
        StartCoroutine(waitForTime());
        agent.SetVisualEffect(VisualEffect.VisualEffectConstAll, Color.black, 0);
        return codeIsCorrect;
    }

    // Coroutine which waits until a given key is released
    private IEnumerator waitForRelease(int key)
    {
        yield return new WaitUntil(() => agent._celluloRobot.GetTouch(key) == Touch.TouchReleased);
    }

    // MOTION 

    public override Steering GetSteering()
    {
        Steering steering = new Steering();

        if(gameManager.isWebGame())
        {
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
        }
        else
        {
            List<Vector3> walls = new List<Vector3>();

            // TODO potentially remove this. Makes the cellulo flee the walls in a given radius
            Collider[] rangeCheck = Physics.OverlapSphere(transform.position, wallRange);
            foreach(Collider c in rangeCheck)
            {
                if(c.gameObject.tag == "Wall")
                {
                    walls.Add(c.gameObject.transform.position);
                }
            }

            steering.linear = GetMeanVector(walls) * -agent.maxAccel;
            steering.linear = this.transform.parent.TransformDirection(Vector3.ClampMagnitude(steering.linear, agent.maxAccel));

        }
        
        return steering;
    }

    // Method to calculate the mean position of a list of vectors
    private Vector3 GetMeanVector(List<Vector3> positions){
        if (positions.Count == 0)
        {
            return Vector3.zero;
        }
        
        float x = 0f;
        float z = 0f;

        foreach (Vector3 pos in positions)
        {
            x += pos.x;
            z += pos.z;
        }
        return new Vector3(x / positions.Count, 0, z / positions.Count);
    }

}
