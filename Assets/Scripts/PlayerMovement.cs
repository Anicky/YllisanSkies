using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    Rigidbody2D rbody;
    Animator anim;
    public float speed = 64;
    Tiled2Unity.TiledMap tiledMap;

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

        rbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        GameObject map = GameObject.Find("Map");
        tiledMap = map.GetComponentInParent<Tiled2Unity.TiledMap>();
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
        }
        else
        {
            anim.SetBool("is_walking", false);
        }
    }
}
