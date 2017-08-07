using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {

	List<Hero> heroes;
	public Canvas menu;
	private KeyCode keyCodeToOpenMenu = KeyCode.Escape;
	public bool isMenuOpened = false;

	// Use this for initialization
	void Start () {

		heroes = new List<Hero>();

		Hero heroCyril = new Hero ("Cyril");
		heroCyril.hp = 540;
		heroCyril.hpMax = 720;

		heroes.Add(heroCyril);
	}
	
	// Update is called once per frame
	void Update () {
		checkMenu ();
	}

	void checkMenu() {

		if (Input.GetKeyDown (keyCodeToOpenMenu)) {
			isMenuOpened = !isMenuOpened;

			if (isMenuOpened) {
				menu.enabled = true;
                Animator anim = menu.GetComponent<Animator>();
                anim.SetBool("is_opened", true);
                var thePlayer = FindObjectOfType<PlayerController>();
				thePlayer.disableMovement();
			} else {
				menu.enabled = false;
                Animator anim = menu.GetComponent<Animator>();
                anim.SetBool("is_opened", false);
                var thePlayer = FindObjectOfType<PlayerController>();
				thePlayer.enableMovement();
			}
		}

	}
}
