using UnityEngine;
using UnityEngine.UI;
using RaverSoft.YllisanSkies.Characters;
using RaverSoft.YllisanSkies.Sound;
using RaverSoft.YllisanSkies.Utils;
using System;

namespace RaverSoft.YllisanSkies.TitleScreen
{
    public class TitleScreenSectionContinue : TitleScreenSection
    {
        private int currentCursorIndex = 1;
        private bool isAxisInUse = false;
        private SaveData[] saves;

        public override void open()
        {
            currentCursorIndex = 1;
            displayBlockSelectionAtCurrentCursorPosition();
            displayCursor(true);
            displaySaves();
            base.open();
        }

        void Update()
        {
            if (isOpened)
            {
                if (Input.GetAxisRaw("Vertical") != 0)
                {
                    if (!isAxisInUse)
                    {
                        titleScreen.game.playSound(Sounds.Cursor);
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
                    
                    quitSection();
                }
                if (Input.GetAxisRaw("Vertical") == 0)
                {
                    isAxisInUse = false;
                }
            }
        }
        
        protected override void quitSection()
        {
            displayCursor(false);
            for (int i = 1; i <= SaveSystem.MAX_NUMBER_OF_SAVES; i++)
            {
                displaySaveDataInfo(i, false);
            }
            base.quitSection();
        }

        public void loadSaves()
        {
            saves = new SaveData[SaveSystem.MAX_NUMBER_OF_SAVES];
            for (int i = 0; i < saves.Length; i++)
            {
                try
                {
                    SaveData saveData = titleScreen.game.saveSystem.load(i + 1);
                    if (saveData != null)
                    {
                        saves[i] = saveData;
                    }
                }
                catch (Exception e)
                {
                }
            }
        }

        private void displaySaves()
        {
            for (int i = 1; i <= SaveSystem.MAX_NUMBER_OF_SAVES; i++)
            {
                displaySave(i);
            }
        }

        private void displaySave(int saveNumber)
        {
            SaveData saveData = saves[saveNumber - 1];
            if (saveData != null)
            {
                GameObject.Find("Canvas/Continue/Save" + saveNumber + "/ScreenshotBlock").GetComponent<RawImage>().texture = saveData.getScreenshot();
                GameObject.Find("Canvas/Continue/Save" + saveNumber + "/ChapterBlock/Number").GetComponent<Text>().text = saveData.chapter.ToString();
                GameObject.Find("Canvas/Continue/Save" + saveNumber + "/ChapterBlock/Date").GetComponent<Text>().text = saveData.date.ToString(); // @TODO : improve date format
                GameObject.Find("Canvas/Continue/Save" + saveNumber + "/GameInfoBlock/Location").GetComponent<Text>().text = saveData.heroesTeam.getCurrentLocation().getName();
                for (int i = 0; i < HeroesTeam.MAXIMUM_NUMBER_OF_HEROES; i++)
                {
                    Hero hero = (Hero)saveData.heroesTeam.getCharacterByIndex(i);
                    bool heroEnabled = false;
                    if (hero != null)
                    {
                        heroEnabled = true;
                        GameObject.Find("Canvas/Continue/Save" + saveNumber + "/GameInfoBlock/Hero" + (i + 1) + "/Sprite").GetComponent<RawImage>().texture = Resources.Load<Texture>("UI/Battles/Battles_Interface_Hero_" + hero.id);
                        GameObject.Find("Canvas/Continue/Save" + saveNumber + "/GameInfoBlock/Hero" + (i + 1) + "/Lv_Stats").GetComponent<Text>().text = hero.getLv().ToString();
                    }
                    GameObject.Find("Canvas/Continue/Save" + saveNumber + "/GameInfoBlock/Hero" + (i + 1)).GetComponent<Canvas>().enabled = heroEnabled;
                }
                GameObject.Find("Canvas/Continue/Save" + saveNumber + "/GameInfoBlock/MoneyBlock/Stats").GetComponent<Text>().text = saveData.heroesTeam.getCurrentMoney().ToString();
                GameObject.Find("Canvas/Continue/Save" + saveNumber + "/GameInfoBlock/TimeBlock/Stats").GetComponent<Text>().text = DateTimeUtils.formatTimeFromSeconds(saveData.playtime);
                displaySaveDataInfo(saveNumber, true);
            }
            else
            {
                // @TODO : display empty slot
                displaySaveDataInfo(saveNumber, false);
            }
        }

        private void displaySaveDataInfo(int saveNumber, bool enabled)
        {
            GameObject.Find("Canvas/Continue/Save" + saveNumber + "/ChapterBlock").GetComponent<Canvas>().enabled = enabled;
            GameObject.Find("Canvas/Continue/Save" + saveNumber + "/GameInfoBlock").GetComponent<Canvas>().enabled = enabled;
            for (int i = 0; i < HeroesTeam.MAXIMUM_NUMBER_OF_HEROES; i++)
            {
                GameObject.Find("Canvas/Continue/Save" + saveNumber + "/GameInfoBlock/Hero" + (i + 1)).GetComponent<Canvas>().enabled = enabled;
            }
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
            currentCursorIndex++;
            if (currentCursorIndex > SaveSystem.MAX_NUMBER_OF_SAVES)
            {
                currentCursorIndex = 1;
            }
        }

        private void moveCursorUp()
        {
            currentCursorIndex--;
            if (currentCursorIndex < 1)
            {
                currentCursorIndex = SaveSystem.MAX_NUMBER_OF_SAVES;
            }
        }

        private void displayBlockSelectionAtCurrentCursorPosition()
        {
            GameObject.Find("Canvas/Continue/SaveSelection").transform.position = GameObject.Find("Canvas/Continue/Save" + currentCursorIndex).transform.position;
        }

        private void displayCursor(bool enabled)
        {
            GameObject.Find("Canvas/Continue/SaveSelection").GetComponent<Canvas>().enabled = enabled;
        }

        private void submitSection()
        {
            SaveData saveData = saves[currentCursorIndex - 1];
            if (saveData != null)
            {
                titleScreen.game.playSound(Sounds.Submit);
                // @TODO : add fade
                titleScreen.game.load(currentCursorIndex);
            }
            else
            {
                titleScreen.game.playSound(Sounds.Error);
            }
        }
    }
}