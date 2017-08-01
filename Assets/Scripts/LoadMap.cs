using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadMap : MonoBehaviour {

    public string mapToLoad;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            Application.LoadLevel(mapToLoad);
        }
    }
}
