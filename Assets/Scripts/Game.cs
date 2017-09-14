using RaverSoft.YllisanSkies.Characters;
using RaverSoft.YllisanSkies.Events;
using RaverSoft.YllisanSkies.Menu;
using RaverSoft.YllisanSkies.Pathfinding;
using RaverSoft.YllisanSkies.Sound;
using RaverSoft.YllisanSkies.Utils;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RaverSoft.YllisanSkies
{
    public class Game : MonoBehaviour
    {
        private Database database;
        public SaveSystem saveSystem { get; private set; }
        public MenuSystem menu;
        public Options options;
        private static bool gameExists = false;
        public Player player;
        private IniFileHandler translationsFileHandler;
        public bool menuAllowed = true;
        public string currentLocation;
        public bool isSaveAllowed = false;
        public bool stopEvents = false;
        public bool debugMode = false;
        private bool takeScreen = false;
        Texture2D screenshot;
        private bool inTransition = false;
        private Canvas canvas;
        private RawImage fadeOverlay;
        private bool inMapChange = false;
        private LoadMap.TransitionsEffects transitionEffectOut;
        private Vector3 playerStartingPoint;
        private bool firstMap = true;
        public bool inBattle = false;
        private Language defaultLanguage;
        private Language currentLanguage;
        private int currentChapter = 1;
        public float playtime { get; private set; }
        private bool takeScreenForSave = false;

        // Saved data
        public HeroesTeam heroesTeam;
        public EnemiesTeam enemiesTeam;
        private bool menuEnabled = true;

        // Use this for initialization
        private void Start()
        {
            database = new Database();
            database.load();
            saveSystem = new SaveSystem();
            defaultLanguage = database.getLanguageById(LanguageList.English);
            currentLanguage = database.getLanguageById(LanguageList.French);
            canvas = GameObject.Find("Game/Canvas").GetComponent<Canvas>();
            fadeOverlay = GameObject.Find("Game/Canvas/FadeOverlay").GetComponent<RawImage>();
            canvas.enabled = false;
            debugMode = Debug.isDebugBuild;
            translationsFileHandler = new IniFileHandler("Translations/" + currentLanguage.id);
            loadTranslationsTexts();
            menu.game = this;
            options = new Options();
            heroesTeam = new HeroesTeam();
            enemiesTeam = new EnemiesTeam();
            if (!gameExists)
            {
                gameExists = true;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
            Camera.onPostRender += GamePostRender;
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
            playtime = 0;
        }

        public void setPlaytime(int playtime)
        {
            this.playtime = playtime;
        }

        public void save(int saveNumber)
        {
            StartCoroutine(saveRoutine(saveNumber));
        }

        private IEnumerator saveRoutine(int saveNumber)
        {
            takeScreenForSave = true;
            do
            {
                yield return null;
            } while (takeScreenForSave);
            SaveData saveData = new SaveData(heroesTeam, SceneManager.GetActiveScene().name, player, menuEnabled, currentChapter, (int)playtime);
            saveSystem.save(saveNumber, saveData, screenshot);
        }

        public void load(int saveNumber)
        {
            SaveData saveData = saveSystem.load(saveNumber);
            playtime = saveData.playtime;
            heroesTeam = saveData.heroesTeam;
            player.setPosition(saveData.getPlayerPosition());
            player.displayPlayer(true);
            player.setDirection(saveData.getPlayerDirection());
            menuEnabled = saveData.menuEnabled;
            StartCoroutine(changeScene(saveData.scene, player.transform.position, LoadMap.TransitionsEffects.None, LoadMap.TransitionsEffects.None));
        }

        public Database getDatabase()
        {
            return database;
        }

        public void initTestGame()
        {
            heroesTeam.addCharacter(database.getHeroById(HeroList.Cyril));
            heroesTeam.addCharacter(database.getHeroById(HeroList.Max));
            heroesTeam.addCharacter(database.getHeroById(HeroList.Yuna));
            heroesTeam.addCharacter(database.getHeroById(HeroList.Leonard));
            heroesTeam.addMoney(1200);
            heroesTeam.changeLocation(database.getLocationById(LocationList.Osarian));
        }

        public void setTestStartingMap(string mapName)
        {
            if (mapName == "Assets/Scenes/TitleScreen.unity")
            {
                menuEnabled = false;
            }
            else
            {
                initTestGame();
                player.firstInit = true;
                firstMap = true;
                menuEnabled = true;
            }
            SceneManager.LoadScene(mapName);
        }

        private void loadTranslationsTexts()
        {
            for (int i = 1; i <= 4; i++)
            {
                GameObject.Find("Menu/Main/Block_Hero" + i + "/Lv_Title").GetComponent<Text>().text = getTranslation("Stats", "Lv");
                GameObject.Find("Menu/Main/Block_Hero" + i + "/Hp_Title").GetComponent<Text>().text = getTranslation("Stats", "Hp");
                GameObject.Find("Menu/Main/Block_Hero" + i + "/Ap_Title").GetComponent<Text>().text = getTranslation("Stats", "Ap");
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
            GameObject.Find("Menu/Status/Block_Lv/Lv_Title").GetComponent<Text>().text = getTranslation("Stats", "Lv");
            GameObject.Find("Menu/Status/Block_NextLv/NextLv_Title").GetComponent<Text>().text = getTranslation("Stats", "Next lv") + " :";
            GameObject.Find("Menu/Status/Block_NextLv/NextLv_TitleSuffix").GetComponent<Text>().text = getTranslation("Stats", "xp");
        }

        public string getTranslation(string type, string text)
        {
            string translation = text;
            if (currentLanguage != defaultLanguage)
            {
                translation = translationsFileHandler.IniReadValue(type, text);
            }
            return translation;
        }

        // Update is called once per frame
        private void Update()
        {
            playtime += Time.deltaTime;
            checkMenu();
            // SaveSystem tests
            if (Input.GetKeyDown(KeyCode.S))
            {
                save(1);
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                load(1);
            }
        }

        private void checkMenu()
        {
            if ((Input.GetButtonDown("Cancel")) && (!inBattle) && (menuEnabled) && (menuAllowed) && (!menu.isOpened) && (!menu.inTransition))
            {
                stopEvents = true;
                menu.open();
            }
        }

        public void GamePostRender(Camera currentCamera)
        {
            if (takeScreen)
            {
                screenshot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
                screenshot.ReadPixels(currentCamera.pixelRect, 0, 0);
                screenshot.Apply();
                takeScreen = false;
                fadeOverlay.texture = screenshot;
                canvas.enabled = true;
            }
            if (takeScreenForSave)
            {
                screenshot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
                screenshot.ReadPixels(currentCamera.pixelRect, 0, 0);
                screenshot.Apply();
                takeScreenForSave = false;
            }
        }

        public IEnumerator changeScene(string mapToLoad, Vector3 playerStartingPoint, LoadMap.TransitionsEffects transitionEffectIn, LoadMap.TransitionsEffects transitionEffectOut)
        {
            string currentScene = SceneManager.GetActiveScene().name;
            bool needScreenshot = false;
            if (transitionEffectIn == LoadMap.TransitionsEffects.None && transitionEffectOut != LoadMap.TransitionsEffects.None)
            {
                needScreenshot = true;
            }
            StartCoroutine(transition(transitionEffectIn, "In", needScreenshot));
            do
            {
                yield return null;
            } while (inTransition);
            this.playerStartingPoint = playerStartingPoint;
            this.transitionEffectOut = transitionEffectOut;
            StartCoroutine(loadScene(currentScene, mapToLoad));
        }

        private IEnumerator loadScene(string currentScene, string sceneToLoad)
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
            asyncOperation.allowSceneActivation = true;
            yield return asyncOperation;
            SceneManager.UnloadSceneAsync(currentScene);
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (firstMap)
            {
                initGrid();
                firstMap = false;
            }
            if (inMapChange)
            {
                player.transform.position = playerStartingPoint;
                StartCoroutine(transition(transitionEffectOut, "Out"));
            }
        }

        private void OnSceneUnloaded(Scene scene)
        {
            if (!firstMap)
            {
                initGrid();
            }
        }

        private void initGrid()
        {
            GameObject mapObject = GameObject.Find("Map");
            if (mapObject)
            {
                mapObject.GetComponent<Grid>().initialize();
            }
        }

        private IEnumerator transition(LoadMap.TransitionsEffects transitionEffect, string transitionType, bool needScreenshot = false)
        {
            if (transitionType == "In")
            {
                stopEvents = true;
                inMapChange = true;
                menuAllowed = false;
                player.disableMovement();
                if (needScreenshot)
                {
                    takeScreen = true;
                }
                if (transitionEffect != LoadMap.TransitionsEffects.None)
                {
                    fadeOverlay.texture = Resources.Load<Texture>("Transitions/" + transitionEffect);
                    canvas.enabled = true;
                }
            }
            inTransition = true;
            if (transitionEffect != LoadMap.TransitionsEffects.None)
            {
                Animation anim = GetComponent<Animation>();
                anim.Play("Transition_" + transitionEffect + "_" + transitionType);
                do
                {
                    yield return null;
                } while (anim.isPlaying);
            }
            inTransition = false;
            if (transitionType == "Out")
            {
                canvas.enabled = false;
                menuAllowed = true;
                inMapChange = false;
                stopEvents = false;
                player.enableMovement();
            }
        }

        public void playSound(Sounds sound)
        {
            GetComponent<SoundManager>().playSound(sound);
        }
    }
}