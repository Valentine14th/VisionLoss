using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLevel2 : MonoBehaviour
{
    public GameObject GameManager;
    // Start is called before the first frame update
    void Start()
    {
        GameManager gameManager = GameManager.GetComponent<GameManager>();
        if (gameManager.isWebGame())
        {
            transform.localScale = new Vector3(1, 1, 1);
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
