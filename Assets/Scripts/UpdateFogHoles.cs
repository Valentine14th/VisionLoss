using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateFogHoles : MonoBehaviour
{
    public GameObject cellulo1, cellulo2;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		this.GetComponent<Renderer>().material.SetVector("_Player1_Pos", cellulo1.transform.position);
        this.GetComponent<Renderer>().material.SetVector("_Player2_Pos", cellulo2.transform.position);
	}
}
