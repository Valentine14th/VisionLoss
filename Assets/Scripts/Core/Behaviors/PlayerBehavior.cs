using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

//Input Keys
public enum InputKeyboard{
    arrows = 0, 
    wasd = 1
}
public class PlayerBehavior : AgentBehaviour
{
    public int playerID; // PLayer ID, 1 or 2
    public GameObject GameManager; // Game manager object
    private GameManager gameManager;

    public float linSpeed; // Linear speed to move to the center of a zone
    public float angSpeed; // Angular speed to move to the center of a zone

    public float epsilon; // Precision of position checks
    public InputKeyboard inputKeyboard; 
    public float wallRange; // Range at which walls are detected

    private bool hasStarted; // Whether the cellulo is has been to the starting position or not

    private bool codeIsCorrect;

    //private bool setLed = false;

    public float startingPosX;
    public float startingPosY;

    private List<int> enteredCode;

    void Start()
    {
        enteredCode = new List<int>();
        hasStarted = false;
        gameManager = GameManager.GetComponent<GameManager>();
        if (!gameManager.isWebGame())
        {
            agent.SetVisualEffect(VisualEffect.VisualEffectConstAll, Color.black, 0);
            agent.SetVisualEffect(VisualEffect.VisualEffectConstSingle, Color.white, 0);
        }
        //DEBUG
        hasStarted = true;
    }
    /**
    void FixedUpdate()
    {
        
        if(!hasStarted && !walking)
        {
            agent._celluloRobot.SetGoalPosition(startingPosX, startingPosY, agent.maxAccel);
            agent.isMoved = false;
            walking = true;
            agent.SetVisualEffect(VisualEffect.VisualEffectPulse, Color.yellow, 0);
        }
        
        if(!hasStarted)
        {
            hasStarted = true;
            walking = false;
            agent.SetCasualBackdriveAssistEnabled(true); // TODO remove this?
            agent.ActivateDirectionalHapticFeedback(); // TODO test this, should work with walls directly, otherwise activate normal wall response mode
            agent.isMoved = true;
            
        }
        
    }
    **/

    // Method to show the code on the cellulo
    public void showCode(int[] code, Color color)
    {
        if(!hasStarted)
        {
            return;
        }
        Debug.Log("Show code called");
        StartCoroutine(showCodeCoroutine(code, color));
    }

    private IEnumerator showCodeCoroutine(int[] code, Color color)
    {
        for(int i = 0; i < code.Length; ++i)
        {
            agent.SetVisualEffect(VisualEffect.VisualEffectConstAll, Color.black, 0);
            yield return new WaitForSeconds(gameManager.waitTime/2);
            agent.SetVisualEffect(VisualEffect.VisualEffectConstSingle, color, code[i]);
            yield return new WaitForSeconds(gameManager.waitTime);
        }
        agent.SetVisualEffect(VisualEffect.VisualEffectConstAll, Color.black, 0);
        agent.SetVisualEffect(VisualEffect.VisualEffectConstSingle, Color.white, 0);
    }

    // Coroutine to wait for a given amount of time
    private IEnumerator waitForTime()
    {
        yield return new WaitForSeconds(gameManager.waitTime);
    }

    // Goal angle reached
    public void OnGoalPoseReached()
    {
        if(!hasStarted)
        {
            hasStarted = true;
            agent.SetCasualBackdriveAssistEnabled(true); // TODO remove this?
            //agent.ActivateDirectionalHapticFeedback(); // TODO test this, should work with walls directly, otherwise activate normal wall response mode
            agent.isMoved = true;
        }
    }

    private IEnumerator readCodeCoroutine(int[] correctCode, Color color, GameObject door)
    {
        agent.SetVisualEffect(VisualEffect.VisualEffectConstAll, color, 0);
        yield return new WaitForSeconds(gameManager.waitTime);
        agent.SetVisualEffect(VisualEffect.VisualEffectConstAll, Color.black, 0);
        int length = correctCode.Length;
        // bool releasedAll = true; // Uncomment this line to enable releasedAll functionality
        while(enteredCode.Count < length)
        {
            /*
            if(releasedAll)
            {
                for(int i = 0; i < Config.CELLULO_KEYS; ++i)
                {
                    if(agent._celluloRobot.GetTouch(i) == Touch.TouchBegan)
                    {
                        enteredCode.Add(i);
                        Debug.Log("Touch began on key " + i); // If this sends duplicates, enable "releasedAll" functionality
                        // releasedAll = false;
                        break;
                    }
                }
            }
            else
            {
                // // Remove comments here in bulk (alt+shift+mouse) to allow releasedAll functionality
                // bool test = true;
                // for(int i = 0; i < Config.CELLULO_KEYS; ++i)
                // {
                //     if(agent._celluloRobot.GetTouch(i) != Touch.TouchReleased)
                //     {
                //         test = false;
                //         break;
                //     }
                // }
                // if(test)
                // {
                //     releasedAll = true;
                //     Debug.Log("All keys were released");
                // }
            }
            */
            yield return new WaitForSeconds(0.05f); // Test 20 times per second. Can be adjusted
        }
        for(int i = 0; i < length; ++i)
        {
            if(enteredCode[i] != correctCode[i])
            {
                codeIsCorrect = false;
                break;
            }
        }
        agent.SetVisualEffect(VisualEffect.VisualEffectPulse, (codeIsCorrect) ? Color.green : Color.red, 0);
        if (codeIsCorrect)
        {
            door.SetActive(false);
            // TODO: play sound
        }
        yield return new WaitForSeconds(gameManager.waitTime);
        agent.SetVisualEffect(VisualEffect.VisualEffectConstAll, Color.black, 0);
        agent.SetVisualEffect(VisualEffect.VisualEffectConstSingle, Color.white, 0);
    }

    void OnTouchBegan(int key)
    {
        Debug.Log("touch began");
        enteredCode.Add(key);
    }

    // Method to read the code entered on the cellulo. Blinks green if the code is correct and red otherwise
    public void readCode(int[] correctCode, Color color, GameObject door)
    {
        if(!hasStarted)
        {
            return;
        }

        // get the code the player enters on the cellulo leds
        enteredCode.Clear();
        StartCoroutine(readCodeCoroutine(correctCode, color, door));
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
            if(!hasStarted)
            {
                steering.linear = Vector3.zero;
                return steering;
            }
            List<Vector3> walls = new List<Vector3>();

            // TODO potentially remove this if new template good. Makes the cellulo flee the walls in a given radius
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
