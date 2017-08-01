﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform target;
    public float speed = 0.1f;
    Camera mainCamera;
    float mapWidth;
    float mapHeight;
    public bool alwaysCenteredToTarget = false;
    Tiled2Unity.TiledMap tiledMap;

    float getRelativeX()
    {
        return target.position.x - tiledMap.transform.position.x;
    }

    float getRelativeY()
    {
        return target.position.y - tiledMap.transform.position.y;
    }

    // Use this for initialization
    void Start()
    {
        mainCamera = GetComponent<Camera>();
        GameObject map = GameObject.Find("Map");
        tiledMap = map.GetComponentInParent<Tiled2Unity.TiledMap>();
    }

    // Update is called once per frame
    void Update()
    {
        mainCamera.orthographicSize = Screen.height / 2f;
        if (target)
        {
            float toX = target.position.x;
            float toY = target.position.y;
            if (!alwaysCenteredToTarget)
            {
                float cameraHeight = 2f * mainCamera.orthographicSize;
                float cameraWidth = cameraHeight * mainCamera.aspect;
                float cameraLeftLimitToFollowTarget = (cameraWidth / 2);
                float cameraRightLimitToFollowTarget = tiledMap.GetMapWidthInPixelsScaled() - (cameraWidth / 2);
                float cameraUpLimitToFollowTarget = -(cameraHeight / 2);
                float cameraDownLimitToFollowTarget = -tiledMap.GetMapHeightInPixelsScaled() + (cameraHeight / 2);
                if (getRelativeX() < cameraLeftLimitToFollowTarget)
                {
                    toX = tiledMap.transform.position.x + cameraLeftLimitToFollowTarget;
                }
                else if (getRelativeX() > cameraRightLimitToFollowTarget)
                {
                    toX = tiledMap.transform.position.x + cameraRightLimitToFollowTarget;
                }
                if (getRelativeY() > cameraUpLimitToFollowTarget)
                {
                    toY = tiledMap.transform.position.y + cameraUpLimitToFollowTarget;
                }
                else if (getRelativeY() < cameraDownLimitToFollowTarget)
                {
                    toY = tiledMap.transform.position.y + cameraDownLimitToFollowTarget;
                }
            }
            transform.position = Vector3.Lerp(
                new Vector3(transform.position.x, transform.position.y, transform.position.z),
                new Vector3(toX, toY, transform.position.z)
                , speed
                );
        }
    }
}
