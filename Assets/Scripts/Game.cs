using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{

    public Hero[] heroes;
    public Menu menu;
    public Options options;
    private static bool gameExists = false;
    public Player player;
    private string language;
    private IniFileHandler translationsFileHandler;
    private bool menuEnabled = true;
    public int moneyCollected = 0;
    public int currentMoney = 0;
    public string currentLocation;
    public bool isSaveAllowed = false;
    public string startingMapName;
    public bool stopEvents = false;

    // Use this for initialization
    private void Start()
    {
        language = "francais";
        translationsFileHandler = new IniFileHandler("Translations/" + language);
        loadTranslationsTexts();
        menu.game = this;
        options = new Options();
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
        setStartingMap(startingMapName);
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

        currentMoney = 1200;
        currentLocation = "Forest of Hopes";
    }

    public void setStartingMap(string mapName)
    {
        SceneManager.LoadScene(mapName);
    }

    private void loadTranslationsTexts()
    {
        for (int i = 1; i <= 4; i++)
        {
            GameObject.Find("Menu/Main/Block_Hero" + i + "/Lv_Title").GetComponent<Text>().text = getTranslation("General", "Lv");
            GameObject.Find("Menu/Main/Block_Hero" + i + "/Hp_Title").GetComponent<Text>().text = getTranslation("General", "Hp");
            GameObject.Find("Menu/Main/Block_Hero" + i + "/Ap_Title").GetComponent<Text>().text = getTranslation("General", "Ap");
        }
        GameObject.Find("Menu/Main/Section_01/Section_Title").GetComponent<Text>().text = getTranslation("Menu", "Items");
        GameObject.Find("Menu/Main/Section_02/Section_Title").GetComponent<Text>().text = getTranslation("Menu", "Status");
        GameObject.Find("Menu/Main/Section_03/Section_Title").GetComponent<Text>().text = getTranslation("Menu", "Equipment");
        GameObject.Find("Menu/Main/Section_04/Section_Title").GetComponent<Text>().text = getTranslation("Menu", "Abilities");
        GameObject.Find("Menu/Main/Section_05/Section_Title").GetComponent<Text>().text = getTranslation("Menu", "Airship");
        GameObject.Find("Menu/Main/Section_06/Section_Title").GetComponent<Text>().text = getTranslation("Menu", "Journal");
        GameObject.Find("Menu/Main/Section_07/Section_Title").GetComponent<Text>().text = getTranslation("Menu", "Options");
        GameObject.Find("Menu/Main/Section_08/Section_Title").GetComponent<Text>().text = getTranslation("Menu", "Save");
        GameObject.Find("Menu/Main/Section_09/Section_Title").GetComponent<Text>().text = getTranslation("Menu", "Quit");
        GameObject.Find("Menu/Main/Block_Money/Money_Title").GetComponent<Text>().text = getTranslation("Menu", "Money");
        GameObject.Find("Menu/Main/Block_Time/Time_Title").GetComponent<Text>().text = getTranslation("Menu", "Time");
        GameObject.Find("Menu/Items/Block_Title/Section_Title").GetComponent<Text>().text = getTranslation("Menu", "Items");
        GameObject.Find("Menu/Status/Block_Title/Section_Title").GetComponent<Text>().text = getTranslation("Menu", "Status");
        GameObject.Find("Menu/Equipment/Block_Title/Section_Title").GetComponent<Text>().text = getTranslation("Menu", "Equipment");
        GameObject.Find("Menu/Abilities/Block_Title/Section_Title").GetComponent<Text>().text = getTranslation("Menu", "Abilities");
        GameObject.Find("Menu/Airship/Block_Title/Section_Title").GetComponent<Text>().text = getTranslation("Menu", "Airship");
        GameObject.Find("Menu/Journal/Block_Title/Section_Title").GetComponent<Text>().text = getTranslation("Menu", "Journal");
        GameObject.Find("Menu/Options/Block_Title/Section_Title").GetComponent<Text>().text = getTranslation("Menu", "Options");
    }

    public string getTranslation(string type, string text)
    {
        return translationsFileHandler.IniReadValue(type, text);
    }

    // Update is called once per frame
    private void Update()
    {
        checkMenu();
    }

    private void checkMenu()
    {
        if ((Input.GetButtonDown("Cancel")) && (menuEnabled) && (!menu.isOpened) && (!menu.inTransition))
        {
            stopEvents = true;
            menu.open();
        }
    }

    public int getNumberOfHeroes()
    {
        int numberOfHeroes = 0;
        for (int i = 0; i < heroes.Length; i++)
        {
            if (heroes[i] != null)
            {
                numberOfHeroes++;
            }
        }
        return numberOfHeroes;
    }
}
