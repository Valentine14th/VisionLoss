using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CodeDisplay : MonoBehaviour
{
    private bool enterCodeMode;
    private GameObject[] buttons;
    private GameObject clear;
    public GameObject GameManager;
    private GameManager gameManager;
    private List<int> enteredCode;
    private bool initialized;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.GetComponent<GameManager>();
        initialized = false;
        enteredCode = new List<int>();
        

    }

    // called whenever the object is enabled and active
    void OnEnable()
    {
        if (!initialized)
        {
            for (int i = 0; i < gameManager.getNbOfDoors(); ++i)
            {
                buttons[i] = GameObject.Find("B" + i);
            }
            //buttons = GameObject.FindGameObjectsWithTag("CodeButton");
            clear = GameObject.Find("Clear");
            initialized = true;

        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setToEnterCodeMode(bool choice)
    {
        enterCodeMode = choice;

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
    public void enterCode(Color color)
    {
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

    //checks that the entered code is the same as the given code, clears buffer if full lenght but wrong
    private bool checkCode(int[] code)
    {
        if(enteredCode.Count == gameManager.getCodeLength())
        {
            for(int i = 0; i < gameManager.getCodeLength(); ++i)
            {
                if(enteredCode[i] != code[i])
                {
                    enteredCode.Clear();
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

    public void addCodeDigit(int i)
    {
        if(enteredCode.Count >= gameManager.getCodeLength())
        {
            enteredCode.Clear();
        }
        enteredCode.Add(i);

    }

    public void clearCode()
    {
        enteredCode.Clear();
    }
}
