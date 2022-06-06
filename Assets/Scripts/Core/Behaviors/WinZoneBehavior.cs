using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinZoneBehavior : MonoBehaviour
{

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

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigger win zone");
        GameObject cellulo = other.transform.parent.gameObject;
        if (cellulo.tag == "Cellulo")
        {
            gameManager.playerWon(cellulo.GetComponent<PlayerBehavior>().playerID);

        }

    }
}
