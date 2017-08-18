using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections.Generic;

public class Menu : MonoBehaviour
{

    public bool isOpened = false;
    public Game game;
    public bool inTransition = false;
    private int currentSectionIndex = 1;
    private int numberOfSections = 9;
    private bool cursorEnabled = false;
    private int currentCursorIndex = 0;
    private bool isAxisInUse = false;
    private bool isMenuOpeningOrClosing = false;

    // Use this for initialization
    private void Start()
    {
        GameObject.Find("Menu/Main").GetComponent<Canvas>().enabled = false;
        GameObject.Find("Menu/Main/Block_Selection").GetComponent<Canvas>().enabled = false;
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
            if (cursorEnabled)
            {
                if (Input.GetAxisRaw("Vertical") != 0)
                {
                    if (!isAxisInUse)
                    {
                        moveCursor();
                        isAxisInUse = true;
                    }
                }
                else if (Input.GetButtonDown("Submit"))
                {
                    enterSection();
                }
                else if (Input.GetButtonDown("Cancel"))
                {
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

    private void LateUpdate()
    {
        if (isMenuOpeningOrClosing && currentSectionIndex != 1)
        {
            GameObject.Find("Menu/Main/Section_01").transform.position += new Vector3(29, 0, 0);
            GameObject.Find("Menu/Main/Section_0" + currentSectionIndex).transform.position -= new Vector3(29, 0, 0);
        }
    }

    private void enterSection()
    {
        // @TODO
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
        } while (game.heroes[currentCursorIndex - 1] == null);
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
        } while (game.heroes[currentCursorIndex - 1] == null);
    }

    private void checkIfCursorOrAction()
    {
        List<int> sectionsWithCursor = new List<int> { 2, 3, 4 };
        if (sectionsWithCursor.Contains(currentSectionIndex))
        {
            enableCursor();
        }
        else
        {
            enterSection();
        }
    }

    private void moveSection()
    {
        if (Input.GetAxisRaw("Vertical") < 0)
        {
            currentSectionIndex++;
            if (currentSectionIndex > numberOfSections)
            {
                currentSectionIndex = 1;
            }
        }
        else if (Input.GetAxisRaw("Vertical") > 0)
        {
            currentSectionIndex--;
            if (currentSectionIndex < 1)
            {
                currentSectionIndex = numberOfSections;
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
        if (!game.options.menuMemorizeSectionIndex)
        {
            currentSectionIndex = 1;
        }
        disableCursor();
        displayHeroes();
        displayMoney();
        displayTime();
        displayLocation();
        StartCoroutine(openMenu());
    }

    public void close()
    {
        StartCoroutine(closeMenu());
    }

    private void displayMoney()
    {
        GameObject.Find("Menu/Main/Block_Money/Money_Stats").GetComponent<Text>().text = game.currentMoney.ToString();
    }

    private void displayTime()
    {
        int time = (int)Time.time;
        int hours = time / (60 * 60);
        int minutes = time / 60;
        int seconds = time % 60;

        string hoursToString = hours.ToString();
        if (hours < 10)
        {
            hoursToString = "0" + hoursToString;
        }
        string minutesToString = minutes.ToString();
        if (minutes < 10)
        {
            minutesToString = "0" + minutesToString;
        }
        string secondsToString = seconds.ToString();
        if (seconds < 10)
        {
            secondsToString = "0" + secondsToString;
        }

        GameObject.Find("Menu/Main/Block_Time/Time_Stats").GetComponent<Text>().text = hoursToString + ":" + minutesToString + ":" + secondsToString;
    }

    private void displayLocation()
    {
        GameObject.Find("Menu/Main/Block_Location/Location_Title").GetComponent<Text>().text = game.getTranslation("Location", "Forest of Hopes");
    }

    private void displayHeroes()
    {
        for (int i = 0; i < game.heroes.Length; i++)
        {
            Hero hero = game.heroes[i];
            handleHeroBlock(i + 1, hero);
        }
    }

    private void handleHeroBlock(int heroPosition, Hero hero)
    {
        bool enabled = false;
        if (hero != null)
        {
            enabled = true;
        }

        GameObject block = GameObject.Find("Menu/Main/Block_Hero" + heroPosition);
        Image blockRootImage = block.GetComponent<Image>();
        blockRootImage.enabled = enabled;
        Text[] blockTexts = block.GetComponentsInChildren<Text>();
        Image[] blockImages = block.GetComponentsInChildren<Image>();
        RawImage[] rawImages = block.GetComponentsInChildren<RawImage>();
        foreach (Text blockText in blockTexts)
        {
            blockText.enabled = enabled;
            if (enabled)
            {
                if (blockText.name == "Hero_Name")
                {
                    blockText.text = hero.name;
                }
                else if (blockText.name == "Lv_Stats")
                {
                    blockText.text = hero.lv.ToString();
                }
                else if (blockText.name == "Hp_Stats")
                {
                    blockText.text = hero.hp.ToString() + "/" + hero.hpMax.ToString();
                }
                else if (blockText.name == "Ap_Stats")
                {
                    blockText.text = hero.ap.ToString() + "/" + hero.apMax.ToString();
                }
            }
        }
        foreach (Image blockImage in blockImages)
        {
            blockImage.enabled = enabled;
            if (enabled)
            {
                if (blockImage.name == "Hp_Gauge")
                {
                    blockImage.fillAmount = (float)hero.hp / hero.hpMax;
                }
                else if (blockImage.name == "Ap_Gauge")
                {
                    blockImage.fillAmount = (float)hero.ap / hero.apMax;
                }
            }
        }
        foreach (RawImage rawImage in rawImages)
        {
            rawImage.enabled = enabled;
            if (enabled)
            {
                if (rawImage.name == "Hero_Sprite")
                {
                    rawImage.texture = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/UI/Sprite_Hero_" + hero.name + ".png", typeof(Texture2D));
                }
            }
        }
    }

    private IEnumerator openMenu()
    {
        inTransition = true;
        isMenuOpeningOrClosing = true;
        game.player.disableMovement();
        GameObject.Find("Menu/Main").GetComponent<Canvas>().enabled = true;
        Animation anim = GameObject.Find("Menu/Main").GetComponent<Animation>();
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
        Animation anim = GameObject.Find("Menu/Main").GetComponent<Animation>();
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
                previousSectionIndex = numberOfSections;
            }
        }
        else if (axis > 0)
        {
            previousSectionIndex = currentSectionIndex + 1;
            if (currentSectionIndex == numberOfSections)
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

}
