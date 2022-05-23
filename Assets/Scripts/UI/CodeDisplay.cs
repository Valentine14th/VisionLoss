using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CodeDisplay : MonoBehaviour
{
    public GameObject GameManager;
    private GameManager gameManager;
    private bool enterCodeMode; //whether the display is in enter code mode
    private GameObject[] buttons; // the code entering buttons
    private GameObject clear; // the clear button
    private GameObject retry; // retry Text
    private List<int> enteredCode; // the code currently entered by the user
    private bool initialized; // whether the buttons have been put in their varîables yet
    private GameObject currentDoorZone; // is updated whenever enterCode() is called

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.GetComponent<GameManager>();
        initialized = false;
        enteredCode = new List<int>();
        
    }

    // called whenever the object is enabled and active, necessary because find only finds active objects
    void OnEnable()
    {
        if (!initialized)
        {
            /* strict order of buttons
            for (int i = 0; i < gameManager.getNbOfDoors(); ++i)
            {
                buttons[i] = GameObject.Find("B" + i);
            }
            */
            buttons = GameObject.FindGameObjectsWithTag("CodeButton");
            clear = GameObject.Find("Clear");
            retry = GameObject.Find("Retry");
            initialized = true;

        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // coroutine to display the code on UI
    private IEnumerator displaySlowly(float waitTime, Color color)
    {
        for (int i = 0; i < gameManager.getNbOfDoors(); ++i)
        {
            Image buttonImage = buttons[i].GetComponent<Image>();
            Color initial = buttonImage.color;
            buttonImage.color = color;
            yield return new WaitForSeconds(waitTime);
            buttonImage.color = initial;
        }
    }

    // opens up the code display in display mode and displays the given code once
    public void displayCode(int[] code, Color color)
    {
        gameObject.SetActive(true);
        if (enterCodeMode)
        {
            enterCodeMode = false;
            clear.SetActive(false);
            foreach (var button in buttons)
            {
                button.GetComponent<Button>().enabled = false;
            }
        }
        StartCoroutine(displaySlowly(gameManager.waitTime, color));
    }

    // opens up the code display in enterCode mode 
    public void enterCode(Color color, GameObject doorZone)
    {
        currentDoorZone = doorZone;
        gameObject.SetActive(true);
        if (!enterCodeMode)
        {
            foreach (var button in buttons)
            {
                enterCodeMode = true;
                clear.SetActive(true);
                button.GetComponent<Button>().enabled = true;
                button.GetComponent<Image>().color = color;
            }

        }
    }

    private IEnumerator displayRetry()
    {
        retry.SetActive(true);
        yield return new WaitForSeconds(gameManager.waitTime);
        retry.SetActive(false);

    }

    //checks that the entered code is the same as the given code, clears buffer if full lenght but wrong
    private bool checkCode()
    {
        if(enteredCode.Count == gameManager.getCodeLength())
        {
            for(int i = 0; i < gameManager.getCodeLength(); ++i)
            {
                if(enteredCode[i] != currentDoorZone.GetComponent<DoorZoneBehavior>().getCorrectCode() [i])
                {
                    enteredCode.Clear();
                    if (gameManager.isWebGame())
                    {
                        StartCoroutine(displayRetry());
                    }
                    return false;
                }
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    // is called whenever a UI code button is pushed
    public void addCodeDigit(int i)
    {
        enteredCode.Add(i);
        if (checkCode())
        {
            // close display and open door
            gameObject.SetActive(false);
            currentDoorZone.SetActive(false);
            //play a sound?

        }

    }

    public void clearCode()
    {
        enteredCode.Clear();
    }

    public void closeDisplay()
    {
        gameObject.SetActive(false);
    }
}
