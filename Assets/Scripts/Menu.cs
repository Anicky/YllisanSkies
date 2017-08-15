using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections.Generic;

public class Menu : MonoBehaviour
{

    public bool isOpened = false;
    private Canvas canvas;
    public Game game;
    public bool inTransition = false;
    private int currentSectionIndex = 1;
    private int numberOfSections = 9;
    private bool cursorEnabled = false;
    private int currentCursorIndex = 1;
    private bool isAxisInUse = false;

    // Use this for initialization
    private void Start()
    {
        canvas = this.GetComponent<Canvas>();
    }

    // Update is called once per frame
    private void Update()
    {
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

    private void enterSection()
    {
        // @TODO
    }

    private void moveCursor()
    {
        if (Input.GetAxisRaw("Vertical") < 0)
        {
            currentCursorIndex++;
            if (currentCursorIndex > game.getNumberOfHeroes())
            {
                currentCursorIndex = 1;
            }
        }
        else if (Input.GetAxisRaw("Vertical") > 0)
        {
            currentCursorIndex--;
            if (currentCursorIndex < 1)
            {
                currentCursorIndex = game.getNumberOfHeroes();
            }
        }
        displayBlockSelectionAtCurrentCursorPosition();
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
        currentCursorIndex = 1;
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
        currentSectionIndex = 1;
        disableCursor();
        showHeroesStats();
        StartCoroutine(openMenu());
    }

    public void close()
    {
        StartCoroutine(closeMenu());
    }

    private void showHeroesStats()
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
        game.player.disableMovement();
        GameObject.Find("Menu/Main").GetComponent<Canvas>().enabled = true;
        Animation anim = GameObject.Find("Menu/Main").GetComponent<Animation>();
        anim.Play("Menu_Open");
        do
        {
            yield return null;
        } while (anim.isPlaying);
        isOpened = true;
        inTransition = false;
    }

    private IEnumerator closeMenu()
    {
        inTransition = true;
        Animation anim = GameObject.Find("Menu/Main").GetComponent<Animation>();
        anim.Play("Menu_Close");
        do
        {
            yield return null;
        } while (anim.isPlaying);
        isOpened = false;
        GameObject.Find("Menu/Main").GetComponent<Canvas>().enabled = false;
        game.player.enableMovement();
        inTransition = false;
    }

    private IEnumerator changeSection(float axe)
    {
        inTransition = true;
        int previousSectionIndex = 0;
        if (axe < 0)
        {
            previousSectionIndex = currentSectionIndex - 1;
            if (currentSectionIndex == 1)
            {
                previousSectionIndex = numberOfSections;
            }
        }
        else if (axe > 0)
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
