using UnityEngine;

public class Game : MonoBehaviour
{

    public Hero[] heroes;
    public Menu menu;
    private static bool gameExists = false;
    public Player player;

    // Use this for initialization
    void Start()
    {
        menu.game = this;
        heroes = new Hero[] { null, null, null, null };
        initGame();
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

    void initGame()
    {
        Hero heroCyril = new Hero("Cyril");
        heroCyril.lv = 1;
        heroCyril.hp = 750;
        heroCyril.hpMax = 750;
        heroCyril.ap = 80;
        heroCyril.apMax = 80;

        Hero heroMax = new Hero("Max");
        heroMax.lv = 2;
        heroMax.hp = 830;
        heroMax.hpMax = 830;
        heroMax.ap = 60;
        heroMax.apMax = 60;

        heroes[0] = heroCyril;
        heroes[1] = heroMax;
    }

    // Update is called once per frame
    void Update()
    {
        checkMenu();
    }

    void checkMenu()
    {
        if (Input.GetButton("Cancel"))
        {
            if (menu.isOpened)
            {
                menu.close();
            }
            else
            {
                menu.open();
            }
        }
    }
}
