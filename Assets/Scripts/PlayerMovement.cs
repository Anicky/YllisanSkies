using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    Rigidbody2D rbody;
    Animator anim;
    public float speed = 64;
	float mapWidth;
	float mapHeight;

	// Use this for initialization
	void Start () {

        rbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
		GameObject map = GameObject.Find ("Foret_des_Espoirs");
		var tiledMap = map.GetComponentInParent<Tiled2Unity.TiledMap>();
		mapWidth = tiledMap.GetMapWidthInPixelsScaled();
		mapHeight = tiledMap.GetMapHeightInPixelsScaled();

		Debug.Log (rbody.position.x);

	}
	
	// Update is called once per frame
	void Update () {

        Vector2 movementVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

		bool isWalking = false;

		if (movementVector != Vector2.zero) {
			isWalking = true;
			if (movementVector.x < 0 && rbody.position.x < 0) {
				isWalking = false;
			}
			if (movementVector.y > 0 && rbody.position.y > 0) {
				isWalking = false;
			}
			if (movementVector.x > 0 && rbody.position.x > mapWidth - 24) {
				isWalking = false;
			}
			if (movementVector.y < 0 && rbody.position.y < -mapHeight + 16) {
				isWalking = false;
			}
		}

		if (isWalking) {
			anim.SetBool("is_walking", true);
			anim.SetFloat("input_x", movementVector.x);
			anim.SetFloat("input_y", movementVector.y);
			rbody.MovePosition(rbody.position + speed * (movementVector * Time.deltaTime));
		} else {
			anim.SetBool ("is_walking", false);
		}
    }
}
