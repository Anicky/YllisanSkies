using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform target;
    public float speed = 0.1f;
    Camera myCamera;

	// Use this for initialization
	void Start () {
        myCamera = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        myCamera.orthographicSize = Screen.height / 4f;
        if(target) {
            float distance = Vector3.Distance(target.position, new Vector3(90,-90,-10));
            if (distance < 10)
            {
                transform.position = Vector3.Lerp(transform.position, target.position, speed) + new Vector3(0, 0, -10);
            } else
            {
                transform.position = new Vector3(0, 0, -10);
            }
        }
	}
}
