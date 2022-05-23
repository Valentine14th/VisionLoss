using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeZoneBehavior : MonoBehaviour
{
    private int[] code;
    private Color color;
    public GameObject GameManager;
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.GetComponent<GameManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setCode(int[] bindCode)
    {
        code = bindCode;
    }

    public void setColor(Color c)
    {
        color = c;
    }

    public Color getColor()
    {
        return color;
    }

    public int[] getCode()
    {
        return code;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (gameManager.isWebGame())
        {
            GameObject display = GameObject.FindGameObjectWithTag("CodeDisplay");
            display.GetComponent<CodeDisplay>().displayCode(code, color);
        }
        else
        {
            //TO DO, display on cellulos
        }

    }
}
