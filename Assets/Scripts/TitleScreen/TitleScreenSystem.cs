using RaverSoft.YllisanSkies.Sound;
using RaverSoft.YllisanSkies.Characters;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace RaverSoft.YllisanSkies.TitleScreen
{
    public class TitleScreenSystem : MonoBehaviour
    {
        private SoundManager soundManager;
        private Animation anim;
        public Game game;

        private bool hasGameSaves;
        private int currentSectionIndex = 1;
        private int previousSectionIndex = 0;
        private const int PIXELS_TO_MOVE_FOR_SELECTED_SECTION = 64;
        private const int PIXELS_TO_MOVE_IN_ONE_UPDATE = 4;
        private float pixelsToMove = 0;
        private bool inTransition = true;
        private Sections currentSectionOpened = Sections.None;
        private bool isQuittingSection = false;
        public bool inputEnabled { get; private set; }

        private enum Sections
        {
            None,
            NewGame,
            Continue,
            Options,
            Bonus,
            Quit
        }

        // Use this for initialization
        void Start()
        {
            inputEnabled = true;
            game = GameObject.Find("Game").GetComponent<Game>();
            soundManager = GetComponent<SoundManager>();
            anim = GetComponent<Animation>();
            hasGameSaves = game.saveSystem.hasGameSaves();
            setTranslations();
            GameObject.Find("Canvas/Continue").GetComponent<TitleScreenSectionContinue>().loadSaves();
            displaySectionAvailability("Continue", hasGameSaves);
            if (hasGameSaves)
            {
                previousSectionIndex = (int)Sections.NewGame;
                currentSectionIndex = (int)Sections.Continue;
                GameObject previousSection = GameObject.Find("Canvas/Main/Sections/" + (Sections)previousSectionIndex);
                GameObject currentSection = GameObject.Find("Canvas/Main/Sections/" + (Sections)currentSectionIndex);
                currentSection.transform.localPosition = new Vector2(currentSection.transform.localPosition.x - PIXELS_TO_MOVE_FOR_SELECTED_SECTION, currentSection.transform.localPosition.y);
                previousSection.transform.localPosition = new Vector2(previousSection.transform.localPosition.x + PIXELS_TO_MOVE_FOR_SELECTED_SECTION, previousSection.transform.localPosition.y);
            }
            soundManager.fadeOut(0.5f);
            StartCoroutine(playAnimation("FadeOut"));
        }

        private void setTranslations()
        {
            GameObject.Find("Canvas/Main/Sections/NewGame/Section_Title").GetComponent<Text>().text = game.getTranslation("TitleScreen", "New game");
            GameObject.Find("Canvas/Main/Sections/Continue/Section_Title").GetComponent<Text>().text = game.getTranslation("TitleScreen", "Continue");
            GameObject.Find("Canvas/Main/Sections/Options/Section_Title").GetComponent<Text>().text = game.getTranslation("TitleScreen", "Options");
            GameObject.Find("Canvas/Main/Sections/Bonus/Section_Title").GetComponent<Text>().text = game.getTranslation("TitleScreen", "Bonus");
            GameObject.Find("Canvas/Main/Sections/Quit/Section_Title").GetComponent<Text>().text = game.getTranslation("TitleScreen", "Quit");
            GameObject.Find("Canvas/Continue/Section_Block/Section_Title").GetComponent<Text>().text = game.getTranslation("TitleScreen", "Continue");
            for (int i = 1; i <= SaveSystem.MAX_NUMBER_OF_SAVES; i++)
            {
                GameObject.Find("Canvas/Continue/Save" + i + "/ChapterBlock/Title").GetComponent<Text>().text = game.getTranslation("Menu", "Chapter");
                for (int j = 1; j <= HeroesTeam.MAXIMUM_NUMBER_OF_HEROES; j++)
                {
                    GameObject.Find("Canvas/Continue/Save" + i + "/GameInfoBlock/Hero" + j + "/Lv_Title").GetComponent<Text>().text = game.getTranslation("Stats", "Lv");
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (!inTransition)
            {
                if (pixelsToMove > 0)
                {
                    displaySectionMoving();
                }
                else if (currentSectionOpened == Sections.None && inputEnabled)
                {
                    if (Input.GetAxisRaw("Vertical") != 0)
                    {
                        soundManager.playSound(Sounds.Cursor);
                        moveSection();
                    }
                    else if (Input.GetButtonDown("Submit"))
                    {
                        submitSection();
                    }
                }
            }
            if (isQuittingSection)
            {
                currentSectionOpened = Sections.None;
                isQuittingSection = false;
            }
        }

        private IEnumerator playAnimation(string animationName)
        {
            inTransition = true;
            anim.Play(animationName);
            do
            {
                yield return null;
            } while (anim.isPlaying);
            inTransition = false;
        }

        private void submitSection()
        {
            switch (currentSectionIndex)
            {
                case (int)Sections.NewGame:
                    StartCoroutine(newGame());
                    break;
                case (int)Sections.Continue:
                    if (hasGameSaves)
                    {
                        GameObject.Find("Canvas/" + (Sections)currentSectionIndex).GetComponent<TitleScreenSectionContinue>().open();
                        enterSection((Sections)currentSectionIndex);
                    }
                    else
                    {
                        soundManager.playSound(Sounds.Error);
                    }
                    break;
                case (int)Sections.Options:
                    // @TODO
                    break;
                case (int)Sections.Bonus:
                    // @TODO
                    break;
                case (int)Sections.Quit:
                    StartCoroutine(quit());
                    break;
            }
        }

        private void enterSection(Sections section)
        {
            game.playSound(Sounds.Submit);
            GameObject.Find("Canvas/Main").GetComponent<Canvas>().enabled = false;
            GameObject.Find("Canvas/" + section).GetComponent<Canvas>().enabled = true;
            currentSectionOpened = section;
        }

        private int getNumberOfSections()
        {
            return Enum.GetNames(typeof(Sections)).Length - 1;
        }

        private void displaySectionAvailability(string sectionName, bool isAvailable)
        {
            Color saveColor = Color.white;
            if (!isAvailable)
            {
                saveColor = Color.gray;
            }
            GameObject.Find("Canvas/Main/Sections/" + sectionName + "/Section_Title").GetComponent<Text>().color = saveColor;
        }

        private void moveSection()
        {
            if (Input.GetAxisRaw("Vertical") < 0)
            {
                currentSectionIndex++;
                if (currentSectionIndex > getNumberOfSections())
                {
                    currentSectionIndex = 1;
                }
            }
            else if (Input.GetAxisRaw("Vertical") > 0)
            {
                currentSectionIndex--;
                if (currentSectionIndex < 1)
                {
                    currentSectionIndex = getNumberOfSections();
                }
            }
            pixelsToMove = PIXELS_TO_MOVE_FOR_SELECTED_SECTION;
            previousSectionIndex = getPreviousSectionIndex();
        }

        private int getPreviousSectionIndex()
        {
            if (Input.GetAxisRaw("Vertical") < 0)
            {
                previousSectionIndex = currentSectionIndex - 1;
                if (currentSectionIndex == 1)
                {
                    previousSectionIndex = getNumberOfSections();
                }
            }
            else if (Input.GetAxisRaw("Vertical") > 0)
            {
                previousSectionIndex = currentSectionIndex + 1;
                if (currentSectionIndex == getNumberOfSections())
                {
                    previousSectionIndex = 1;
                }
            }
            return previousSectionIndex;
        }

        private void displaySectionMoving()
        {
            GameObject currentSection = GameObject.Find("Canvas/Main/Sections/" + (Sections)currentSectionIndex);
            GameObject previousSection = GameObject.Find("Canvas/Main/Sections/" + (Sections)previousSectionIndex);
            currentSection.transform.localPosition = new Vector2(currentSection.transform.localPosition.x - PIXELS_TO_MOVE_IN_ONE_UPDATE, currentSection.transform.localPosition.y);
            previousSection.transform.localPosition = new Vector2(previousSection.transform.localPosition.x + PIXELS_TO_MOVE_IN_ONE_UPDATE, previousSection.transform.localPosition.y);
            pixelsToMove -= PIXELS_TO_MOVE_IN_ONE_UPDATE;
        }

        private IEnumerator newGame()
        {
            inputEnabled = false;
            soundManager.fadeIn(1.5f);
            soundManager.playSound(Sounds.Submit);
            yield return playAnimation("FadeIn");
            game.setTestStartingMap("Osarian_Outside_01");
        }

        public void continueGame(int saveNumber)
        {
            inputEnabled = false;
            StartCoroutine(load(saveNumber));
        }

        private IEnumerator load(int saveNumber)
        {
            soundManager.fadeIn(1.5f);
            soundManager.playSound(Sounds.Submit);
            yield return playAnimation("FadeIn");
            game.load(saveNumber);
        }

        private IEnumerator quit()
        {
            soundManager.fadeIn(0.5f);
            soundManager.playSound(Sounds.Submit);
            yield return playAnimation("FadeIn");
            Application.Quit();
        }

        public void quitSection()
        {
            game.playSound(Sounds.Cancel);
            GameObject.Find("Canvas/" + currentSectionOpened).GetComponent<Canvas>().enabled = false;
            GameObject.Find("Canvas/Main").GetComponent<Canvas>().enabled = true;
            isQuittingSection = true;
        }
    }
}