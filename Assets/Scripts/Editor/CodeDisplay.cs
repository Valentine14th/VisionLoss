using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeDisplay : MonoBehaviour
{
    private bool enterCodeMode;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setToEnterCodeMode(bool choice)
    {
        if (choice)
        {
            enterCodeMode = true;

        }

    }
}
