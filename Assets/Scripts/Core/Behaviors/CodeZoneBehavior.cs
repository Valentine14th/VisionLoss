using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeZoneBehavior : MonoBehaviour
{
    private int[] code;
    private Color color;
    public GameObject GameManager;
    private GameManager gameManager;
    public GameObject display;
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
        Debug.Log("trigger code zone");
        GameObject cellulo = other.transform.parent.gameObject;
        if (cellulo.tag == "Cellulo")
        {
            if (gameManager.isWebGame()) // if (gameManager.isWebGame())
            {
                display.GetComponent<CodeDisplay>().displayCode(code, color);

            }
            else
            {
                cellulo.GetComponent<PlayerBehavior>().showCode(code, color);
            }
        }

    }
}
