using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{

    public Hero[] heroes;
    public Menu menu;
    private static bool gameExists = false;
    public Player player;
    private string language;
    private IniFileHandler translationsFileHandler;

    // Use this for initialization
    private void Start()
    {
        language = "francais";
        translationsFileHandler = new IniFileHandler("Assets/Translations/" + language + ".ini");
        loadTranslationsTexts();
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

    private void initGame()
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

    private void loadTranslationsTexts()
    {
        for (int i = 1; i <= 4; i++)
        {
            Text Menu_Lv_Title = GameObject.Find("Menu/Main/Block_Hero" + i + "/Lv_Title").GetComponent<Text>();
            Menu_Lv_Title.text = translationsFileHandler.IniReadValue("General", "Lv");
            Text Menu_Hp_Title = GameObject.Find("Menu/Main/Block_Hero" + i + "/Hp_Title").GetComponent<Text>();
            Menu_Hp_Title.text = translationsFileHandler.IniReadValue("General", "Hp");
            Text Menu_Ap_Title = GameObject.Find("Menu/Main/Block_Hero" + i + "/Ap_Title").GetComponent<Text>();
            Menu_Ap_Title.text = translationsFileHandler.IniReadValue("General", "Ap");
        }
    }

    // Update is called once per frame
    private void Update()
    {
        checkMenu();
    }

    private void checkMenu()
    {
        if ((Input.GetButton("Cancel")) && (!menu.inTransition))
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
