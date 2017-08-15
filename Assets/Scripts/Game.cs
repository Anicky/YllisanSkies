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
    private bool menuEnabled = true;

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
            GameObject.Find("Menu/Main/Block_Hero" + i + "/Lv_Title").GetComponent<Text>().text = translationsFileHandler.IniReadValue("General", "Lv");
            GameObject.Find("Menu/Main/Block_Hero" + i + "/Hp_Title").GetComponent<Text>().text = translationsFileHandler.IniReadValue("General", "Hp");
            GameObject.Find("Menu/Main/Block_Hero" + i + "/Ap_Title").GetComponent<Text>().text = translationsFileHandler.IniReadValue("General", "Ap");
        }
        GameObject.Find("Menu/Main/Section_01/Section_Title").GetComponent<Text>().text = translationsFileHandler.IniReadValue("Menu", "Items");
        GameObject.Find("Menu/Main/Section_02/Section_Title").GetComponent<Text>().text = translationsFileHandler.IniReadValue("Menu", "Status");
        GameObject.Find("Menu/Main/Section_03/Section_Title").GetComponent<Text>().text = translationsFileHandler.IniReadValue("Menu", "Equipment");
        GameObject.Find("Menu/Main/Section_04/Section_Title").GetComponent<Text>().text = translationsFileHandler.IniReadValue("Menu", "Abilities");
        GameObject.Find("Menu/Main/Section_05/Section_Title").GetComponent<Text>().text = translationsFileHandler.IniReadValue("Menu", "Airship");
        GameObject.Find("Menu/Main/Section_06/Section_Title").GetComponent<Text>().text = translationsFileHandler.IniReadValue("Menu", "Journal");
        GameObject.Find("Menu/Main/Section_07/Section_Title").GetComponent<Text>().text = translationsFileHandler.IniReadValue("Menu", "Options");
        GameObject.Find("Menu/Main/Section_08/Section_Title").GetComponent<Text>().text = translationsFileHandler.IniReadValue("Menu", "Save");
        GameObject.Find("Menu/Main/Section_09/Section_Title").GetComponent<Text>().text = translationsFileHandler.IniReadValue("Menu", "Quit");
    }

    // Update is called once per frame
    private void Update()
    {
        checkMenu();
    }

    private void checkMenu()
    {
        if ((Input.GetButtonDown("Cancel")) && (menuEnabled) && (!menu.isOpened))
        {
            menu.open();
        }
    }

    public int getNumberOfHeroes()
    {
        int numberOfHeroes = 0;
        for(int i = 0; i < heroes.Length; i++)
        {
            if (heroes[i] != null)
            {
                numberOfHeroes++;
            }
        }
        return numberOfHeroes;
    }
}
