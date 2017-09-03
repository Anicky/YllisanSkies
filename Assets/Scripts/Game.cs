﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using RaverSoft.YllisanSkies.Characters;
using RaverSoft.YllisanSkies.Events;
using RaverSoft.YllisanSkies.Pathfinding;
using RaverSoft.YllisanSkies.Utils;

namespace RaverSoft.YllisanSkies
{
    public class Game : MonoBehaviour
    {
        private Database database;
        public HeroesTeam heroesTeam;
        public EnemiesTeam enemiesTeam;
        public Menu menu;
        public Options options;
        private static bool gameExists = false;
        public Player player;
        private IniFileHandler translationsFileHandler;
        private bool menuEnabled = true;
        public bool menuAllowed = true;
        public string currentLocation;
        public bool isSaveAllowed = false;
        public string startingMapName;
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

        // Use this for initialization
        private void Start()
        {
            database = new Database();
            database.load();
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
            Camera.onPostRender += GamePostRender;
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        public Database getDatabase()
        {
            return database;
        }

        private void initGame()
        {
            heroesTeam.addHero(database.getHeroById(HeroList.Cyril));
            heroesTeam.addHero(database.getHeroById(HeroList.Max));
            heroesTeam.addMoney(1200);
            heroesTeam.changeLocation(database.getLocationById(LocationList.Osarian));
        }

        public void setStartingMap(string mapName)
        {
            player.firstInit = true;
            firstMap = true;
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
            checkMenu();
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
    }
}