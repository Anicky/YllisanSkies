using RaverSoft.YllisanSkies.Characters;
using RaverSoft.YllisanSkies.Utils;
using RaverSoft.YllisanSkies.Sound;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RaverSoft.YllisanSkies.Menu
{
    public class MenuSystem : MonoBehaviour
    {

        public bool isOpened = false;
        public Game game;
        public bool inTransition = false;
        private int currentSectionIndex = 1;
        private bool cursorEnabled = false;
        private int currentCursorIndex = 0;
        private bool isAxisInUse = false;
        private bool isMenuOpeningOrClosing = false;
        private Sections currentSectionOpened = Sections.None;
        private bool isQuittingSection = false;

        private enum Sections
        {
            None,
            Items,
            Status,
            Equipment,
            Abilities,
            Airship,
            Journal,
            Options,
            Save,
            Quit
        }

        // Use this for initialization
        private void Start()
        {
            GameObject.Find("Menu/Main").GetComponent<Canvas>().enabled = false;
            GameObject.Find("Menu/Items").GetComponent<Canvas>().enabled = false;
            GameObject.Find("Menu/Status").GetComponent<Canvas>().enabled = false;
            GameObject.Find("Menu/Equipment").GetComponent<Canvas>().enabled = false;
            GameObject.Find("Menu/Abilities").GetComponent<Canvas>().enabled = false;
            GameObject.Find("Menu/Airship").GetComponent<Canvas>().enabled = false;
            GameObject.Find("Menu/Journal").GetComponent<Canvas>().enabled = false;
            GameObject.Find("Menu/Options").GetComponent<Canvas>().enabled = false;
            disableCursor();
        }

        // Update is called once per frame
        private void Update()
        {
            if (isOpened)
            {
                displayTime();
            }
            if (isOpened && !inTransition)
            {
                if (currentSectionOpened == Sections.None)
                {
                    if (cursorEnabled)
                    {
                        if (Input.GetAxisRaw("Vertical") != 0)
                        {
                            if (!isAxisInUse)
                            {
                                game.playSound(Sounds.Cursor);
                                moveCursor();
                                isAxisInUse = true;
                            }
                        }
                        else if (Input.GetButtonDown("Submit"))
                        {
                            submitSection();
                        }
                        else if (Input.GetButtonDown("Cancel"))
                        {
                            game.playSound(Sounds.Cancel);
                            disableCursor();
                        }
                        if (Input.GetAxisRaw("Vertical") == 0)
                        {
                            isAxisInUse = false;
                        }
                    }
                    else
                    {
                        if (Input.GetAxisRaw("Vertical") != 0)
                        {
                            game.playSound(Sounds.Cursor);
                            moveSection();
                        }
                        else if (Input.GetButtonDown("Submit"))
                        {
                            checkIfCursorOrAction();
                        }
                        else if (Input.GetButtonDown("Cancel"))
                        {
                            close();
                        }
                    }
                }
            }
            if (isQuittingSection)
            {
                currentSectionOpened = Sections.None;
                isQuittingSection = false;
            }
        }

        private void LateUpdate()
        {
            if (isMenuOpeningOrClosing && currentSectionIndex != 1)
            {
                GameObject.Find("Menu/Main/Section_01").transform.localPosition += new Vector3(64, 0, 0);
                GameObject.Find("Menu/Main/Section_0" + currentSectionIndex).transform.localPosition -= new Vector3(64, 0, 0);
            }
        }

        private void submitSection()
        {
            switch (currentSectionIndex)
            {
                case (int)Sections.Items:
                    enterSection((Sections)currentSectionIndex);
                    GameObject.Find("Menu/" + (Sections)currentSectionIndex).GetComponent<MenuSectionItems>().open();
                    break;
                case (int)Sections.Status:
                    enterSection((Sections)currentSectionIndex);
                    GameObject.Find("Menu/" + (Sections)currentSectionIndex).GetComponent<MenuSectionStatus>().open(getHeroFromCursorIndex());
                    break;
                case (int)Sections.Equipment:
                    enterSection((Sections)currentSectionIndex);
                    GameObject.Find("Menu/" + (Sections)currentSectionIndex).GetComponent<MenuSectionEquipment>().open(getHeroFromCursorIndex());
                    break;
                case (int)Sections.Abilities:
                    enterSection((Sections)currentSectionIndex);
                    GameObject.Find("Menu/" + (Sections)currentSectionIndex).GetComponent<MenuSectionAbilities>().open(getHeroFromCursorIndex());
                    break;
                case (int)Sections.Airship:
                    enterSection((Sections)currentSectionIndex);
                    GameObject.Find("Menu/" + (Sections)currentSectionIndex).GetComponent<MenuSectionAirship>().open();
                    break;
                case (int)Sections.Journal:
                    enterSection((Sections)currentSectionIndex);
                    GameObject.Find("Menu/" + (Sections)currentSectionIndex).GetComponent<MenuSectionJournal>().open();
                    break;
                case (int)Sections.Options:
                    enterSection((Sections)currentSectionIndex);
                    GameObject.Find("Menu/" + (Sections)currentSectionIndex).GetComponent<MenuSectionOptions>().open();
                    break;
                case (int)Sections.Save:
                    if (game.isSaveAllowed)
                    {
                        game.playSound(Sounds.Submit);
                        // @TODO
                    }
                    else
                    {
                        game.playSound(Sounds.Error);
                        // @TODO : play Error sound
                    }
                    break;
                case (int)Sections.Quit:
                    // @TODO
                    game.playSound(Sounds.Submit);
                    break;
            }
        }

        private void enterSection(Sections section)
        {
            game.playSound(Sounds.Submit);
            GameObject.Find("Menu/Main").GetComponent<Canvas>().enabled = false;
            GameObject.Find("Menu/" + section).GetComponent<Canvas>().enabled = true;
            currentSectionOpened = section;
        }

        public void quitSection()
        {
            game.playSound(Sounds.Cancel);
            disableCursor();
            GameObject.Find("Menu/" + currentSectionOpened).GetComponent<Canvas>().enabled = false;
            GameObject.Find("Menu/Main").GetComponent<Canvas>().enabled = true;
            isQuittingSection = true;
        }

        private Hero getHeroFromCursorIndex()
        {
            return (Hero)game.heroesTeam.getCharacterByIndex(currentCursorIndex - 1);
        }

        private void moveCursor()
        {
            if (Input.GetAxisRaw("Vertical") < 0)
            {
                moveCursorDown();
            }
            else if (Input.GetAxisRaw("Vertical") > 0)
            {
                moveCursorUp();
            }
            displayBlockSelectionAtCurrentCursorPosition();
        }

        private void moveCursorDown()
        {
            do
            {
                currentCursorIndex++;
                if (currentCursorIndex > 4)
                {
                    currentCursorIndex = 1;
                }
            } while (getHeroFromCursorIndex() == null);
        }

        private void moveCursorUp()
        {
            do
            {
                currentCursorIndex--;
                if (currentCursorIndex < 1)
                {
                    currentCursorIndex = 4;
                }
            } while (getHeroFromCursorIndex() == null);
        }

        private void checkIfCursorOrAction()
        {
            List<int> sectionsWithCursor = new List<int> { 2, 3, 4 };
            if (sectionsWithCursor.Contains(currentSectionIndex))
            {
                game.playSound(Sounds.Submit);
                enableCursor();
            }
            else
            {
                submitSection();
            }
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
            StartCoroutine(changeSection(Input.GetAxisRaw("Vertical")));
        }

        private void displayBlockSelectionAtCurrentCursorPosition()
        {
            GameObject.Find("Menu/Main/Block_Selection").transform.position = GameObject.Find("Menu/Main/Block_Hero" + currentCursorIndex).transform.position;
        }

        private void enableCursor()
        {
            currentCursorIndex = 0;
            moveCursorDown();
            displayBlockSelectionAtCurrentCursorPosition();
            GameObject.Find("Menu/Main/Block_Selection").GetComponent<Canvas>().enabled = true;
            cursorEnabled = true;
        }

        private void disableCursor()
        {
            GameObject.Find("Menu/Main/Block_Selection").GetComponent<Canvas>().enabled = false;
            cursorEnabled = false;
        }

        public void open()
        {
            game.playSound(Sounds.Cancel);
            if (!game.options.menuMemorizeSectionIndex)
            {
                currentSectionIndex = 1;
            }
            disableCursor();
            displayHeroes();
            displayMoney();
            displayTime();
            displayLocation();
            displaySave();
            StartCoroutine(openMenu());
        }

        public void close()
        {
            game.playSound(Sounds.Cancel);
            StartCoroutine(closeMenu());
        }

        private void displayMoney()
        {
            GameObject.Find("Menu/Main/Block_Money/Money_Stats").GetComponent<Text>().text = game.heroesTeam.getCurrentMoney().ToString();
        }

        private void displayTime()
        {
            GameObject.Find("Menu/Main/Block_Time/Time_Stats").GetComponent<Text>().text = DateTimeUtils.formatTimeFromSeconds(game.getPlaytime());
        }

        private void displayLocation()
        {
            GameObject.Find("Menu/Main/Block_Location/Location_Title").GetComponent<Text>().text = game.getTranslation("Locations", game.heroesTeam.getCurrentLocation().getName());
        }

        private void displaySave()
        {
            Color saveColor = Color.white;
            if (!game.isSaveAllowed)
            {
                saveColor = Color.gray;
            }
            GameObject.Find("Menu/Main/Section_08/Section_Title").GetComponent<Text>().color = saveColor;
        }

        private void displayHeroes()
        {
            for (int i = 0; i < HeroesTeam.MAXIMUM_NUMBER_OF_HEROES; i++)
            {
                handleHeroBlock(i + 1, (Hero)game.heroesTeam.getCharacterByIndex(i));
            }
        }

        private void handleHeroBlock(int heroPosition, Hero hero)
        {
            bool enabled = false;
            if (hero != null)
            {
                enabled = true;
            }
            string elementsGroupName = "Menu/Main/Block_Hero" + heroPosition;
            GameObject.Find(elementsGroupName).GetComponent<Canvas>().enabled = enabled;
            if (enabled)
            {
                GameObject.Find(elementsGroupName + "/Hero_Name").GetComponent<Text>().text = hero.name;
                GameObject.Find(elementsGroupName + "/Lv_Stats").GetComponent<Text>().text = hero.getLv().ToString();
                GameObject.Find(elementsGroupName + "/Hp_Stats").GetComponent<Text>().text = hero.hp.ToString() + "/" + hero.hpMax.ToString();
                GameObject.Find(elementsGroupName + "/Ap_Stats").GetComponent<Text>().text = hero.ap.ToString() + "/" + hero.apMax.ToString();
                GameObject.Find(elementsGroupName + "/Hp_Gauge").GetComponent<Image>().fillAmount = (float)hero.hp / hero.hpMax;
                GameObject.Find(elementsGroupName + "/Ap_Gauge").GetComponent<Image>().fillAmount = (float)hero.ap / hero.apMax;
                GameObject.Find(elementsGroupName + "/Hero_Sprite").GetComponent<RawImage>().texture = Resources.Load<Texture>("UI/Sprite_Hero_" + hero.id);
            }
        }

        private IEnumerator openMenu()
        {
            inTransition = true;
            isMenuOpeningOrClosing = true;
            game.player.disableMovement();
            GameObject.Find("Menu/Main").GetComponent<Canvas>().enabled = true;
            Animation anim = GameObject.Find("Menu").GetComponent<Animation>();
            anim.Play("Menu_Open");
            do
            {
                yield return null;
            } while (anim.isPlaying);
            isOpened = true;
            isMenuOpeningOrClosing = false;
            inTransition = false;
        }

        private IEnumerator closeMenu()
        {
            inTransition = true;
            isMenuOpeningOrClosing = true;
            Animation anim = GameObject.Find("Menu").GetComponent<Animation>();
            anim.Play("Menu_Close");
            do
            {
                yield return null;
            } while (anim.isPlaying);
            isOpened = false;
            GameObject.Find("Menu/Main").GetComponent<Canvas>().enabled = false;
            game.player.enableMovement();
            isMenuOpeningOrClosing = false;
            inTransition = false;
            game.stopEvents = false;
        }

        private IEnumerator changeSection(float axis)
        {
            inTransition = true;
            int previousSectionIndex = 0;
            if (axis < 0)
            {
                previousSectionIndex = currentSectionIndex - 1;
                if (currentSectionIndex == 1)
                {
                    previousSectionIndex = getNumberOfSections();
                }
            }
            else if (axis > 0)
            {
                previousSectionIndex = currentSectionIndex + 1;
                if (currentSectionIndex == getNumberOfSections())
                {
                    previousSectionIndex = 1;
                }
            }
            Animation anim = GameObject.Find("Menu/Main").GetComponent<Animation>();
            anim.Play("Menu_Section_Current_0" + currentSectionIndex);
            anim["Menu_Section_Previous_0" + previousSectionIndex].layer = 1;
            anim.Play("Menu_Section_Previous_0" + previousSectionIndex);
            do
            {
                yield return null;
            } while (anim.isPlaying);
            inTransition = false;
        }

        private int getNumberOfSections()
        {
            return Enum.GetNames(typeof(Sections)).Length - 1;
        }

    }
}