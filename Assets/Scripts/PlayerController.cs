﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    Rigidbody2D rbody;
    Animator anim;
    public float speed = 64;
    Tiled2Unity.TiledMap tiledMap;
    private static bool playerExists;
    private Vector2 lastMove;
    public string eventNameWherePlayerHasToBeTeleported;

    float getRelativeX()
    {
        return rbody.position.x - tiledMap.transform.position.x;
    }

    float getRelativeY()
    {
        return rbody.position.y - tiledMap.transform.position.y;
    }

    // Use this for initialization
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        initialize();
        if (!playerExists)
        {
            playerExists = true;
            DontDestroyOnLoad(transform.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void initialize()
    {
        rbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        GameObject map = GameObject.Find("Map");
        tiledMap = map.GetComponentInParent<Tiled2Unity.TiledMap>();
        if (eventNameWherePlayerHasToBeTeleported != "")
        {
            GameObject entryPoint = GameObject.Find(eventNameWherePlayerHasToBeTeleported);

            float positionAdjustX = 0;
            float positionAdjustY = 0;

            if (lastMove.x == -1)
            {
                positionAdjustX = -16;
                positionAdjustY = 0;
            }
            if (lastMove.x == 1)
            {
                positionAdjustX = 16;
                positionAdjustY = 0;
            }
            if (lastMove.y == -1)
            {
                positionAdjustX = 8;
                positionAdjustY = -16;
            }
            if (lastMove.y == 1)
            {
                positionAdjustX = 16;
                positionAdjustY = 16;
            }

            transform.position = new Vector3(entryPoint.transform.position.x + positionAdjustX, entryPoint.transform.position.y + positionAdjustY, transform.position.z);
            var theCamera = FindObjectOfType<CameraController>();
            theCamera.transform.position = new Vector3(entryPoint.transform.position.x, entryPoint.transform.position.y, theCamera.transform.position.z);
            eventNameWherePlayerHasToBeTeleported = "";
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        initialize();
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 movementVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        bool isWalking = false;

        if (movementVector != Vector2.zero)
        {
            isWalking = true;
            if (movementVector.x < 0 && getRelativeX() < 8)
            {
                isWalking = false;
            }
            if (movementVector.y > 0 && getRelativeY() > 0)
            {
                isWalking = false;
            }
            if (movementVector.x > 0 && getRelativeX() > tiledMap.GetMapWidthInPixelsScaled() - 12)
            {
                isWalking = false;
            }
            if (movementVector.y < 0 && getRelativeY() < -tiledMap.GetMapHeightInPixelsScaled() + 16)
            {
                isWalking = false;
            }
        }

        if (isWalking)
        {
            anim.SetBool("is_walking", true);
            anim.SetFloat("input_x", movementVector.x);
            anim.SetFloat("input_y", movementVector.y);
            rbody.MovePosition(rbody.position + speed * (movementVector * Time.deltaTime));
            lastMove = movementVector;
        }
        else
        {
            anim.SetBool("is_walking", false);
        }
    }
}