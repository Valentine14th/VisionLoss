using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorZoneBehavior : MonoBehaviour
{
    public GameObject GameManager;
    private GameManager gameManager;
    public GameObject codeZone;
    private CodeZoneBehavior codeZoneScript;
    public GameObject codeDisplay;
    private bool isShowing; // true when the enterCode display is showing
    private int[] enteredCode; //the code the user has currently entered
    public AudioSource doorCodeSound;
    // Start is called before the first frame update
    void Start()
    {
        codeZoneScript = codeZone.GetComponent<CodeZoneBehavior>();
        gameManager = GameManager.GetComponent<GameManager>();
        
    }

    // Update is called once per frame
    void Update()
    { 

    }

    public int[] getCorrectCode()
    {
        return codeZoneScript.getCode();
    }


    void OnCollisionEnter(Collision collisionInfo)
    {
        Debug.Log("collison with door!");
        GameObject cellulo = collisionInfo.collider.transform.parent.gameObject;
        Debug.Log("coolider is: " + cellulo.name);
        if(cellulo.tag == "Cellulo")
        {
            doorCodeSound.Play();
            if (gameManager.isWebGame()) 
            {
                codeDisplay.GetComponent<CodeDisplay>().enterCode(codeZoneScript.getColor(), gameObject);
            }
            else
            {
                cellulo.GetComponent<PlayerBehavior>().readCode(getCorrectCode(), codeZoneScript.getColor(), gameObject);
            }


        }

    }
}
