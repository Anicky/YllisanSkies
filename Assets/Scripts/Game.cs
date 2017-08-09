using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {

	List<Hero> heroes;
	public Menu menu;
	private KeyCode keyCodeToOpenMenu = KeyCode.Escape;
    private static bool gameExists = false;

	// Use this for initialization
	void Start () {

        heroes = new List<Hero>();

		Hero heroCyril = new Hero ("Cyril");
		heroCyril.hp = 540;
		heroCyril.hpMax = 720;

		heroes.Add(heroCyril);
        if (!gameExists)
        {
            gameExists = true;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
	
	// Update is called once per frame
	void Update () {
		checkMenu ();
	}

	void checkMenu() {
		if (Input.GetKeyDown (keyCodeToOpenMenu)) {
			if (menu.isOpened) {
				menu.close ();
			} else {
				menu.open ();
			}
		}
	}
}
