using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeZoneBehavior : MonoBehaviour
{
    private int[] code;
    public GameObject GameManager;
    public GameObject door;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setCode(int[] bindCode)
    {
        code = bindCode;
    }

    public void OnTriggerEnter()
    {
        if (GameManager.GetComponent<GameManager>().isWebGame())
        {
            GameObject display = GameObject.FindGameObjectWithTag("CodeDisplay");
            display.SetActive(true);


        }
        else
        {
            //TO DO, display on cellulos
        }

    }
}
