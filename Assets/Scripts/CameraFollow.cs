using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform target;
    public float speed = 0.1f;
    Camera mainCamera;
    float mapWidth = 480; // @TODO : Fix by get real map width
    float mapHeight = 320; // @TODO : Fix by get real map width

    // Use this for initialization
    void Start()
    {
        mainCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        mainCamera.orthographicSize = Screen.height / 2f;
        float cameraHeight = 2f * mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;
        float cameraBorderLeft = (cameraWidth / 2) - 12;
        float cameraBorderRight = mapWidth - (cameraWidth / 2) - 16;
        float cameraBorderUp = -(cameraHeight / 2);
        float cameraBorderDown = -mapHeight + (cameraHeight / 2) + 2;
        if (target)
        {
            float toX = target.position.x;
            float toY = target.position.y;
            if (target.position.x < cameraBorderLeft)
            {
                toX = cameraBorderLeft;
            }
            else if (target.position.x > cameraBorderRight)
            {
                toX = cameraBorderRight;
            }
            if (target.position.y > cameraBorderUp)
            {
                toY = cameraBorderUp;
            }
            else if (target.position.y < cameraBorderDown)
            {
                toY = cameraBorderDown;
            }
            transform.position = Vector3.Lerp(
                new Vector3(transform.position.x, transform.position.y, transform.position.z),
                new Vector3(toX, toY, transform.position.z)
                , speed
                );
        }
    }
}
