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


    void onCollisionEnter(Collision collisionInfo)
    {
        Collider cellulo = collisionInfo.collider;
        if(cellulo.tag == "Cellulo")
        {
            if (gameManager.isWebGame())
            {
                GameObject display = GameObject.FindGameObjectWithTag("CodeDisplay");
                display.GetComponent<CodeDisplay>().enterCode(codeZoneScript.getColor(), gameObject);
            }
            else
            {
                //TODO handle when with real cellulos
            }


        }

    }
}
