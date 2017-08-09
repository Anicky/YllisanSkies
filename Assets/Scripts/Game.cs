using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {

	public Hero[] heroes;
	public Menu menu;
	private KeyCode keyCodeToOpenMenu = KeyCode.Escape;
    private static bool gameExists = false;
    public Player player;

	// Use this for initialization
	void Start () {
        menu.game = this;
        heroes = new Hero[] { null, null, null, null};

		Hero heroCyril = new Hero ("Cyril");
        heroCyril.lv = 1;
		heroCyril.hp = 540;
		heroCyril.hpMax = 720;
        heroCyril.ap = 120;
        heroCyril.apMax = 140;

        heroes[0] = heroCyril;
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
